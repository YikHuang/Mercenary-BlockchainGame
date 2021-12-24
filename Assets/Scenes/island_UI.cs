using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Threading;

public class island_UI : NetworkBehaviour
{
    public GM gm;

    public GameObject player;
    public GameObject bulletin;
    public GameObject dragon, human, god;
    public GameObject dragon_number, human_number, god_number;
    public GameObject dragon_img, human_img, god_img;

    public Text type_text, SysMsg;
    public Button tomb, grassland, mountain, lake;
    public Button conquer_tomb, conquer_grassland, conquer_mountain, conquer_lake;
    public Button battle, leave;

    public bool restart = false;
    public int[] bulletin_number = new int[3];

    void Start()
    {
        //Server端處理 初始化記分板分數
        if (isServer)
        {
            gm = GameObject.Find("GM").GetComponent<GM>();
        }

        //LocalPlayer的按鍵 圖像 文字設定
        if (isLocalPlayer)
        {
            Debug.Log("回來了");

            type_text = GameObject.Find("type").GetComponent<Text>();
            type_text.text = "雇主 : " + player.GetComponent<Player>().type;
            leave = GameObject.Find("Leave").GetComponent<Button>();
            leave.onClick.AddListener(() => LeaveGame());

            //display icon
            if (player.GetComponent<Player>().type == "龍族")
                GameObject.Find("icon_dragon").GetComponent<SpriteRenderer>().enabled = true;
            else if (player.GetComponent<Player>().type == "人族")
                GameObject.Find("icon_human").GetComponent<SpriteRenderer>().enabled = true;
            else if (player.GetComponent<Player>().type == "神族")
                GameObject.Find("icon_god").GetComponent<SpriteRenderer>().enabled = true;

            bulletin = GameObject.Find("wooden");
            bulletin.SetActive(false);

            //記分板的種族 消失
            dragon = GameObject.Find("dragon");
            human = GameObject.Find("human");
            god = GameObject.Find("god");

            dragon.SetActive(false);
            human.SetActive(false);
            god.SetActive(false);

            //記分板的種族數量 消失
            dragon_number = GameObject.Find("dragon_number");
            human_number = GameObject.Find("human_number");
            god_number = GameObject.Find("god_number");

            dragon_number.SetActive(false);
            human_number.SetActive(false);
            god_number.SetActive(false);

            //記分板的種族數量 消失
            dragon_img = GameObject.Find("drag_Image");
            human_img = GameObject.Find("human_Image");
            god_img = GameObject.Find("god_Image");

            dragon_img.SetActive(false);
            human_img.SetActive(false);
            god_img.SetActive(false);

            //顯示 各個地區 的分數按鈕
            tomb = GameObject.Find("tomb").GetComponent<Button>();
            tomb.onClick.AddListener(() => Show_point("tomb"));
            grassland = GameObject.Find("grassland").GetComponent<Button>();
            grassland.onClick.AddListener(() => Show_point("grassland"));
            mountain = GameObject.Find("mountain").GetComponent<Button>();
            mountain.onClick.AddListener(() => Show_point("mountain"));
            lake = GameObject.Find("lake").GetComponent<Button>();
            lake.onClick.AddListener(() => Show_point("lake"));

            //佔領 各個地區 按鈕
            conquer_tomb = GameObject.Find("conquer_tomb").GetComponent<Button>();
            conquer_tomb.onClick.AddListener(() => Conquer_place("tomb"));
            conquer_grassland = GameObject.Find("conquer_grassland").GetComponent<Button>();
            conquer_grassland.onClick.AddListener(() => Conquer_place("grassland"));
            conquer_mountain = GameObject.Find("conquer_mountain").GetComponent<Button>();
            conquer_mountain.onClick.AddListener(() => Conquer_place("mountain"));
            conquer_lake = GameObject.Find("conquer_lake").GetComponent<Button>();
            conquer_lake.onClick.AddListener(() => Conquer_place("lake"));

            //有在對戰中獲勝的玩家 才能點擊佔領按鈕
            if (player.GetComponent<Player>().win == false)
            {
                conquer_tomb.gameObject.SetActive(false);
                conquer_grassland.gameObject.SetActive(false);
                conquer_mountain.gameObject.SetActive(false);
                conquer_lake.gameObject.SetActive(false);
            }
            if (player.GetComponent<Player>().win == true)
            {
                conquer_tomb.gameObject.SetActive(true);
                conquer_grassland.gameObject.SetActive(true);
                conquer_mountain.gameObject.SetActive(true);
                conquer_lake.gameObject.SetActive(true);
            }

            //搜尋對戰按鈕
            battle = GameObject.Find("Battle").GetComponent<Button>();
            battle.onClick.AddListener(() => CmdStart_battle());

            //尋找搜尋狀態文字
            SysMsg = GameObject.Find("SysMsg").GetComponent<Text>();
        }
    }

    void Update()
    {
        //從battle_UI切回來的時候用的
        if (restart == true)
        {
            restart = false;
            Start();
        }
        
        //點擊任一區域 讓記分板消失
        if (isLocalPlayer)
        {
            if (SceneManager.GetActiveScene().name == "island")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    bulletin.SetActive(false);

                    dragon.SetActive(false);
                    human.SetActive(false);
                    god.SetActive(false);

                    dragon_number.SetActive(false);
                    human_number.SetActive(false);
                    god_number.SetActive(false);

                    dragon_img.SetActive(false);
                    human_img.SetActive(false);
                    god_img.SetActive(false);
                }
            }
        }
    }
    
    //顯示記分板
    public void Show_point(string place)
    {
        if (isLocalPlayer)
        {
            bulletin.SetActive(true);
            dragon.SetActive(true);
            human.SetActive(true);
            god.SetActive(true);
            
            //統計並取得區塊鏈的內容 然後顯示分數
            bulletin_number = player.GetComponent<Player>().Show_Bulletin(place);
            dragon_number.GetComponent<Text>().text = bulletin_number[0].ToString();
            human_number.GetComponent<Text>().text = bulletin_number[1].ToString();
            god_number.GetComponent<Text>().text = bulletin_number[2].ToString();

            dragon_number.gameObject.SetActive(true);
            human_number.gameObject.SetActive(true);
            god_number.gameObject.SetActive(true);

            dragon_img.SetActive(true);
            human_img.SetActive(true);
            god_img.SetActive(true);
        }
    }

    //佔領按鍵 將運算交由Server端進行處理
    //點一次 增加一個
    public void Conquer_place(string place)
    {
        if (isLocalPlayer)
        {
            player.GetComponent<Player>().Conquer(place);

            //點了一次之後 就不能再點了
            conquer_tomb.gameObject.SetActive(false);
            conquer_grassland.gameObject.SetActive(false);
            conquer_mountain.gameObject.SetActive(false);
            conquer_lake.gameObject.SetActive(false);
        }
    }

    //將搜尋對戰的玩家加入GM裡的等待列表
    [Command]
    public void CmdStart_battle()
    {
        if (isServer)
            player.GetComponent<Player>().CmdStart_Battle();
    }

    public void SetSysMsg(string msg)
    {
        if (isLocalPlayer)
            SysMsg.text = msg;
    }

    public void Change_scene()
    {
        if (isLocalPlayer)
        {
            SceneManager.LoadScene("battle");
            player.GetComponent<battle_UI>().restart = true;
        }
    }

    void LeaveGame()
    {
        if (NetworkServer.active || NetworkClient.active)
            NetworkManager.singleton.StopHost();
    }
}