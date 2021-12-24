using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;


public class GM : NetworkBehaviour
{
    public List<Player> allPlayer = new List<Player>();
    public List<Player> wait_for_battle = new List<Player>();
    public List<List<Player>> Battle_Room = new List<List<Player>>();

    public List<List<List<string>>> chess_attack_pos = new List<List<List<string>>>();

    List<string> allID = new List<string>();
    List<string> allIP = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        //新增對戰房間
        for (int i = 0; i < 20; i++)
        {
            chess_attack_pos.Add(new List<List<string>>());
            Battle_Room.Add(new List<Player>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //判斷目前在等待對戰的有幾個人
        //設定狀態訊息
        if (wait_for_battle.Count < 2)
        {
            foreach (Player p1 in wait_for_battle)
            {
                p1.RpcSetState("等待玩家...");
            }
        }
        //如果等待列表有2個人以上 就進入戰鬥頁面
        else
        {
            int room_number = 0;
            bool empty_room = true;

            //尋找有沒有空的房間可以戰鬥
            for (int i = 0; i < Battle_Room.Count; i++)
            {
                if (Battle_Room[i].Count == 0)
                {
                    room_number = i;
                    break;
                }

                if (i == Battle_Room.Count - 1)
                {
                    empty_room = false;
                }
            }

            if (empty_room == true)
            {
                //把玩家加入戰鬥房間
                Player p1 = wait_for_battle[0];
                wait_for_battle.RemoveAt(0);

                p1.RpcSetState("開始戰鬥");
                p1.RpcSetRoom(room_number);

                Battle_Room[room_number].Add(p1);

                Player p2 = wait_for_battle[0];
                wait_for_battle.RemoveAt(0);

                p2.RpcSetState("開始戰鬥");
                p2.RpcSetRoom(room_number);

                Battle_Room[room_number].Add(p2);

                //設定對手ID和戰鬥位置
                p1.RpcSetRival_ID_and_position(p2.ID, "left");
                p2.RpcSetRival_ID_and_position(p1.ID, "right");
            }
        }

        //運行戰鬥流程
        for (int i = 0; i < Battle_Room.Count; i++)
        {
            if (Battle_Room[i].Count != 0)
            {
                //下載資料到各個battle_UI裡
                if (Battle_Room[i][0].state == "update" && Battle_Room[i][1].state == "update")
                {
                    int room = Battle_Room[i][0].room_number;

                    for (int j = 0; j < chess_attack_pos[room].Count; j++)
                    {
                        Battle_Room[i][0].GetComponent<battle_UI>().RpcDownload(chess_attack_pos[room][j].ToArray());
                    }

                    for (int j = 0; j < chess_attack_pos[room].Count; j++)
                    {
                        Battle_Room[i][1].GetComponent<battle_UI>().RpcDownload(chess_attack_pos[room][j].ToArray());
                    }

                    Battle_Room[i][0].GetComponent<battle_UI>().RpcCall_download_pos();
                    Battle_Room[i][1].GetComponent<battle_UI>().RpcCall_download_pos();

                    SetState(Battle_Room[i][0], "fight");
                    SetState(Battle_Room[i][1], "fight");
                }

                if (Battle_Room[i][0].state == "fight" && Battle_Room[i][1].state == "fight")
                {

                }

                //當兩邊都結束時 才同時開始放旗子階段
                if (Battle_Room[i][0].state == "round_end" && Battle_Room[i][1].state == "round_end")
                {
                    Battle_Room[i][0].player.GetComponent<battle_UI>().Rpcclear_chess();
                    Battle_Room[i][1].player.GetComponent<battle_UI>().Rpcclear_chess();
                    
                    for (int k = 0; k < chess_attack_pos.Count; k++)
                    {
                        for (int j = 0; j < chess_attack_pos[k].Count; j++)
                        {
                            chess_attack_pos[k][j].Clear();
                        }
                        chess_attack_pos[k].Clear();
                    }

                    Battle_Room[i][0].RpcPrepare();
                    Battle_Room[i][1].RpcPrepare();

                    SetState(Battle_Room[i][0], "prepare");
                    SetState(Battle_Room[i][1], "prepare");
                }

                //整場對戰結束了
                if (Battle_Room[i][0].state == "battle_end" && Battle_Room[i][1].state == "battle_end")
                {
                    Battle_Room[i][0].player.GetComponent<battle_UI>().Rpcclear_chess();
                    Battle_Room[i][1].player.GetComponent<battle_UI>().Rpcclear_chess();

                    for (int k = 0; k < chess_attack_pos.Count; k++)
                    {
                        for (int j = 0; j < chess_attack_pos[k].Count; j++)
                        {
                            chess_attack_pos[k][j].Clear();
                        }
                        chess_attack_pos[k].Clear();
                    }

                    //計算分數
                    Battle_Room[i][0].GetComponent<battle_UI>().RpcCalculate_territory();
                    Battle_Room[i][1].GetComponent<battle_UI>().RpcCalculate_territory();

                    //移除房間
                    Battle_Room[i].Clear();
                    continue;
                }

                Battle_Room[i][0].RpcBattle_Schedule();
                Battle_Room[i][1].RpcBattle_Schedule();
            }
        }
    }

    /* 把目前所有Player的ID和IP位址分別存成list */
    void GetInfo()
    {
        foreach (var player in allPlayer)
        {
            if (player.ID != 0 && !allID.Contains((player.ID + 6000).ToString()))
            {
                allID.Add((player.ID + 6000).ToString());
                allIP.Add(player.IP);
                Debug.Log(player.IP);
            }
        }
    }

    //新的Client登入
    public void Login(Player player)
    {
        /* 因為RpcSetID傳值不能傳List<>，
         * 所以先把他們變成字串 */
        GetInfo();
        string IDs = string.Join(",", allID);
        string IPs = string.Join(",", allIP);

        //新增 player 並且設定id
        allPlayer.Add(player);
        player.RpcSetID(allPlayer.Count, IDs, IPs);
    }
    
    //設定種族 一定要用ClientRpc去分配 才可以設定Client Localhost的變數
    public void Distribue_Type(Player player, string type)
    {
        player.RpcSetType(type);
    }

    public void AddBattlePlayer(Player player)
    {
        if(!wait_for_battle.Contains(player))
            wait_for_battle.Add(player);
    }

    public void SetState(Player player, string s)
    {
        player.RpcSetState(s);
    }

    public void update_archer_pos(string[] pos, int room, Player player)
    {
        chess_attack_pos[room].Add(new List<string>());

        for (int i = 0; i < pos.Length; i++)
        {
            chess_attack_pos[room][chess_attack_pos[room].Count - 1].Add(pos[i]);
        }
    }

    /*public void send_all_pos(battle_UI battle_UI_cs)
    {
        battle_UI_cs.RpcCall_download_pos();
    }*/
}
