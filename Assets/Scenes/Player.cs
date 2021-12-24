using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Text;
using System.Security.Cryptography;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

/* ========= IP位址 =============================== */
public static class IPManager
{
    public static string GetIP(ADDRESSFAM Addfam)
    {
        //Return null if ADDRESSFAM is Ipv6 but Os does not support it
        if (Addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6)
        {
            return null;
        }

        string output = "";
        int count = 0;

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
#endif 
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    //IPv4
                    if (Addfam == ADDRESSFAM.IPv4)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {

                            if (item.Name == "Hamachi")
                            {
                                output = ip.Address.ToString();
                                //Debug.Log("擷取" + output);
                            }
                            count++;
                        }
                    }
                    //IPv6
                    else if (Addfam == ADDRESSFAM.IPv6)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            output = ip.Address.ToString();
                        }
                    }                    
                }
            }
        }
        return output;
    }
}

public enum ADDRESSFAM
{
    IPv4, IPv6
}
/* ================================================ */

public class Player : NetworkBehaviour
{
    GM gm;
    public GameObject button_control;
    public GameObject player;
    
    public string IP;
    public int ID;
    public int rival_ID;
    public int room_number;
    public string state;
    public string battle_position;
    public string type;
    public bool win = false;

    void Start()
    {
        DontDestroyOnLoad(this);

        //讓server新增 Player
        if (isServer)
        {
            IP = IPManager.GetIP(ADDRESSFAM.IPv4);
            gm = GameObject.Find("GM").GetComponent<GM>();
            gm.Login(this);
        }

        if (isLocalPlayer)
        {
            //IP = IPManager.GetIP(ADDRESSFAM.IPv4);
            //Debug.Log(IP);
            //Debug.Log("hey");
            //gm = GameObject.Find("GM").GetComponent<GM>();
            //gm.Login(this);
        }
    }

    void Update()
    {
        /*if (!NetworkClient.active)
        {
            if (isServer)
            {
                IP = IPManager.GetIP(ADDRESSFAM.IPv4);
                gm = GameObject.Find("GM").GetComponent<GM>();
                gm.Login(this);
            }
        }*/
        //判斷是否切換到island場景
        if (SceneManager.GetActiveScene().name == "island")
        {
            player.GetComponent<island_UI>().enabled = true;
        }
        else if (SceneManager.GetActiveScene().name == "battle")
        {
            player.GetComponent<battle_UI>().enabled = true;
        }
    }

    //設定Player的ID
    [ClientRpc]
    public void RpcSetID(int id, string allid, string allip)
    {
        ID = id - 1;
        button_control.GetComponent<Button_control>().SetIDText(ID);
        
        if (isLocalPlayer)
        {
            IP = IPManager.GetIP(ADDRESSFAM.IPv4);
            
            //啟動 .net core 區塊鏈
            string dotnet_command = $"{IP} {(6000 + ID).ToString()} user{ID.ToString()} {allid} {allip}";
            Debug.Log(dotnet_command);

            Thread net_core = new Thread(new ParameterizedThreadStart(BlockChain.Program.Initialize));
            net_core.Start(dotnet_command);
        }
    }
    
    [ClientRpc]
    public void RpcSetType(string tribe)
    {
        type = tribe;
    }
    
    //把Player選的種族 傳到Server讓 所有Client端的資訊也能更新
    [Command]
    public void CmdChoose_Dragon()
    {
        if (isServer)
        {
            gm.Distribue_Type(this, "龍族");
        }
    }

    [Command]
    public void CmdChoose_Human()
    {
        if (isServer)
        {
            gm.Distribue_Type(this, "人族");
        }
    }

    [Command]
    public void CmdChoose_GOD()
    {
        if (isServer)
        {
            gm.Distribue_Type(this, "神族");
        }
    }
    
    public void Conquer(string place)
    {
        BlockChain.Program.AddBlock(ID, type, place);
    }

    public int[] Show_Bulletin(string place)
    {
        return BlockChain.Program.Show_Block(place);
    }

    //將搜尋對戰的玩家加入GM裡的等待列表
    [Command]
    public void CmdStart_Battle()
    {
        if (isServer)
        {
            gm = GameObject.Find("GM").GetComponent<GM>();
            gm.AddBattlePlayer(this);
        }
    }

    [ClientRpc]
    public void RpcSetState(string msg)
    {
        state = msg;

        if (isLocalPlayer && msg == "等待玩家...")
        {
            state = msg;
            player.GetComponent<island_UI>().SetSysMsg(msg);
        }
        else if (isLocalPlayer && msg == "開始戰鬥")
        {
            state = msg;
            player.GetComponent<island_UI>().Change_scene();
        }
    }

    [ClientRpc]
    public void RpcSetRoom(int room)
    {
        room_number = room;
    }

    [ClientRpc]
    public void RpcSetRival_ID_and_position(int rival, string pos)
    {
        rival_ID = rival;
        battle_position = pos;
    }

    [ClientRpc]
    public void RpcBattle_Schedule()
    {
        if (isLocalPlayer)
        {
            //準備階段 開啟倒數計時器
            if (player.GetComponent<battle_UI>().start_timer == true)
            {

                //將不是自己領土的按鈕 設為false
                if (player.GetComponent<battle_UI>().round > 0)
                {
                    
                    if (battle_position == "left")
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if (player.GetComponent<battle_UI>().checker[i][j].GetComponent<Image>().color == Color.blue)
                                {
                                    player.GetComponent<battle_UI>().checker[i][j].gameObject.SetActive(false);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if (player.GetComponent<battle_UI>().checker[i][j].GetComponent<Image>().color == Color.red)
                                {
                                    player.GetComponent<battle_UI>().checker[i][j].gameObject.SetActive(false);
                                }
                            }
                        }
                    }
                }


                CmdSetState("prepare");
                InvokeRepeating("CountDown", 1, 1);

                player.GetComponent<battle_UI>().start_timer = false;

            }

            //回合開始
            if (player.GetComponent<battle_UI>().prepare == false)
            {
                CancelInvoke("CountDown");


                //對戰結束
                if (player.GetComponent<battle_UI>().battle_end == true)
                {
                    if (player.GetComponent<battle_UI>().round < 5)
                    {
                        player.GetComponent<battle_UI>().battle_end = false;

                        CmdSetState("round_end");
                    }

                    else
                    {
                        CmdSetState("battle_end");
                    }

                }
            }
        }
    }

    //倒數計時函式 去呼叫battle_UI裡的倒數計時
    public void CountDown()
    {
        player.GetComponent<battle_UI>().CountDown();
    }

    //設定對戰狀態
    [Command]
    public void CmdSetState(string s)
    {
        gm = GameObject.Find("GM").GetComponent<GM>();
        gm.SetState(this, s);
    }

    //使模式 從對戰結束階段 切換成放旗子階段
    [ClientRpc]
    public void RpcPrepare()
    {
        if (isLocalPlayer)
        {
            player.GetComponent<battle_UI>().prepare = true;
            player.GetComponent<battle_UI>().start_timer = true;
        }
    }
    
    //--------------------------------傳位置到GM
    [Command]
    public void Cmdupdate_my_pos(string[] pos)
    {
        if (isServer)
        {
            gm = GameObject.Find("GM").GetComponent<GM>();
            gm.update_archer_pos(pos, room_number, this);

        }
    }
}

namespace BlockChain
{
    class Program
    {
        public static int Port = 0;
        public static P2PServer Server = null;
        public static P2PClient Client = new P2PClient();
        public static Blockchain blockchain = new Blockchain();
        public static string IPAddress = "", name = "Unknown";

        public static void Initialize(object arg) //啟動server時會輸入 dotnet Blockchain.dll 6001 user1
        {
            blockchain.InitializeChain();
            string allid = "", allip = "";

            string[] args = ((string)arg).Split(' ');
            // args = $"{IP} {(6000 + ID).ToString()} user{ID.ToString()} {allid} {allip}"

            if (args.Length >= 1)
                IPAddress = args[0];
            if (args.Length >= 2)
                Port = int.Parse(args[1]);          //args[0] = 6001
            if (args.Length >= 3)
                name = args[2];                     //args[1] = user1
            if (args.Length >= 4)
                allid = args[3];
            if (args.Length >= 5)
                allip = args[4];

            /* 將ID及其對應的IP位址存成Dictionary */
            Dictionary<string, string> IPDict = new Dictionary<string, string>();
            string[] ports = allid.Split(',');
            string[] ips = allip.Split(',');
            for (int i = 0; i < ports.Length; i++)
            {
                IPDict.Add(ports[i], ips[i]);
            }

            //創建server
            if (Port > 0)
            {
                Server = new P2PServer();
                Server.Start();
            }
            if (name != "Unkown")
            {
                Debug.Log($"Current user is {name}");
            }

            //每個新連進來的都去connect之前的Player
            /*for (int i = 6001; i < Port; i++)
            {
                Connect_Server("ws://127.0.0.1:" + i.ToString());
            }*/
            if (Port != 6001)
            {
                foreach (var item in IPDict)
                {
                    Connect_Server("ws://" + item.Value + ":" + item.Key);
                }
            }
        }

        //連接新的peer
        public static void Connect_Server(string URL)
        {
            //Game
            Client.Connect($"{URL}/Game");
            Debug.Log($"Connect to {URL}");
        }

        //點擊佔領 新增區塊鏈
        public static void AddBlock(int id, string type, string place)
        {
            blockchain.CreateTransaction(new Transaction(id, type, place));
            blockchain.ProcessPendingTransactions(name);
            Client.Broadcast(JsonConvert.SerializeObject(blockchain));
        }

        //點擊顯示 得到區塊鏈內容
        public static int[] Show_Block(string show_place)
        {
            int[] point = { 0, 0, 0 };
            string place;
            string type;

            if (blockchain.Chain.Count > 1)
            {
                //遍立區塊鏈取得內容
                for (int i = 1; i < blockchain.Chain.Count; i++)
                {
                    place = blockchain.Chain[i].Transactions[0].Place;
                    type = blockchain.Chain[i].Transactions[0].Type;

                    //依照想看的區域 統計分數
                    if (place == show_place)
                    {
                        if (type == "龍族")
                        {
                            point[0]++;
                        }
                        else if (type == "人族")
                        {
                            point[1]++;
                        }
                        else if (type == "神族")
                        {
                            point[2]++;
                        }
                    }
                }
            }
            return point;
        }
    }

    public class P2PServer : WebSocketBehavior
    {
        bool chainSynched = false;
        WebSocketServer wss = null;

        public void Start()
        {
            //wss = new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
            wss = new WebSocketServer($"ws://{Program.IPAddress}:{Program.Port}");
            wss.AddWebSocketService<P2PServer>("/Game");
            wss.Start();
            //Debug.Log($"Started server at ws://127.0.0.1:{Program.Port}");
            Debug.Log($"Started server at ws://{Program.IPAddress}:{Program.Port}");
            //Debug.Log("啟動server");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data.Substring(0, 9) == "Hi Server")
            {
                Debug.Log(e.Data);
                Send("Hi Client");

                //Program.Connect_Server("ws://127.0.0.1:" + e.Data.Split(' ')[2]);
                Program.Connect_Server("ws://" + e.Data.Split(' ')[2] + ":" + e.Data.Split(' ')[3]);
                //Program.Connect_Server("ws://127.0.0.1:6002");
            }
            else
            {
                Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

                if (newChain.IsValid() && newChain.Chain.Count > Program.blockchain.Chain.Count)
                {
                    List<Transaction> newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newChain.PendingTransactions);
                    newTransactions.AddRange(Program.blockchain.PendingTransactions);

                    newChain.PendingTransactions = newTransactions;
                    Program.blockchain = newChain;
                }

                if (!chainSynched)
                {
                    Send(JsonConvert.SerializeObject(Program.blockchain));
                    chainSynched = true;
                }
            }
        }
    }

    public class P2PClient
    {
        IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();

        public void Connect(string url)
        {
            Debug.Log(wsDict.Count);

            if (!wsDict.ContainsKey(url))
            {
                WebSocket ws = new WebSocket(url);
                ws.OnMessage += (sender, e) =>
                {
                    if (e.Data == "Hi Client")
                    {
                        Debug.Log(e.Data);
                    }
                    else
                    {
                        Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                        if (newChain.IsValid() && newChain.Chain.Count > Program.blockchain.Chain.Count)
                        {
                            List<Transaction> newTransactions = new List<Transaction>();
                            newTransactions.AddRange(newChain.PendingTransactions);
                            newTransactions.AddRange(Program.blockchain.PendingTransactions);

                            newChain.PendingTransactions = newTransactions;
                            Program.blockchain = newChain;
                        }
                    }
                };

                Debug.Log(url);
                ws.Connect();

                ws.Send($"Hi Server {Program.IPAddress} {Program.Port}");
                ws.Send(JsonConvert.SerializeObject(Program.blockchain));
                wsDict.Add(url, ws);
            }
        }

        public void Send(string url, string data)
        {
            foreach (var item in wsDict)
            {
                if (item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }

        public void Broadcast(string data)
        {
            foreach (var item in wsDict)
            {
                Debug.Log(item.Key);
                item.Value.Send(data);
            }
        }

        public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach (var item in wsDict)
            {
                servers.Add(item.Key);
            }
            return servers;
        }

        public void Close()
        {
            foreach (var item in wsDict)
            {
                item.Value.Close();
            }
        }
    }

    public class Blockchain
    {
        public IList<Transaction> PendingTransactions = new List<Transaction>();    //Transaction類別
        public IList<Block> Chain { set; get; }    //Block.cs
        public int Difficulty { set; get; } = 1; // 工作量證明演算法的困難值
        public int Reward = 1; //1 cryptocurrency

        public Blockchain()
        {

        }

        /* 區塊鏈初始化 */
        public void InitializeChain()
        {
            Chain = new List<Block>();
            AddGenesisBlock(); // 
        }

        /* 把初始Block跟Blockchain串在一起 */
        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
            Debug.Log(JsonConvert.SerializeObject(Chain, Formatting.Indented));
        }

        /* 建立初始Block*/
        public Block CreateGenesisBlock()
        {
            Block block = new Block(DateTime.Now, null, PendingTransactions);
            block.Mine(Difficulty);
            PendingTransactions = new List<Transaction>();
            return block;
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        /* 建立一筆交易 */
        public void CreateTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }

        public void ProcessPendingTransactions(string minerAddress)
        {
            Block block = new Block(DateTime.Now, GetLatestBlock().Hash, PendingTransactions);
            AddBlock(block);

            PendingTransactions = new List<Transaction>();
            //CreateTransaction(new Transaction(null, minerAddress, 0));
        }

        /* 計算Block中的資訊並將其串到Blockchain上 */
        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            //block.Hash = block.CalculateHash();
            block.Mine(this.Difficulty);
            Chain.Add(block);
        }

        /* 檢查
         * ‧Each block's hash to see if the block is changed
         * ‧Previous block's hash to see if the block is changed and recalculated */
        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                    return false;

                if (currentBlock.PreviousHash != previousBlock.Hash)
                    return false;
            }
            return true;
        }

        /*public int GetBalance(string address)
        {
            int balance = 0;

            for (int i = 0; i < Chain.Count; i++)
            {
                for (int j = 0; j < Chain[i].Transactions.Count; j++)
                {
                    var transaction = Chain[i].Transactions[j];

                    if (transaction.FromAddress == address)
                    {
                        balance -= transaction.Amount;
                    }

                    if (transaction.ToAddress == address)
                    {
                        balance += transaction.Amount;
                    }
                }
            }

            return balance;
        }*/
    }

    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public IList<Transaction> Transactions { get; set; }
        public int Nonce { get; set; } = 0; //隨機數，用於工作量證明演算法的計數器

        public Block(DateTime timeStamp, string previousHash, IList<Transaction> transactions)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Transactions = transactions;
        }

        /* 計算Hash */
        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{JsonConvert.SerializeObject(Transactions)}-{Nonce}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }

        /* 挖礦 */
        public void Mine(int difficulty)
        {
            //困難度 Hash值要符合特定規則 e.g. 前四個數字要是 0000
            var leadingZeros = new string('0', difficulty);

            while (this.Hash == null || this.Hash.Substring(0, difficulty) != leadingZeros)
            {
                this.Nonce++;
                this.Hash = this.CalculateHash();
            }
        }
    }

    public class Transaction
    {
        public string Place { get; set; }
        public string Type { get; set; }
        public int ID { get; set; }

        /* 交易內容 */
        public Transaction(int id, string type, string place)
        {
            ID = id;
            Type = type;
            Place = place;
        }
    }
}
