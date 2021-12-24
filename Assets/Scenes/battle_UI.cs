using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class battle_UI : NetworkBehaviour
{
    public GM gm;

    public AudioSource warrior_audio;
    public AudioSource knight_audio;
    public AudioSource magician_audio;
    public AudioSource archer_audio;
    public AudioSource monk_audio;
    public GameObject player;
    public GameObject warrior;
    public GameObject Instantiate_Position;
    public Text p1_id;
    public Text p2_id;
    public Text num_round; 
    public Text Text;
    public Text timer_text;
    public Text round_text;
    public Color color = Color.white;
    public int timer = 30;
    public bool start_timer = true;
    public bool prepare = true;
    public bool battle_end = false;
    public bool restart = false;
    public int round = 0;
    public int current_put_num = 0;
    public int warrior_last = 4;
    public int p1_warrior_put_last = 4;
    public int p2_warrior_put_last = 4;
    public int p1_warrior_clear = 4;
    public int p2_warrior_clear = 4;
    public int p1_warrior_move = 4;
    public int p2_warrior_move = 4;
    public int knight_last = 2;
    public int p1_knight_put_last = 2;
    public int p2_knight_put_last = 2;
    public int p1_knight_clear = 2;
    public int p2_knight_clear = 2;
    public int p1_knight_move = 2;
    public int p2_knight_move = 2;
    public int archer_last = 3;
    public int p1_archer_put_last = 3;
    public int p2_archer_put_last = 3;
    public int p1_archer_clear = 3;
    public int p2_archer_clear = 3;
    public int p1_archer_move = 3;
    public int p2_archer_move = 3;
    public int magician_last = 2;
    public int p1_magician_put_last = 2;
    public int p2_magician_put_last = 2;
    public int p1_magician_clear = 2;
    public int p2_magician_clear = 2;
    public int p1_magician_move = 2;
    public int p2_magician_move = 2;
    public int monk_last = 1;
    public int p1_monk_put_last = 1;
    public int p2_monk_put_last = 1;
    public int p1_monk_clear = 1;
    public int p2_monk_clear = 1;
    public int p1_monk_move = 1;
    public int p2_monk_move = 1;
    public int select = 0;
    public int warrior_attacktime = 2;
    public int magician_attacktime = 2;
    public int archer_attacktime = 4;
    public int monk_attacktime = 2;
    int[] array1 = new int[] { 1, 3, 5, 7, 9 };
    bool start_download_chess_func = false;

    public int attack_time_count = 0;
    public int[] round_chess = new int[] { 2, 2, 2, 3, 3 };
    public int warrior_one = 0;
    public int warrior_two = 0;
    public string put_done = "";
    public string warrior_pos = "";
    public Button button_warrior;
    public Button button_knight;
    public Button button_magician;
    public Button button_archer;
    public Button button_monk;
    public Button A1;
    public Text red_territory;
    public Text blue_territory;
    public GameObject battle_end_notice;
    public GameObject p1_warrior1;
    public GameObject win_picture;
    public GameObject lose_picture;

    public List<List<string>> my_chess_attack_pos = new List<List<string>>();
    public SyncListString get_chess_attack_pos = new SyncListString();
    public List<List<string>> all_chess_attack_pos = new List<List<string>>();
    public List<List<string>> color_pos = new List<List<string>>();

    public GameObject p1_attack_monk;
    public GameObject p1_attack_warrior;
    public GameObject p1_attack_knight;
    public GameObject p1_attack_archer;
    public GameObject p1_attack_magician;

    public GameObject die;
    public List<GameObject> copy_die = new List<GameObject>();

    public List<GameObject> copy_monk_attack = new List<GameObject>();
    public List<GameObject> copy_warrior_attack = new List<GameObject>();
    public List<GameObject> copy_knight_attack = new List<GameObject>();
    public List<GameObject> copy_magician_attack = new List<GameObject>();
    public List<GameObject> copy_archer_attack = new List<GameObject>();

    private Ray mouseRay1;    // 宣告一條射線
    private RaycastHit rayHit;  //RaycastHit是用來儲存被射線所打到的位置
    public float posX, posY;  //用來接收滑鼠點擊座標的X、Y值

    public List<List<Button>> checker = new List<List<Button>>();
    public List<GameObject> p1_warrior = new List<GameObject>();
    public List<GameObject> p2_warrior = new List<GameObject>();
    public List<GameObject> p1_knight = new List<GameObject>();
    public List<GameObject> p2_knight = new List<GameObject>();
    public List<GameObject> p1_magician = new List<GameObject>();
    public List<GameObject> p2_magician = new List<GameObject>();
    public List<GameObject> p1_archer = new List<GameObject>();
    public List<GameObject> p2_archer = new List<GameObject>();
    public List<GameObject> p1_monk = new List<GameObject>();
    public List<GameObject> p2_monk = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        p1_id = GameObject.Find("p1_id").GetComponent<Text>();
        if (isLocalPlayer)
        {


            restart = false;


            //勝敗圖片
            win_picture = GameObject.Find("win");
            lose_picture = GameObject.Find("lose");

            win_picture.SetActive(false);
            lose_picture.SetActive(false);

            //結束對戰提示訊息
            battle_end_notice = GameObject.Find("battle_end_notice");
            battle_end_notice.SetActive(false);


            //領土數字
            red_territory = GameObject.Find("red_territory").GetComponent<Text>();
            blue_territory = GameObject.Find("blue_territory").GetComponent<Text>();

            num_round = GameObject.Find("num").GetComponent<Text>();

            red_territory.text = "24";
            blue_territory.text = "24";

            die = GameObject.Find("die");

            warrior_audio = GameObject.Find("warrior_audio").GetComponent<AudioSource>();
            knight_audio = GameObject.Find("knight_audio").GetComponent<AudioSource>();
            magician_audio = GameObject.Find("magician_audio").GetComponent<AudioSource>();
            archer_audio = GameObject.Find("archer_audio").GetComponent<AudioSource>();
            monk_audio = GameObject.Find("monk_audio").GetComponent<AudioSource>();


            p1_attack_monk = GameObject.Find("p1_attack_monk");
            p1_attack_warrior = GameObject.Find("p1_attack_warrior");
            p1_attack_knight = GameObject.Find("p1_attack_knight");
            p1_attack_magician = GameObject.Find("p1_attack_magician");
            p1_attack_archer = GameObject.Find("p1_attack_archer");

            //點擊卡片
            button_warrior = GameObject.Find("warrior").GetComponent<Button>();
            button_warrior.onClick.AddListener(() => select_warrior());
            button_knight = GameObject.Find("knight").GetComponent<Button>();
            button_knight.onClick.AddListener(() => select_knight());
            button_magician = GameObject.Find("magician").GetComponent<Button>();
            button_magician.onClick.AddListener(() => select_magician());
            button_archer = GameObject.Find("archer").GetComponent<Button>();
            button_archer.onClick.AddListener(() => select_archer());
            button_monk = GameObject.Find("monk").GetComponent<Button>();
            button_monk.onClick.AddListener(() => select_monk());
            color.a = 0;
            string board = "";
            string[] word = new string[] { "A", "B", "C", "D", "E", "F" };
            for (int i = 0; i < 6; i++)
            {
                color_pos.Add(new List<string>());
                checker.Add(new List<Button>());
                for (int j = 0; j < 8; j++)
                {

                    //新增角色顏色區塊
                    if (j < 4)
                    {
                        color_pos[i].Add("red");
                    }

                    else
                    {
                        color_pos[i].Add("blue");
                    }

                    board = word[i] + (j + 1).ToString();
                    checker[i].Add(GameObject.Find(board).GetComponent<Button>());
                    checker[i][j].onClick.AddListener(() => Put());
                    checker[i][j].onClick.AddListener(() => attack_select());
                }
            }
            p1_warrior.Add(GameObject.Find("p1_warrior0"));
            p1_warrior.Add(GameObject.Find("p1_warrior1"));
            p1_warrior.Add(GameObject.Find("p1_warrior2"));
            p1_warrior.Add(GameObject.Find("p1_warrior3"));
            p2_warrior.Add(GameObject.Find("p2_warrior0"));
            p2_warrior.Add(GameObject.Find("p2_warrior1"));
            p2_warrior.Add(GameObject.Find("p2_warrior2"));
            p2_warrior.Add(GameObject.Find("p2_warrior3"));

            p1_knight.Add(GameObject.Find("p1_knight0"));
            p1_knight.Add(GameObject.Find("p1_knight1"));
            p2_knight.Add(GameObject.Find("p2_knight0"));
            p2_knight.Add(GameObject.Find("p2_knight1"));

            p1_magician.Add(GameObject.Find("p1_magician0"));
            p1_magician.Add(GameObject.Find("p1_magician1"));
            p2_magician.Add(GameObject.Find("p2_magician0"));
            p2_magician.Add(GameObject.Find("p2_magician1"));

            p1_archer.Add(GameObject.Find("p1_archer0"));
            p1_archer.Add(GameObject.Find("p1_archer1"));
            p1_archer.Add(GameObject.Find("p1_archer2"));
            p2_archer.Add(GameObject.Find("p2_archer0"));
            p2_archer.Add(GameObject.Find("p2_archer1"));
            p2_archer.Add(GameObject.Find("p2_archer2"));

            p1_monk.Add(GameObject.Find("p1_monk0"));
            p2_monk.Add(GameObject.Find("p2_monk0"));


            //依照戰鬥位置顯示 對手id和自己的id 
            p1_id = GameObject.Find("p1_id").GetComponent<Text>();
            p2_id = GameObject.Find("p2_id").GetComponent<Text>();

            //-----------------------P1
            if (player.GetComponent<Player>().battle_position == "left")
            {
                p1_id.text = "Player : " + player.GetComponent<Player>().ID.ToString();
                p2_id.text = "Player : " + player.GetComponent<Player>().rival_ID.ToString();
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        checker[i][j].GetComponent<Image>().color = Color.red;
                        checker[i][j].gameObject.SetActive(true);
                    }
                }
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 4; j < 8; j++)
                    {
                        checker[i][j].GetComponent<Image>().color = Color.blue;
                        checker[i][j].gameObject.SetActive(false);
                    }
                }

            }

            else
            {
                p2_id.text = "Player : " + player.GetComponent<Player>().ID.ToString();
                p1_id.text = "Player : " + player.GetComponent<Player>().rival_ID.ToString();
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        checker[i][j].GetComponent<Image>().color = Color.red;
                        checker[i][j].gameObject.SetActive(false);
                    }
                }
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 4; j < 8; j++)
                    {
                        checker[i][j].GetComponent<Image>().color = Color.blue;
                        checker[i][j].gameObject.SetActive(true);
                    }
                }

            }

            //設定計時器 回合
            timer_text = GameObject.Find("timer").GetComponent<Text>();
            round_text = GameObject.Find("round").GetComponent<Text>();
        }
    }

    void Update()
    {

        if (isLocalPlayer)
        {
            if (restart == true)
            {
                restart = false;

                Start();
            }

            if (round < 5)
            {
                num_round.text = current_put_num.ToString() + "/" + (round_chess[round].ToString());
            }
            /*if (current_put_num == round_chess[round])
            {
                button_warrior.gameObject.SetActive(false);
                button_knight.gameObject.SetActive(false);
                button_magician.gameObject.SetActive(false);
                button_archer.gameObject.SetActive(false);
                button_monk.gameObject.SetActive(false);
            }*/
        }

    }


    //倒數計時
    public void CountDown()
    {
        if (isLocalPlayer)
        {

            round_text.text = "";

            timer--;
            timer_text.text = timer.ToString();


            //0秒時進入下一局
            if (timer == 0)
            {
                round++;
                round_text.text = "Round : " + round.ToString();

                timer = 30;
                timer_text.text = "";

                prepare = false;
                player.GetComponent<Player>().CmdSetState("round_start");

                put_done = "";

                button_warrior.gameObject.SetActive(false);
                button_knight.gameObject.SetActive(false);
                button_magician.gameObject.SetActive(false);
                button_archer.gameObject.SetActive(false);
                button_monk.gameObject.SetActive(false);

                current_put_num = 0;

                //傳資料到Player 再傳到Server
                for (int i = 0; i < my_chess_attack_pos.Count; i++)
                {
                    player.GetComponent<Player>().Cmdupdate_my_pos(my_chess_attack_pos[i].ToArray());
                }

                player.GetComponent<Player>().CmdSetState("update");



            }
            else
            {
                button_warrior.gameObject.SetActive(true);
                button_knight.gameObject.SetActive(true);
                button_magician.gameObject.SetActive(true);
                button_archer.gameObject.SetActive(true);
                button_monk.gameObject.SetActive(true);
            }
        }
    }

    void Put() //放置棋子
    {
        if (isLocalPlayer)
        {
            string chessman_pos = "";
            var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            Vector2 pos = buttonSelf.transform.position;
            //--------------------------------劍士
            if (select == 1 && current_put_num < round_chess[round])
            {

                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        checker[i][j].gameObject.SetActive(true);
                    }
                }
                
                warrior_pos = buttonSelf.ToString();
                warrior_one = warrior_pos[0] - 65;
                warrior_two = int.Parse(warrior_pos[1].ToString()) - 1;
                if (warrior_last != 0)
                {
                    if (player.GetComponent<Player>().battle_position == "left")
                    {
                        p1_warrior[warrior_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                    }
                    else
                    {
                        p2_warrior[warrior_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                    }
                    warrior_last = warrior_last - 1;
                    GameObject.Find("Canvas/warrior/warrior_num").GetComponent<Text>().text = warrior_last.ToString();
                    put_done = "warrior";
                    my_chess_attack_pos.Add(new List<string>());
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(player.GetComponent<Player>().battle_position);
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add("warrior");
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(buttonSelf.ToString());
                    current_put_num++;
                }
            }
            //--------------------------------騎士
            else if (select == 2 && current_put_num < round_chess[round])
            {
                
                if (knight_last != 0)
                {
                    if (player.GetComponent<Player>().battle_position == "left")
                    {
                        p1_knight[knight_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                    }
                    else
                    {
                        p2_knight[knight_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                    }
                    knight_last = knight_last - 1;
                    GameObject.Find("Canvas/knight/knight_num").GetComponent<Text>().text = knight_last.ToString();
                    put_done = "knight";
                    my_chess_attack_pos.Add(new List<string>());

                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(player.GetComponent<Player>().battle_position);
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add("knight");
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(buttonSelf.ToString());
                    current_put_num++;
                }
            }
            //--------------------------------魔法師
            else if (select == 3 && current_put_num < round_chess[round])
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        checker[i][j].gameObject.SetActive(true);
                    }
                }

                
                if (magician_last != 0)
                {
                    if (player.GetComponent<Player>().battle_position == "left")
                    {
                        p1_magician[magician_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                    }
                    else
                    {
                        p2_magician[magician_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                    }
                    magician_last = magician_last - 1;
                    GameObject.Find("Canvas/magician/magician_num").GetComponent<Text>().text = magician_last.ToString();
                    //buttonSelf.GetComponent<Image>().color = Color.red;
                    put_done = "magician";
                    my_chess_attack_pos.Add(new List<string>());
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(player.GetComponent<Player>().battle_position);
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add("magician");
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(buttonSelf.ToString());
                    current_put_num++;
                }
            }
            //--------------------------------弓箭手
            else if (select == 4 && current_put_num < round_chess[round])
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        checker[i][j].gameObject.SetActive(true);
                    }
                }

                
                if (archer_last != 0)
                {
                    if (player.GetComponent<Player>().battle_position == "left")
                    {
                        p1_archer[archer_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                    }
                    else
                    {
                        p2_archer[archer_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                    }
                    archer_last = archer_last - 1;
                    GameObject.Find("Canvas/archer/archer_num").GetComponent<Text>().text = archer_last.ToString();
                    //buttonSelf.GetComponent<Image>().color = Color.red;
                    put_done = "archer";
                    my_chess_attack_pos.Add(new List<string>());
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(player.GetComponent<Player>().battle_position);
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add("archer");
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(buttonSelf.ToString());
                    current_put_num++;
                }
            }
            //--------------------------------僧侶
            else if (select == 5 && current_put_num < round_chess[round])
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        checker[i][j].gameObject.SetActive(true);
                    }
                }

                
                if (monk_last != 0)
                {
                    if (player.GetComponent<Player>().battle_position == "left")
                    {
                        p1_monk[monk_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                    }
                    else
                    {
                        p2_monk[monk_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                    }
                    monk_last = monk_last - 1;
                    GameObject.Find("Canvas/monk/monk_num").GetComponent<Text>().text = monk_last.ToString();
                    //buttonSelf.GetComponent<Image>().color = Color.red;
                    put_done = "monk";
                    my_chess_attack_pos.Add(new List<string>());
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(player.GetComponent<Player>().battle_position);
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add("monk");
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(buttonSelf.ToString());
                    current_put_num++;
                }
            }

            select = 0;
        }
    }

    void attack_select()
    {
        if (isLocalPlayer)
        {
            string now_checker = "";
            if (put_done == "warrior")
            {
                if (warrior_attacktime == 2)
                {
                    warrior_attacktime--;
                }
                else if (warrior_attacktime == 1)
                {
                    var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
                    now_checker = buttonSelf.ToString();
                    int one = now_checker[0] - 65;
                    int two = int.Parse(now_checker[1].ToString()) - 1;
                    if (one == warrior_one + 1 || one == warrior_one + 2 || one == warrior_one + 3)
                    {
                        if (two == warrior_two)
                        {
                            checker[warrior_one + 1][warrior_two].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[warrior_one + 1][warrior_two].gameObject.ToString());
                            checker[warrior_one + 2][warrior_two].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[warrior_one + 2][warrior_two].gameObject.ToString());
                            checker[warrior_one + 3][warrior_two].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[warrior_one + 3][warrior_two].gameObject.ToString());
                            warrior_attacktime--;
                        }
                    }
                    else if (one == warrior_one - 1 || one == warrior_one - 2 || one == warrior_one - 3)
                    {
                        if (two == warrior_two)
                        {
                            checker[warrior_one - 1][warrior_two].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[warrior_one - 1][warrior_two].gameObject.ToString());
                            checker[warrior_one - 2][warrior_two].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[warrior_one - 2][warrior_two].gameObject.ToString());
                            checker[warrior_one - 3][warrior_two].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[warrior_one - 3][warrior_two].gameObject.ToString());
                            warrior_attacktime--;
                        }
                    }
                    else if (two == warrior_two + 1 || two == warrior_two + 2 || two == warrior_two + 3)
                    {
                        if (one == warrior_one)
                        {
                            checker[warrior_one][warrior_two + 1].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[warrior_one][warrior_two + 1].gameObject.ToString());
                            checker[warrior_one][warrior_two + 2].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[warrior_one][warrior_two + 2].gameObject.ToString());
                            checker[warrior_one][warrior_two + 3].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[warrior_one][warrior_two + 3].gameObject.ToString());
                            warrior_attacktime--;
                        }
                    }
                    else if (two == warrior_two - 1 || two == warrior_two - 2 || two == warrior_two - 3)
                    {
                        if (one == warrior_one)
                        {
                            checker[warrior_one][warrior_two - 1].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[warrior_one][warrior_two - 1].gameObject.ToString());
                            checker[warrior_one][warrior_two - 2].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[warrior_one][warrior_two - 2].gameObject.ToString());
                            checker[warrior_one][warrior_two - 3].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[warrior_one][warrior_two - 3].gameObject.ToString());
                            warrior_attacktime--;
                        }
                    }
                }
                else
                {
                    put_done = "";
                }
            }
            else if (put_done == "knight")
            {
                var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
                now_checker = buttonSelf.ToString();
                int one = now_checker[0] - 65;
                int two = int.Parse(now_checker[1].ToString()) - 1;
                if (one == 0)
                {
                    if (two == 0)
                    {
                        checker[one][two + 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two + 1].gameObject.ToString());
                        checker[one + 1][two + 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two + 1].gameObject.ToString());
                    }
                    else if (two == 7)
                    {
                        checker[one][two - 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two - 1].gameObject.ToString());
                        checker[one + 1][two - 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two - 1].gameObject.ToString());
                    }
                    else//2
                    {
                        checker[one][two + 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two + 1].gameObject.ToString());
                        checker[one + 1][two - 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two - 1].gameObject.ToString());
                        checker[one][two - 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two - 1].gameObject.ToString());
                        checker[one + 1][two + 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two + 1].gameObject.ToString());
                    }
                    checker[one + 1][two].GetComponent<Image>().color = Color.yellow;
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two].gameObject.ToString());
                }
                else if (one == 5)
                {
                    if (two == 0)
                    {
                        checker[one][two + 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two + 1].gameObject.ToString());
                        checker[one - 1][two + 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two + 1].gameObject.ToString());
                    }
                    else if (two == 7)
                    {
                        checker[one][two - 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two - 1].gameObject.ToString());
                        checker[one - 1][two - 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two - 1].gameObject.ToString());
                    }
                    else
                    {
                        checker[one - 1][two - 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two - 1].gameObject.ToString());
                        checker[one - 1][two + 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two + 1].gameObject.ToString());
                        checker[one][two + 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two + 1].gameObject.ToString());
                        checker[one][two - 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two - 1].gameObject.ToString());
                    }
                    checker[one - 1][two].GetComponent<Image>().color = Color.yellow;
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two].gameObject.ToString());
                }
                else if (two == 0)
                {
                    if (one != 0 && one != 5)
                    {
                        checker[one - 1][two].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two].gameObject.ToString());
                        checker[one - 1][two + 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two + 1].gameObject.ToString());
                        checker[one + 1][two + 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two + 1].gameObject.ToString());
                        checker[one][two + 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two + 1].gameObject.ToString());
                        checker[one + 1][two].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two].gameObject.ToString());
                    }
                }
                else if (two == 7)
                {
                    if (one != 0 && one != 5)
                    {
                        checker[one - 1][two].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two].gameObject.ToString());
                        checker[one - 1][two - 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two - 1].gameObject.ToString());
                        checker[one + 1][two - 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two - 1].gameObject.ToString());
                        checker[one][two - 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two - 1].gameObject.ToString());
                        checker[one + 1][two].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two].gameObject.ToString());
                    }
                }
                else
                {
                    checker[one - 1][two - 1].GetComponent<Image>().color = Color.yellow;
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two - 1].gameObject.ToString());
                    checker[one - 1][two + 1].GetComponent<Image>().color = Color.yellow;
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two + 1].gameObject.ToString());
                    checker[one][two + 1].GetComponent<Image>().color = Color.yellow;
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two + 1].gameObject.ToString());
                    checker[one][two - 1].GetComponent<Image>().color = Color.yellow;
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two - 1].gameObject.ToString());
                    checker[one + 1][two - 1].GetComponent<Image>().color = Color.yellow;
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two - 1].gameObject.ToString());
                    checker[one + 1][two + 1].GetComponent<Image>().color = Color.yellow;
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two + 1].gameObject.ToString());
                    checker[one - 1][two].GetComponent<Image>().color = Color.yellow;
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two].gameObject.ToString());
                    checker[one + 1][two].GetComponent<Image>().color = Color.yellow;
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two].gameObject.ToString());
                }
                put_done = "";
            }
            else if (put_done == "magician")
            {
                if (magician_attacktime == 2)
                {
                    magician_attacktime--;
                }
                else if (magician_attacktime == 1)
                {
                    var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
                    now_checker = buttonSelf.ToString();
                    int one = now_checker[0] - 65;
                    int two = int.Parse(now_checker[1].ToString()) - 1;
                    if (one == 0)
                    {
                        if (two == 0)
                        {
                            checker[one][two + 1].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two + 1].gameObject.ToString());
                        }
                        else if (two == 7)
                        {
                            checker[one][two - 1].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two - 1].gameObject.ToString());
                        }
                        else
                        {
                            checker[one][two + 1].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two + 1].gameObject.ToString());
                            checker[one][two - 1].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two - 1].gameObject.ToString());
                        }
                        checker[one + 1][two].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two].gameObject.ToString());
                        checker[one][two].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two].gameObject.ToString());
                    }
                    else if (one == 5)
                    {
                        if (two == 0) // 7
                        {
                            checker[one][two + 1].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two + 1].gameObject.ToString());
                        }
                        else if (two == 7) //9
                        {
                            checker[one][two - 1].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two - 1].gameObject.ToString());
                        }
                        else
                        {
                            checker[one][two + 1].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two + 1].gameObject.ToString());
                            checker[one][two - 1].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two - 1].gameObject.ToString());
                        }
                        checker[one - 1][two].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two].gameObject.ToString());
                        checker[one][two].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two].gameObject.ToString());
                    }
                    else if (two == 0)
                    {
                        if (one != 5 && one != 0)
                        {
                            checker[one + 1][two].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two].gameObject.ToString());
                            checker[one - 1][two].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two].gameObject.ToString());
                            checker[one][two + 1].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two + 1].gameObject.ToString());
                        }
                        checker[one][two].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two].gameObject.ToString());
                    }
                    else if (two == 7)
                    {
                        if (one != 5 && one != 0)
                        {
                            checker[one + 1][two].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two].gameObject.ToString());
                            checker[one - 1][two].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two].gameObject.ToString());
                            checker[one][two - 1].GetComponent<Image>().color = Color.yellow;
                            my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two - 1].gameObject.ToString());
                        }
                        checker[one][two].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two].gameObject.ToString());
                    }
                    else
                    {
                        checker[one][two + 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two + 1].gameObject.ToString());
                        checker[one][two - 1].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two - 1].gameObject.ToString());
                        checker[one - 1][two].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one - 1][two].gameObject.ToString());
                        checker[one + 1][two].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one + 1][two].gameObject.ToString());
                        checker[one][two].GetComponent<Image>().color = Color.yellow;
                        my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(checker[one][two].gameObject.ToString());
                    }
                    magician_attacktime--;
                }
                else
                {
                    put_done = "";
                }
            }
            else if (put_done == "archer")
            {
                if (archer_attacktime == 4)
                {
                    archer_attacktime--;
                }
                else if (archer_attacktime < 4 && archer_attacktime > 0)
                {
                    var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(buttonSelf.ToString());
                    buttonSelf.GetComponent<Image>().color = Color.yellow;
                    archer_attacktime--;
                }
                else
                {
                    put_done = "";
                }
            }
            else if (put_done == "monk")
            {
                if (monk_attacktime == 2)
                {
                    monk_attacktime--;
                }
                else if (monk_attacktime == 1)
                {
                    var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
                    my_chess_attack_pos[my_chess_attack_pos.Count - 1].Add(buttonSelf.ToString());
                    buttonSelf.GetComponent<Image>().color = Color.yellow;
                    monk_attacktime--;
                }
                else
                {
                    put_done = "";
                }
            }
        }
    }

    //---------------------------------------------select = 1 ->劍士 2->騎士 3->魔法師 4->弓箭手 5->僧侶
    void select_warrior()
    {
        if (isLocalPlayer)
        {
            if (player.GetComponent<Player>().battle_position == "left")
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (checker[i][j].GetComponent<Image>().color == Color.red)
                        {
                            checker[i][j].gameObject.SetActive(true);
                        }

                        else if (checker[i][j].GetComponent<Image>().color == Color.blue)
                        {
                            checker[i][j].gameObject.SetActive(false);
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
                        if (checker[i][j].GetComponent<Image>().color == Color.blue)
                        {
                            checker[i][j].gameObject.SetActive(true);
                        }

                        else if (checker[i][j].GetComponent<Image>().color == Color.red)
                        {
                            checker[i][j].gameObject.SetActive(false);
                        }

                    }
                }
            }
            if (warrior_last != 0)
            {
                select = 1;
            }
            warrior_attacktime = 2;
        }
    }
    void select_knight()
    {
        if (isLocalPlayer)
        {
            if (player.GetComponent<Player>().battle_position == "left")
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (checker[i][j].GetComponent<Image>().color == Color.red)
                        {
                            checker[i][j].gameObject.SetActive(true);
                        }

                        else if (checker[i][j].GetComponent<Image>().color == Color.blue)
                        {
                            checker[i][j].gameObject.SetActive(false);
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
                        if (checker[i][j].GetComponent<Image>().color == Color.blue)
                        {
                            checker[i][j].gameObject.SetActive(true);
                        }

                        else if (checker[i][j].GetComponent<Image>().color == Color.red)
                        {
                            checker[i][j].gameObject.SetActive(false);
                        }

                    }
                }
            }
            if (knight_last != 0)
            {
                select = 2;
            }
        }
    }
    void select_magician()
    {
        if (isLocalPlayer)
        {
            if (player.GetComponent<Player>().battle_position == "left")
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (checker[i][j].GetComponent<Image>().color == Color.red)
                        {
                            checker[i][j].gameObject.SetActive(true);
                        }

                        else if (checker[i][j].GetComponent<Image>().color == Color.blue)
                        {
                            checker[i][j].gameObject.SetActive(false);
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
                        if (checker[i][j].GetComponent<Image>().color == Color.blue)
                        {
                            checker[i][j].gameObject.SetActive(true);
                        }

                        else if (checker[i][j].GetComponent<Image>().color == Color.red)
                        {
                            checker[i][j].gameObject.SetActive(false);
                        }

                    }
                }
            }
            if (magician_last != 0)
            {
                select = 3;
            }
            magician_attacktime = 2;
        }
    }
    void select_archer()
    {
        if (isLocalPlayer)
        {
            if (player.GetComponent<Player>().battle_position == "left")
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (checker[i][j].GetComponent<Image>().color == Color.red)
                        {
                            checker[i][j].gameObject.SetActive(true);
                        }

                        else if (checker[i][j].GetComponent<Image>().color == Color.blue)
                        {
                            checker[i][j].gameObject.SetActive(false);
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
                        if (checker[i][j].GetComponent<Image>().color == Color.blue)
                        {
                            checker[i][j].gameObject.SetActive(true);
                        }

                        else if (checker[i][j].GetComponent<Image>().color == Color.red)
                        {
                            checker[i][j].gameObject.SetActive(false);
                        }

                    }
                }
            }
            if (archer_last != 0)
            {
                select = 4;
            }
            archer_attacktime = 4;
        }
    }
    void select_monk()
    {
        if (isLocalPlayer)
        {
            if (player.GetComponent<Player>().battle_position == "left")
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (checker[i][j].GetComponent<Image>().color == Color.red)
                        {
                            checker[i][j].gameObject.SetActive(true);
                        }

                        else if (checker[i][j].GetComponent<Image>().color == Color.blue)
                        {
                            checker[i][j].gameObject.SetActive(false);
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
                        if (checker[i][j].GetComponent<Image>().color == Color.blue)
                        {
                            checker[i][j].gameObject.SetActive(true);
                        }

                        else if (checker[i][j].GetComponent<Image>().color == Color.red)
                        {
                            checker[i][j].gameObject.SetActive(false);
                        }

                    }
                }
            }
            if (monk_last != 0)
            {
                select = 5;
            }
            monk_attacktime = 2;
        }
    }


    [ClientRpc]
    public void Rpcclear_chess()
    {
        if (isLocalPlayer)
        {
            for (int i = 0; i < all_chess_attack_pos.Count; i++)
            {
                if (p1_warrior_clear != p1_warrior_put_last)
                {
                    p1_warrior[p1_warrior_clear - 1].transform.position = new Vector3(50, 50, 0);
                    p1_warrior_clear--;
                }
                if (p1_knight_clear != p1_knight_put_last)
                {
                    p1_knight[p1_knight_clear - 1].transform.position = new Vector3(50, 50, 0);
                    p1_knight_clear--;
                }
                if (p1_magician_clear != p1_magician_put_last)
                {
                    p1_magician[p1_magician_clear - 1].transform.position = new Vector3(50, 50, 0);
                    p1_magician_clear--;
                }
                if (p1_archer_clear != p1_archer_put_last)
                {
                    p1_archer[p1_archer_clear - 1].transform.position = new Vector3(50, 50, 0);
                    p1_archer_clear--;
                }
                if (p1_monk_clear != p1_monk_put_last)
                {
                    p1_monk[p1_monk_clear - 1].transform.position = new Vector3(50, 50, 0);
                    p1_monk_clear--;
                }
                if (p2_warrior_clear != p2_warrior_put_last)
                {
                    p2_warrior[p2_warrior_clear - 1].transform.position = new Vector3(50, 50, 0);
                    p2_warrior_clear--;
                }
                if (p2_knight_clear != p2_knight_put_last)
                {
                    p2_knight[p2_knight_clear - 1].transform.position = new Vector3(50, 50, 0);
                    p2_knight_clear--;
                }
                if (p2_magician_clear != p2_magician_put_last)
                {
                    p2_magician[p2_magician_clear - 1].transform.position = new Vector3(50, 50, 0);
                    p2_magician_clear--;
                }
                if (p2_archer_clear != p2_archer_put_last)
                {
                    p2_archer[p2_archer_clear - 1].transform.position = new Vector3(50, 50, 0);
                    p2_archer_clear--;
                }
                if (p2_monk_clear != p2_monk_put_last)
                {
                    p2_monk[p2_monk_clear - 1].transform.position = new Vector3(50, 50, 0);
                    p2_monk_clear--;
                }
            }
            for (int i = 0; i < my_chess_attack_pos.Count; i++)
            {
                my_chess_attack_pos[i].Clear();
            }
            my_chess_attack_pos.Clear();
            for (int i = 0; i < all_chess_attack_pos.Count; i++)
            {
                all_chess_attack_pos[i].Clear();
            }
            all_chess_attack_pos.Clear();

            for (int i = 0; i < copy_die.Count; i++)
            {
                Destroy(copy_die[i]);
            }
            for (int i = 0; i < copy_warrior_attack.Count; i++)
            {
                Destroy(copy_warrior_attack[i]);
            }
            for (int i = 0; i < copy_monk_attack.Count; i++)
            {
                Destroy(copy_monk_attack[i]);
            }
            for (int i = 0; i < copy_archer_attack.Count; i++)
            {
                Destroy(copy_archer_attack[i]);
            }
            for (int i = 0; i < copy_knight_attack.Count; i++)
            {
                Destroy(copy_knight_attack[i]);
            }
            for (int i = 0; i < copy_magician_attack.Count; i++)
            {
                Destroy(copy_magician_attack[i]);
            }

            Debug.Log(my_chess_attack_pos.Count);
            Debug.Log(all_chess_attack_pos.Count);
        }
    }

    /*[Command]
    public void Cmddownload_my_pos()
    {
        gm = GameObject.Find("GM").GetComponent<GM>();
        gm.send_all_pos(this);
    }*/


    [ClientRpc]
    public void RpcDownload(string[] chess_attack_pos)
    {

        if (isLocalPlayer)
        {
            //---------------------------------這邊是從GM抓棋子的資料下來

            all_chess_attack_pos.Add(new List<string>());

            int room = player.GetComponent<Player>().room_number;

            for (int i = 0; i < chess_attack_pos.Length; i++)
            {
                all_chess_attack_pos[all_chess_attack_pos.Count - 1].Add(chess_attack_pos[i]);

            }


        }
    }


    [ClientRpc]
    public void RpcCall_download_pos()
    {
        if (isLocalPlayer)
        {



            string now_checker = "";
            //--------------------------------判斷棋子並顯示出來
            for (int i = 0; i < all_chess_attack_pos.Count; i++)
            {
                if (all_chess_attack_pos[i][0] == "right")//---------------P2
                {
                    if (all_chess_attack_pos[i][1] == "warrior")
                    {
                        now_checker = all_chess_attack_pos[i][2];
                        int one = now_checker[0] - 65;
                        int two = int.Parse(now_checker[1].ToString()) - 1;
                        Vector2 pos = checker[one][two].transform.position;
                        p2_warrior[p2_warrior_put_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                        p2_warrior_put_last = p2_warrior_put_last - 1;
                        StartCoroutine(p2_warrior_move_fun(pos.x, pos.y));
                    }
                    else if (all_chess_attack_pos[i][1] == "knight")
                    {
                        now_checker = all_chess_attack_pos[i][2];
                        int one = now_checker[0] - 65;
                        int two = int.Parse(now_checker[1].ToString()) - 1;
                        Vector2 pos = checker[one][two].transform.position;
                        p2_knight[p2_knight_put_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                        p2_knight_put_last = p2_knight_put_last - 1;
                        StartCoroutine(p2_knight_move_fun(pos.x, pos.y));
                    }
                    else if (all_chess_attack_pos[i][1] == "magician")
                    {
                        now_checker = all_chess_attack_pos[i][2];
                        int one = now_checker[0] - 65;
                        int two = int.Parse(now_checker[1].ToString()) - 1;
                        Vector2 pos = checker[one][two].transform.position;
                        p2_magician[p2_magician_put_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                        p2_magician_put_last = p2_magician_put_last - 1;
                        StartCoroutine(p2_magician_move_fun(pos.x, pos.y));
                    }
                    else if (all_chess_attack_pos[i][1] == "archer")
                    {
                        now_checker = all_chess_attack_pos[i][2];
                        int one = now_checker[0] - 65;
                        int two = int.Parse(now_checker[1].ToString()) - 1;
                        Vector2 pos = checker[one][two].transform.position;
                        p2_archer[p2_archer_put_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                        p2_archer_put_last = p2_archer_put_last - 1;
                        StartCoroutine(p2_archer_move_fun(pos.x, pos.y));
                    }
                    else if (all_chess_attack_pos[i][1] == "monk")
                    {
                        now_checker = all_chess_attack_pos[i][2];
                        int one = now_checker[0] - 65;
                        int two = int.Parse(now_checker[1].ToString()) - 1;
                        Vector2 pos = checker[one][two].transform.position;
                        p2_monk[p2_monk_put_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                        p2_monk_put_last = p2_monk_put_last - 1;
                        StartCoroutine(p2_monk_move_fun(pos.x, pos.y));
                    }
                }
                else//-----------------------------------------------------P1
                {
                    if (all_chess_attack_pos[i][1] == "warrior")
                    {
                        now_checker = all_chess_attack_pos[i][2];
                        int one = now_checker[0] - 65;
                        int two = int.Parse(now_checker[1].ToString()) - 1;
                        Vector2 pos = checker[one][two].transform.position;
                        p1_warrior[p1_warrior_put_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                        p1_warrior_put_last = p1_warrior_put_last - 1;
                        StartCoroutine(p1_warrior_move_fun(pos.x, pos.y));
                    }
                    else if (all_chess_attack_pos[i][1] == "knight")
                    {
                        now_checker = all_chess_attack_pos[i][2];
                        int one = now_checker[0] - 65;
                        int two = int.Parse(now_checker[1].ToString()) - 1;
                        Vector2 pos = checker[one][two].transform.position;
                        p1_knight[p1_knight_put_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                        p1_knight_put_last = p1_knight_put_last - 1;
                        StartCoroutine(p1_knight_move_fun(pos.x, pos.y));
                    }
                    else if (all_chess_attack_pos[i][1] == "magician")
                    {
                        now_checker = all_chess_attack_pos[i][2];
                        int one = now_checker[0] - 65;
                        int two = int.Parse(now_checker[1].ToString()) - 1;
                        Vector2 pos = checker[one][two].transform.position;
                        p1_magician[p1_magician_put_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                        p1_magician_put_last = p1_magician_put_last - 1;
                        StartCoroutine(p1_magician_move_fun(pos.x, pos.y));
                    }
                    else if (all_chess_attack_pos[i][1] == "archer")
                    {
                        now_checker = all_chess_attack_pos[i][2];
                        int one = now_checker[0] - 65;
                        int two = int.Parse(now_checker[1].ToString()) - 1;
                        Vector2 pos = checker[one][two].transform.position;
                        p1_archer[p1_archer_put_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                        p1_archer_put_last = p1_archer_put_last - 1;
                        StartCoroutine(p1_archer_move_fun(pos.x, pos.y));
                    }
                    else if (all_chess_attack_pos[i][1] == "monk")
                    {
                        now_checker = all_chess_attack_pos[i][2];
                        int one = now_checker[0] - 65;
                        int two = int.Parse(now_checker[1].ToString()) - 1;
                        Vector2 pos = checker[one][two].transform.position;
                        p1_monk[p1_monk_put_last - 1].transform.position = new Vector3(pos.x, pos.y + 1, 0);
                        p1_monk_put_last = p1_monk_put_last - 1;
                        StartCoroutine(p1_monk_move_fun(pos.x, pos.y));
                    }
                }
            }
        }


        Battle_on();
    }




    //戰鬥開始
    public void Battle_on()
    {

        if (isLocalPlayer)
        {
            string now_checker = "";


            List<List<string>> left_warrior = new List<List<string>>();
            List<List<string>> right_warrior = new List<List<string>>();

            List<List<string>> left_knight = new List<List<string>>();
            List<List<string>> right_knight = new List<List<string>>();

            List<List<string>> left_archer = new List<List<string>>();
            List<List<string>> right_archer = new List<List<string>>();

            List<List<string>> left_magician = new List<List<string>>();
            List<List<string>> right_magician = new List<List<string>>();

            List<List<string>> left_monk = new List<List<string>>();
            List<List<string>> right_monk = new List<List<string>>();







            //先把棋盤顏色恢復

            for (int i = 0; i < color_pos.Count; i++)
            {
                for (int j = 0; j < color_pos[i].Count; j++)
                {

                    checker[i][j].gameObject.SetActive(true);

                    if (color_pos[i][j] == "red")
                    {
                        checker[i][j].GetComponent<Image>().color = Color.red;
                    }

                    else
                    {
                        checker[i][j].GetComponent<Image>().color = Color.blue;
                    }

                }
            }


            //Thread.Sleep(3000);



            //先把所有資訊歸類 分成各職業和位置 方便之後順序判斷

            for (int i = 0; i < all_chess_attack_pos.Count; i++)
            {


                //僧侶 左邊和右邊歸類

                if (all_chess_attack_pos[i][0] == "left" && all_chess_attack_pos[i][1] == "monk")
                {

                    left_monk.Add(new List<string>());

                    for (int j = 0; j < all_chess_attack_pos[i].Count; j++)
                    {
                        left_monk[left_monk.Count - 1].Add(all_chess_attack_pos[i][j]);
                    }
                }

                else if (all_chess_attack_pos[i][0] == "right" && all_chess_attack_pos[i][1] == "monk")
                {
                    right_monk.Add(new List<string>());

                    for (int j = 0; j < all_chess_attack_pos[i].Count; j++)
                    {
                        right_monk[right_monk.Count - 1].Add(all_chess_attack_pos[i][j]);
                    }
                }




                //騎士 左邊和右邊歸類

                if (all_chess_attack_pos[i][0] == "left" && all_chess_attack_pos[i][1] == "knight")
                {

                    left_knight.Add(new List<string>());

                    for (int j = 0; j < all_chess_attack_pos[i].Count; j++)
                    {
                        left_knight[left_knight.Count - 1].Add(all_chess_attack_pos[i][j]);
                    }
                }

                else if (all_chess_attack_pos[i][0] == "right" && all_chess_attack_pos[i][1] == "knight")
                {
                    right_knight.Add(new List<string>());

                    for (int j = 0; j < all_chess_attack_pos[i].Count; j++)
                    {
                        right_knight[right_knight.Count - 1].Add(all_chess_attack_pos[i][j]);
                    }
                }


                //弓箭手 左邊和右邊歸類

                if (all_chess_attack_pos[i][0] == "left" && all_chess_attack_pos[i][1] == "archer")
                {

                    left_archer.Add(new List<string>());

                    for (int j = 0; j < all_chess_attack_pos[i].Count; j++)
                    {
                        left_archer[left_archer.Count - 1].Add(all_chess_attack_pos[i][j]);
                    }
                }

                else if (all_chess_attack_pos[i][0] == "right" && all_chess_attack_pos[i][1] == "archer")
                {
                    right_archer.Add(new List<string>());

                    for (int j = 0; j < all_chess_attack_pos[i].Count; j++)
                    {
                        right_archer[right_archer.Count - 1].Add(all_chess_attack_pos[i][j]);
                    }
                }



                //劍士 左邊和右邊歸類

                if (all_chess_attack_pos[i][0] == "left" && all_chess_attack_pos[i][1] == "warrior")
                {

                    left_warrior.Add(new List<string>());

                    for (int j = 0; j < all_chess_attack_pos[i].Count; j++)
                    {
                        left_warrior[left_warrior.Count - 1].Add(all_chess_attack_pos[i][j]);
                    }
                }

                else if (all_chess_attack_pos[i][0] == "right" && all_chess_attack_pos[i][1] == "warrior")
                {
                    right_warrior.Add(new List<string>());

                    for (int j = 0; j < all_chess_attack_pos[i].Count; j++)
                    {
                        right_warrior[right_warrior.Count - 1].Add(all_chess_attack_pos[i][j]);
                    }
                }



                //法師 左邊和右邊歸類

                if (all_chess_attack_pos[i][0] == "left" && all_chess_attack_pos[i][1] == "magician")
                {

                    left_magician.Add(new List<string>());

                    for (int j = 0; j < all_chess_attack_pos[i].Count; j++)
                    {
                        left_magician[left_magician.Count - 1].Add(all_chess_attack_pos[i][j]);
                    }
                }

                else if (all_chess_attack_pos[i][0] == "right" && all_chess_attack_pos[i][1] == "magician")
                {
                    right_magician.Add(new List<string>());

                    for (int j = 0; j < all_chess_attack_pos[i].Count; j++)
                    {
                        right_magician[right_magician.Count - 1].Add(all_chess_attack_pos[i][j]);
                    }
                }


            }


            /*
            //印出看一下 棋子情況
            for (int i = 0; i < left_warrior.Count; i++)
            {
                for (int j = 0; j < left_warrior[i].Count; j++)
                {
                    Debug.Log(left_warrior[i][j]);
                }
            }*/



            //開始對戰計算

            //InvokeRepeating(attack_countdown, 1, 2);

            /*    僧侶    */
            Debug.Log("僧侶回合");


            List<string> left_monk_heal = new List<string>();
            List<string> right_monk_heal = new List<string>();



            //先把僧侶的治癒位置存起來

            for (int i = 0; i < left_monk.Count; i++)
            {
                left_monk_heal.Add(left_monk[i][3]);
            }

            for (int i = 0; i < right_monk.Count; i++)
            {
                right_monk_heal.Add(right_monk[i][3]);
            }



            /*    僧侶特效     */

            for (int i = 0; i < left_monk_heal.Count; i++)
            {
                now_checker = left_monk_heal[i];
                int one = now_checker[0] - 65;
                int two = int.Parse(now_checker[1].ToString()) - 1;
                Vector2 pos = checker[one][two].transform.position;
                StartCoroutine(monk_countdown(pos.x, pos.y));
            }
            for (int i = 0; i < right_monk_heal.Count; i++)
            {
                now_checker = right_monk_heal[i];
                int one = now_checker[0] - 65;
                int two = int.Parse(now_checker[1].ToString()) - 1;
                Vector2 pos = checker[one][two].transform.position;
                StartCoroutine(monk_countdown(pos.x, pos.y));
            }

            /*    騎士(盾牌)計算    */

            Debug.Log("騎士回合");

            List<string> left_knight_defend = new List<string>();
            List<string> right_knight_defend = new List<string>();


            //先把騎士的保護位置存起來
            for (int i = 0; i < left_knight.Count; i++)
            {
                for (int j = 2; j < left_knight[i].Count; j++)
                {
                    left_knight_defend.Add(left_knight[i][j]);
                }
            }


            for (int i = 0; i < right_knight.Count; i++)
            {
                for (int j = 2; j < right_knight[i].Count; j++)
                {
                    right_knight_defend.Add(right_knight[i][j]);
                }
            }


            //Thread.Sleep(3000);
            /*   騎士特效放在這裡  */

            for (int i = 0; i < left_knight_defend.Count; i++)
            {
                now_checker = left_knight_defend[i];
                int one = now_checker[0] - 65;
                int two = int.Parse(now_checker[1].ToString()) - 1;
                Vector2 pos = checker[one][two].transform.position;
                StartCoroutine(knight_countdown(pos.x, pos.y));
            }
            for (int i = 0; i < right_knight_defend.Count; i++)
            {
                now_checker = right_knight_defend[i];
                int one = now_checker[0] - 65;
                int two = int.Parse(now_checker[1].ToString()) - 1;
                Vector2 pos = checker[one][two].transform.position;
                StartCoroutine(knight_countdown(pos.x, pos.y));
            }

            Debug.Log("弓箭回合");


            /*    弓手計算    */

            List<string> left_archer_attack = new List<string>();
            List<string> right_archer_attack = new List<string>();


            //先把弓手的攻擊位置存起來
            for (int i = 0; i < left_archer.Count; i++)
            {
                for (int j = 3; j < left_archer[i].Count; j++)
                {
                    left_archer_attack.Add(left_archer[i][j]);
                }
            }


            for (int i = 0; i < right_archer.Count; i++)
            {
                for (int j = 3; j < right_archer[i].Count; j++)
                {
                    right_archer_attack.Add(right_archer[i][j]);
                }
            }


            //弓手互相攻擊的情況

            //左邊攻擊右邊
            for (int i = 0; i < left_archer_attack.Count; i++)
            {

                int pivot = 0;

                for (int j = 0; j < right_archer.Count; j++)
                {
                    if (left_archer_attack[i] == right_archer[pivot][2])
                    {

                        //判斷被攻擊角色 是不是有被僧侶保護
                        bool hit = true;

                        for (int k = 0; k < right_monk_heal.Count; k++)
                        {
                            if (right_archer[pivot][2] == right_monk_heal[k])
                            {
                                hit = false;
                            }
                        }

                        if (hit == true)
                        {
                            //---------------------------------------
                            now_checker = right_archer[pivot][2];
                            int one = now_checker[0] - 65;
                            int two = int.Parse(now_checker[1].ToString()) - 1;
                            Vector2 pos = checker[one][two].transform.position;
                            StartCoroutine(die_archer(pos.x, pos.y));
                            right_archer.RemoveAt(pivot);
                            pivot--;
                        }

                    }

                    pivot++;
                }
            }

            //弓手互相攻擊的情況
            //右邊攻擊左邊

            for (int i = 0; i < right_archer_attack.Count; i++)
            {

                int pivot = 0;

                for (int j = 0; j < left_archer.Count; j++)
                {
                    if (right_archer_attack[i] == left_archer[pivot][2])
                    {
                        //判斷被攻擊角色 是不是有被僧侶保護
                        bool hit = true;

                        for (int k = 0; k < left_monk_heal.Count; k++)
                        {
                            if (left_archer[pivot][2] == left_monk_heal[k])
                            {
                                hit = false;
                            }
                        }

                        if (hit == true)
                        {
                            //-------------------------------------
                            now_checker = left_archer[pivot][2];
                            int one = now_checker[0] - 65;
                            int two = int.Parse(now_checker[1].ToString()) - 1;
                            Vector2 pos = checker[one][two].transform.position;
                            StartCoroutine(die_archer(pos.x, pos.y));
                            left_archer.RemoveAt(pivot);
                            pivot--;
                        }
                    }

                    pivot++;
                }
            }



            //左邊攻擊右邊

            for (int i = 0; i < left_archer.Count; i++)
            {

                //弓手攻擊劍士

                int pivot = 0;

                for (int j = 0; j < right_warrior.Count; j++)
                {
                    for (int k = 3; k < left_archer[i].Count; k++)
                    {
                        if (left_archer[i][k] == right_warrior[pivot][2])
                        {

                            //判斷被攻擊角色 是不是有被僧侶保護

                            bool hit = true;

                            for (int m = 0; m < right_monk_heal.Count; m++)
                            {
                                if (right_warrior[pivot][2] == right_monk_heal[m])
                                {
                                    hit = false;
                                }
                            }

                            if (hit == true)
                            {
                                //---------------------------------------
                                now_checker = right_warrior[pivot][2];
                                int one = now_checker[0] - 65;
                                int two = int.Parse(now_checker[1].ToString()) - 1;
                                Vector2 pos = checker[one][two].transform.position;
                                StartCoroutine(die_archer(pos.x, pos.y));
                                right_warrior.RemoveAt(pivot);
                                pivot--;

                                break;
                            }

                        }
                    }

                    pivot++;
                }


                //弓手攻擊魔法師

                pivot = 0;

                for (int j = 0; j < right_magician.Count; j++)
                {
                    for (int k = 3; k < left_archer[i].Count; k++)
                    {
                        if (left_archer[i][k] == right_magician[pivot][2])
                        {
                            //判斷被攻擊角色 是不是有被僧侶保護

                            bool hit = true;

                            for (int m = 0; m < right_monk_heal.Count; m++)
                            {
                                if (right_magician[pivot][2] == right_monk_heal[m])
                                {
                                    hit = false;
                                }
                            }

                            if (hit == true)
                            {
                                //-----------------------------------------------------
                                now_checker = right_magician[pivot][2];
                                int one = now_checker[0] - 65;
                                int two = int.Parse(now_checker[1].ToString()) - 1;
                                Vector2 pos = checker[one][two].transform.position;
                                StartCoroutine(die_archer(pos.x, pos.y));
                                right_magician.RemoveAt(pivot);
                                pivot--;

                                break;
                            }

                        }
                    }

                    pivot++;
                }
            }


            //右邊攻擊左邊

            for (int i = 0; i < right_archer.Count; i++)
            {

                //弓手攻擊劍士

                int pivot = 0;

                for (int j = 0; j < left_warrior.Count; j++)
                {
                    for (int k = 3; k < right_archer[i].Count; k++)
                    {
                        if (right_archer[i][k] == left_warrior[pivot][2])
                        {
                            //判斷被攻擊角色 是不是有被僧侶保護

                            bool hit = true;

                            for (int m = 0; m < left_monk_heal.Count; m++)
                            {
                                if (left_warrior[pivot][2] == left_monk_heal[m])
                                {
                                    hit = false;
                                }
                            }

                            if (hit == true)
                            {
                                //----------------------------------------------------------
                                now_checker = left_warrior[pivot][2];
                                int one = now_checker[0] - 65;
                                int two = int.Parse(now_checker[1].ToString()) - 1;
                                Vector2 pos = checker[one][two].transform.position;
                                StartCoroutine(die_archer(pos.x, pos.y));
                                left_warrior.RemoveAt(pivot);
                                pivot--;

                                break;
                            }
                        }
                    }

                    pivot++;
                }


                //弓手攻擊魔法師

                pivot = 0;

                for (int j = 0; j < left_magician.Count; j++)
                {
                    for (int k = 3; k < right_archer[i].Count; k++)
                    {
                        if (right_archer[i][k] == left_magician[pivot][2])
                        {

                            //判斷被攻擊角色 是不是有被僧侶保護

                            bool hit = true;

                            for (int m = 0; m < left_monk_heal.Count; m++)
                            {
                                if (left_magician[pivot][2] == left_monk_heal[m])
                                {
                                    hit = false;
                                }
                            }

                            if (hit == true)
                            {
                                //------------------------------------
                                now_checker = left_magician[pivot][2];
                                int one = now_checker[0] - 65;
                                int two = int.Parse(now_checker[1].ToString()) - 1;
                                Vector2 pos = checker[one][two].transform.position;
                                StartCoroutine(die_archer(pos.x, pos.y));
                                left_magician.RemoveAt(pivot);
                                pivot--;

                                break;
                            }
                        }
                    }

                    pivot++;
                }
            }





            List<string> word = new List<string> { "A", "B", "C", "D", "E", "F" };


            //選到相似的領土的計算

            //存攻擊相似領土的位置以及 要變成的顏色 ["A1", "blue"]
            List<string> same_archer_attack = new List<string>();

            char[] same_archer_arr;
            int index_site;

            for (int i = 0; i < left_archer.Count; i++)
            {
                for (int j = 3; j < left_archer[i].Count; j++)
                {
                    for (int k = 0; k < right_archer.Count; k++)
                    {
                        index_site = right_archer[k].IndexOf(left_archer[i][j].ToString());


                        //找到攻擊一樣位置的領土
                        if (index_site > 2)
                        {

                            same_archer_arr = left_archer[i][j].ToCharArray();


                            int row = word.IndexOf(same_archer_arr[0].ToString());
                            int col = int.Parse(same_archer_arr[1].ToString()) - 1;


                            //如果原本是紅的 就把他記成藍的
                            if (checker[row][col].GetComponent<Image>().color == Color.red)
                            {

                                //如果遇到騎士守護

                                if (left_knight_defend.Contains(checker[row][col].gameObject.ToString()))
                                {
                                    continue;
                                }


                                same_archer_attack.Add(checker[row][col].ToString());
                                same_archer_attack.Add("blue");
                            }

                            else if (checker[row][col].GetComponent<Image>().color == Color.blue)
                            {

                                if (right_knight_defend.Contains(checker[row][col].gameObject.ToString()))
                                {
                                    continue;
                                }

                                same_archer_attack.Add(checker[row][col].ToString());
                                same_archer_attack.Add("red");
                            }

                        }

                    }
                }
            }







            //Thread.Sleep(3000);
            /*    弓箭特效       */

            for (int i = 0; i < left_archer.Count; i++)
            {
                for (int j = 3; j < left_archer[i].Count; j++)
                {
                    now_checker = left_archer[i][j];
                    int one = now_checker[0] - 65;
                    int two = int.Parse(now_checker[1].ToString()) - 1;
                    Vector2 pos = checker[one][two].transform.position;
                    StartCoroutine(archer_countdown(pos.x, pos.y, left_archer, right_archer, left_knight_defend, right_knight_defend, same_archer_attack, word));
                }
            }
            for (int i = 0; i < right_archer.Count; i++)
            {
                for (int j = 3; j < right_archer[i].Count; j++)
                {
                    now_checker = right_archer[i][j];
                    int one = now_checker[0] - 65;
                    int two = int.Parse(now_checker[1].ToString()) - 1;
                    Vector2 pos = checker[one][two].transform.position;
                    StartCoroutine(archer_countdown(pos.x, pos.y, left_archer, right_archer, left_knight_defend, right_knight_defend, same_archer_attack, word));
                }
            }


            Debug.Log("劍士回合");


            /*    劍士計算    */

            List<string> left_warrior_attack = new List<string>();
            List<string> right_warrior_attack = new List<string>();


            //先把劍士的攻擊位置存起來
            for (int i = 0; i < left_warrior.Count; i++)
            {
                for (int j = 3; j < left_warrior[i].Count; j++)
                {
                    left_warrior_attack.Add(left_warrior[i][j]);
                }
            }


            for (int i = 0; i < right_warrior.Count; i++)
            {
                for (int j = 3; j < right_warrior[i].Count; j++)
                {
                    right_warrior_attack.Add(right_warrior[i][j]);
                }
            }


            //劍士互相攻擊的情況 把被砍到的從陣列裡移除
            for (int i = 0; i < left_warrior_attack.Count; i++)
            {

                int pivot = 0;

                for (int j = 0; j < right_warrior.Count; j++)
                {
                    if (left_warrior_attack[i] == right_warrior[pivot][2])
                    {
                        //判斷被攻擊角色 是不是有被僧侶保護

                        bool hit = true;

                        for (int m = 0; m < right_monk_heal.Count; m++)
                        {
                            if (right_warrior[pivot][2] == right_monk_heal[m])
                            {
                                hit = false;
                            }
                        }

                        if (hit == true)
                        {
                            //--------------------------------------------------------
                            now_checker = right_warrior[pivot][2];
                            int one = now_checker[0] - 65;
                            int two = int.Parse(now_checker[1].ToString()) - 1;
                            Vector2 pos = checker[one][two].transform.position;
                            StartCoroutine(die_warrior(pos.x, pos.y));
                            right_warrior.RemoveAt(pivot);
                            pivot--;
                        }
                    }

                    pivot++;
                }
            }


            for (int i = 0; i < right_warrior_attack.Count; i++)
            {

                int pivot = 0;

                for (int j = 0; j < left_warrior.Count; j++)
                {
                    if (right_warrior_attack[i] == left_warrior[pivot][2])
                    {

                        //判斷被攻擊角色 是不是有被僧侶保護

                        bool hit = true;

                        for (int m = 0; m < left_monk_heal.Count; m++)
                        {
                            if (left_warrior[pivot][2] == left_monk_heal[m])
                            {
                                hit = false;
                            }
                        }

                        if (hit == true)
                        {
                            //-------------------------------------------
                            now_checker = left_warrior[pivot][2];
                            int one = now_checker[0] - 65;
                            int two = int.Parse(now_checker[1].ToString()) - 1;
                            Vector2 pos = checker[one][two].transform.position;
                            StartCoroutine(die_warrior(pos.x, pos.y));
                            left_warrior.RemoveAt(pivot);
                            pivot--;
                        }
                    }

                    pivot++;
                }
            }


            //左邊攻擊右邊

            for (int i = 0; i < left_warrior.Count; i++)
            {

                //劍士攻擊魔法師

                int pivot = 0;

                for (int j = 0; j < right_magician.Count; j++)
                {
                    for (int k = 3; k < left_warrior[i].Count; k++)
                    {
                        if (left_warrior[i][k] == right_magician[pivot][2])
                        {

                            //判斷被攻擊角色 是不是有被僧侶保護

                            bool hit = true;

                            for (int m = 0; m < right_monk_heal.Count; m++)
                            {
                                if (right_magician[pivot][2] == right_monk_heal[m])
                                {
                                    hit = false;
                                }
                            }

                            if (hit == true)
                            {
                                //----------------------------------------
                                now_checker = right_magician[pivot][2];
                                int one = now_checker[0] - 65;
                                int two = int.Parse(now_checker[1].ToString()) - 1;
                                Vector2 pos = checker[one][two].transform.position;
                                StartCoroutine(die_warrior(pos.x, pos.y));
                                right_magician.RemoveAt(pivot);
                                pivot--;


                                break;
                            }
                        }
                    }

                    pivot++;
                }
            }


            //右邊攻擊左邊

            for (int i = 0; i < right_warrior.Count; i++)
            {


                //劍士攻擊魔法師

                int pivot = 0;

                for (int j = 0; j < left_magician.Count; j++)
                {
                    for (int k = 3; k < right_warrior[i].Count; k++)
                    {
                        if (right_warrior[i][k] == left_magician[pivot][2])
                        {
                            //判斷被攻擊角色 是不是有被僧侶保護

                            bool hit = true;

                            for (int m = 0; m < left_monk_heal.Count; m++)
                            {
                                if (left_magician[pivot][2] == left_monk_heal[m])
                                {
                                    hit = false;
                                }
                            }

                            if (hit == true)
                            {
                                //-------------------------------------------------
                                now_checker = left_magician[pivot][2];
                                int one = now_checker[0] - 65;
                                int two = int.Parse(now_checker[1].ToString()) - 1;
                                Vector2 pos = checker[one][two].transform.position;
                                StartCoroutine(die_warrior(pos.x, pos.y));
                                left_magician.RemoveAt(pivot);
                                pivot--;


                                break;
                            }
                        }
                    }

                    pivot++;
                }
            }


            //將結果顯示在畫面上

            //選到相似的領土的計算

            //存攻擊相似領土的位置以及 要變成的顏色 ["A1", "blue"]
            List<string> same_warrior_attack = new List<string>();

            char[] same_warrior_arr;


            for (int i = 0; i < left_warrior.Count; i++)
            {
                for (int j = 3; j < left_warrior[i].Count; j++)
                {
                    for (int k = 0; k < right_warrior.Count; k++)
                    {
                        index_site = right_warrior[k].IndexOf(left_warrior[i][j].ToString());


                        //找到攻擊一樣位置的領土
                        if (index_site > 2)
                        {

                            same_warrior_arr = left_warrior[i][j].ToCharArray();


                            int row = word.IndexOf(same_warrior_arr[0].ToString());
                            int col = int.Parse(same_warrior_arr[1].ToString()) - 1;


                            //如果原本是紅的 就把他記成藍的
                            if (checker[row][col].GetComponent<Image>().color == Color.red)
                            {

                                //如果遇到騎士守護

                                if (left_knight_defend.Contains(checker[row][col].gameObject.ToString()))
                                {
                                    continue;
                                }


                                same_warrior_attack.Add(checker[row][col].ToString());
                                same_warrior_attack.Add("blue");
                            }

                            else if (checker[row][col].GetComponent<Image>().color == Color.blue)
                            {

                                if (right_knight_defend.Contains(checker[row][col].gameObject.ToString()))
                                {
                                    continue;
                                }

                                same_warrior_attack.Add(checker[row][col].ToString());
                                same_warrior_attack.Add("red");
                            }

                        }

                    }
                }
            }








            /*    劍士特效       */

            for (int i = 0; i < left_warrior.Count; i++)
            {
                for (int j = 3; j < left_warrior[i].Count; j++)
                {
                    now_checker = left_warrior[i][j];
                    int one = now_checker[0] - 65;
                    int two = int.Parse(now_checker[1].ToString()) - 1;
                    Vector2 pos = checker[one][two].transform.position;
                    StartCoroutine(warrior_countdown(pos.x, pos.y, left_warrior, right_warrior, left_knight_defend, right_knight_defend, same_warrior_attack, word));
                }
            }
            for (int i = 0; i < right_warrior.Count; i++)
            {
                for (int j = 3; j < right_warrior[i].Count; j++)
                {
                    now_checker = right_warrior[i][j];
                    int one = now_checker[0] - 65;
                    int two = int.Parse(now_checker[1].ToString()) - 1;
                    Vector2 pos = checker[one][two].transform.position;
                    StartCoroutine(warrior_countdown(pos.x, pos.y, left_warrior, right_warrior, left_knight_defend, right_knight_defend, same_warrior_attack, word));
                }
            }


            /*    法師計算    */

            Debug.Log("法師回合");

            List<string> left_magician_attack = new List<string>();
            List<string> right_magician_attack = new List<string>();


            //先把法師的攻擊位置存起來
            for (int i = 0; i < left_magician.Count; i++)
            {
                for (int j = 3; j < left_magician[i].Count; j++)
                {
                    left_magician_attack.Add(left_magician[i][j]);
                }
            }


            for (int i = 0; i < right_magician.Count; i++)
            {
                for (int j = 3; j < right_magician[i].Count; j++)
                {
                    right_magician_attack.Add(right_magician[i][j]);
                }
            }


            //法師互相攻擊的情況 把被燒到的從陣列裡移除
            for (int i = 0; i < left_magician_attack.Count; i++)
            {

                int pivot = 0;

                for (int j = 0; j < right_magician.Count; j++)
                {
                    if (left_magician_attack[i] == right_magician[pivot][2])
                    {
                        //判斷被攻擊角色 是不是有被僧侶保護

                        bool hit = true;

                        for (int m = 0; m < right_monk_heal.Count; m++)
                        {
                            if (right_magician[pivot][2] == right_monk_heal[m])
                            {
                                hit = false;
                            }
                        }

                        if (hit == true)
                        {
                            //-------------------------------------------
                            now_checker = right_magician[pivot][2];
                            int one = now_checker[0] - 65;
                            int two = int.Parse(now_checker[1].ToString()) - 1;
                            Vector2 pos = checker[one][two].transform.position;
                            StartCoroutine(die_magician(pos.x, pos.y));
                            right_magician.RemoveAt(pivot);
                            pivot--;
                        }
                    }

                    pivot++;
                }
            }


            for (int i = 0; i < right_magician_attack.Count; i++)
            {

                int pivot = 0;

                for (int j = 0; j < left_magician.Count; j++)
                {
                    if (right_magician_attack[i] == left_magician[pivot][2])
                    {
                        //判斷被攻擊角色 是不是有被僧侶保護
                        bool hit = true;

                        for (int k = 0; k < left_monk_heal.Count; k++)
                        {
                            if (left_magician[pivot][2] == left_monk_heal[k])
                            {
                                hit = false;
                            }
                        }

                        if (hit == true)
                        {
                            //-----------------------------------------
                            now_checker = left_magician[pivot][2];
                            int one = now_checker[0] - 65;
                            int two = int.Parse(now_checker[1].ToString()) - 1;
                            Vector2 pos = checker[one][two].transform.position;
                            StartCoroutine(die_magician(pos.x, pos.y));
                            left_magician.RemoveAt(pivot);
                            pivot--;
                        }
                    }

                    pivot++;
                }
            }



            //將結果顯示在畫面上

            //選到相似的領土的計算

            //存攻擊相似領土的位置以及 要變成的顏色 ["A1", "blue"]
            List<string> same_magician_attack = new List<string>();

            char[] same_magician_arr;


            for (int i = 0; i < left_magician.Count; i++)
            {
                for (int j = 3; j < left_magician[i].Count; j++)
                {
                    for (int k = 0; k < right_magician.Count; k++)
                    {
                        index_site = right_magician[k].IndexOf(left_magician[i][j].ToString());


                        //找到攻擊一樣位置的領土
                        if (index_site > 2)
                        {

                            same_magician_arr = left_magician[i][j].ToCharArray();


                            int row = word.IndexOf(same_magician_arr[0].ToString());
                            int col = int.Parse(same_magician_arr[1].ToString()) - 1;


                            //如果原本是紅的 就把他記成藍的
                            if (checker[row][col].GetComponent<Image>().color == Color.red)
                            {

                                //如果遇到騎士守護

                                if (left_knight_defend.Contains(checker[row][col].gameObject.ToString()))
                                {
                                    continue;
                                }


                                same_magician_attack.Add(checker[row][col].ToString());
                                same_magician_attack.Add("blue");
                            }

                            else if (checker[row][col].GetComponent<Image>().color == Color.blue)
                            {

                                if (right_knight_defend.Contains(checker[row][col].gameObject.ToString()))
                                {
                                    continue;
                                }

                                same_magician_attack.Add(checker[row][col].ToString());
                                same_magician_attack.Add("red");
                            }

                        }

                    }
                }
            }







            //法師特效

            for (int i = 0; i < left_magician.Count; i++)
            {
                for (int j = 3; j < left_magician[i].Count; j++)
                {
                    now_checker = left_magician[i][j];
                    int one = now_checker[0] - 65;
                    int two = int.Parse(now_checker[1].ToString()) - 1;
                    Vector2 pos = checker[one][two].transform.position;
                    StartCoroutine(magician_countdown(pos.x, pos.y, left_magician, right_magician, left_knight_defend, right_knight_defend, same_magician_attack, word));
                }
            }
            for (int i = 0; i < right_magician.Count; i++)
            {
                for (int j = 3; j < right_magician[i].Count; j++)
                {
                    now_checker = right_magician[i][j];
                    int one = now_checker[0] - 65;
                    int two = int.Parse(now_checker[1].ToString()) - 1;
                    Vector2 pos = checker[one][two].transform.position;
                    StartCoroutine(magician_countdown(pos.x, pos.y, left_magician, right_magician, left_knight_defend, right_knight_defend, same_magician_attack, word));
                }
            }


            StartCoroutine(battle_end_count_down());

        }//ifisLocalPlayer

    }


    //對戰統計
    [ClientRpc]
    public void RpcCalculate_territory()
    {

        if (isLocalPlayer)
        {
            int red_num = 0;
            int blue_num = 0;


            //統計分數

            for (int i = 0; i < color_pos.Count; i++)
            {
                for (int j = 0; j < color_pos[i].Count; j++)
                {
                    if (color_pos[i][j] == "red")
                    {
                        red_num++;
                    }

                    else if (color_pos[i][j] == "blue")
                    {
                        blue_num++;
                    }
                }
            }


            if (red_num > blue_num)
            {
                if (player.GetComponent<Player>().battle_position == "left")
                {
                    player.GetComponent<Player>().win = true;
                    win_picture.SetActive(true);
                }

                else if (player.GetComponent<Player>().battle_position == "right")
                {
                    player.GetComponent<Player>().win = false;
                    lose_picture.SetActive(true);
                }
            }

            else if (red_num < blue_num)
            {
                if (player.GetComponent<Player>().battle_position == "left")
                {
                    player.GetComponent<Player>().win = false;
                    lose_picture.SetActive(true);
                }

                else if (player.GetComponent<Player>().battle_position == "right")
                {
                    player.GetComponent<Player>().win = true;
                    win_picture.SetActive(true);
                }
            }

            else if (red_num == blue_num)
            {
                player.GetComponent<Player>().win = true;
                win_picture.SetActive(true);
            }

            //把List全部清空
            Battle_end_clear();


            battle_end_notice.SetActive(true);

            StartCoroutine(win_lose_interface_count_down());



        }
    }

    //全部回歸初始狀態
    public void Battle_end_clear()
    {
        color_pos.Clear();
        my_chess_attack_pos.Clear();
        all_chess_attack_pos.Clear();
        checker.Clear();

        p1_warrior.Clear();
        p2_warrior.Clear();
        p1_knight.Clear();
        p2_knight.Clear();
        p1_magician.Clear();
        p2_magician.Clear();
        p1_archer.Clear();
        p2_archer.Clear();
        p1_monk.Clear();
        p2_monk.Clear();
        copy_monk_attack.Clear();
        copy_warrior_attack.Clear();
        copy_knight_attack.Clear();
        copy_magician_attack.Clear();
        copy_archer_attack.Clear();


        timer = 30;
        start_timer = true;
        prepare = true;
        battle_end = false;
        restart = false;
        round = 0;
        current_put_num = 0;
        warrior_last = 4;
        p1_warrior_put_last = 4;
        p2_warrior_put_last = 4;
        p1_warrior_clear = 4;
        p2_warrior_clear = 4;
        p1_warrior_move = 4;
        p2_warrior_move = 4;
        knight_last = 2;
        p1_knight_put_last = 2;
        p2_knight_put_last = 2;
        p1_knight_clear = 2;
        p2_knight_clear = 2;
        p1_knight_move = 2;
        p2_knight_move = 2;
        archer_last = 3;
        p1_archer_put_last = 3;
        p2_archer_put_last = 3;
        p1_archer_clear = 3;
        p2_archer_clear = 3;
        p1_archer_move = 3;
        p2_archer_move = 3;
        magician_last = 2;
        p1_magician_put_last = 2;
        p2_magician_put_last = 2;
        p1_magician_clear = 2;
        p2_magician_clear = 2;
        p1_magician_move = 2;
        p2_magician_move = 2;
        monk_last = 1;
        p1_monk_put_last = 1;
        p2_monk_put_last = 1;
        p1_monk_clear = 1;
        p2_monk_clear = 1;
        p1_monk_move = 1;
        p2_monk_move = 1;
        select = 0;
        warrior_attacktime = 2;
        magician_attacktime = 2;
        archer_attacktime = 4;
        monk_attacktime = 2;



        start_download_chess_func = false;


        warrior_one = 0;
        warrior_two = 0;
        put_done = "";
        warrior_pos = "";

    }

    //換場景 換回island
    public void Change_Screen()
    {
        SceneManager.LoadScene("island");
        player.GetComponent<island_UI>().restart = true;
    }


    //----------------------- 特效

    IEnumerator die_archer(float posx, float posy)
    {
        yield return new WaitForSeconds(4); //注意等待时间的写法
        copy_die.Add(Instantiate(die, new Vector3(posx, posy + 1.5f, 0), new Quaternion(0, 0, 0, 0)) as GameObject);
    }
    IEnumerator die_warrior(float posx, float posy)
    {
        yield return new WaitForSeconds(5); //注意等待时间的写法
        copy_die.Add(Instantiate(die, new Vector3(posx, posy + 1.5f, 0), new Quaternion(0, 0, 0, 0)) as GameObject);
    }
    IEnumerator die_magician(float posx, float posy)
    {
        yield return new WaitForSeconds(6); //注意等待时间的写法
        copy_die.Add(Instantiate(die, new Vector3(posx, posy + 1.5f, 0), new Quaternion(0, 0, 0, 0)) as GameObject);
    }
    



    IEnumerator p1_monk_move_fun(float posx, float posy)
    {
        yield return new WaitForSeconds(2); //注意等待时间的写法
        if (p1_monk_move != p1_monk_put_last)
        {
            p1_monk[p1_monk_move - 1].transform.position = new Vector3(posx + 0.5f, posy + 1, 0);
            yield return new WaitForSeconds(0.2f);
            p1_monk[p1_monk_move - 1].transform.position = new Vector3(posx, posy + 1, 0);
            p1_monk_move--;
        }
    }
    IEnumerator p2_monk_move_fun(float posx, float posy)
    {
        yield return new WaitForSeconds(2); //注意等待时间的写法
        if (p2_monk_move != p2_monk_put_last)
        {
            p2_monk[p2_monk_move - 1].transform.position = new Vector3(posx - 0.5f, posy + 1, 0);
            yield return new WaitForSeconds(0.2f);
            p2_monk[p2_monk_move - 1].transform.position = new Vector3(posx, posy + 1, 0);
            p2_monk_move--;
        }
    }
    IEnumerator p1_knight_move_fun(float posx, float posy)
    {
        yield return new WaitForSeconds(3); //注意等待时间的写法
        if (p1_knight_move != p1_knight_put_last)
        {
            p1_knight[p1_knight_move - 1].transform.position = new Vector3(posx + 0.5f, posy + 1, 0);
            yield return new WaitForSeconds(0.2f);
            p1_knight[p1_knight_move - 1].transform.position = new Vector3(posx, posy + 1, 0);
            p1_knight_move--;
        }
    }
    IEnumerator p2_knight_move_fun(float posx, float posy)
    {
        yield return new WaitForSeconds(3); //注意等待时间的写法
        if (p2_knight_move != p2_knight_put_last)
        {
            p2_knight[p2_knight_move - 1].transform.position = new Vector3(posx - 0.5f, posy + 1, 0);
            yield return new WaitForSeconds(0.2f);
            p2_knight[p2_knight_move - 1].transform.position = new Vector3(posx, posy + 1, 0);
            p2_knight_move--;
        }
    }
    IEnumerator p1_archer_move_fun(float posx, float posy)
    {
        yield return new WaitForSeconds(4); //注意等待时间的写法
        if (p1_archer_move != p1_archer_put_last)
        {
            p1_archer[p1_archer_move - 1].transform.position = new Vector3(posx + 0.5f, posy + 1, 0);
            yield return new WaitForSeconds(0.2f);
            p1_archer[p1_archer_move - 1].transform.position = new Vector3(posx, posy + 1, 0);
            p1_archer_move--;
        }
    }
    IEnumerator p2_archer_move_fun(float posx, float posy)
    {
        yield return new WaitForSeconds(4); //注意等待时间的写法
        if (p2_archer_move != p2_archer_put_last)
        {
            p2_archer[p2_archer_move - 1].transform.position = new Vector3(posx - 0.5f, posy + 1, 0);
            yield return new WaitForSeconds(0.2f);
            p2_archer[p2_archer_move - 1].transform.position = new Vector3(posx, posy + 1, 0);
            p2_archer_move--;
        }
    }
    IEnumerator p1_warrior_move_fun(float posx, float posy)
    {
        yield return new WaitForSeconds(5); //注意等待时间的写法
        if (p1_warrior_move != p1_warrior_put_last)
        {
            p1_warrior[p1_warrior_move - 1].transform.position = new Vector3(posx + 0.5f, posy + 1, 0);
            yield return new WaitForSeconds(0.2f);
            p1_warrior[p1_warrior_move - 1].transform.position = new Vector3(posx, posy + 1, 0);
            p1_warrior_move--;
        }
    }
    IEnumerator p2_warrior_move_fun(float posx, float posy)
    {
        yield return new WaitForSeconds(5); //注意等待时间的写法
        if (p2_warrior_move != p2_warrior_put_last)
        {
            p2_warrior[p2_warrior_move - 1].transform.position = new Vector3(posx - 0.5f, posy + 1, 0);
            yield return new WaitForSeconds(0.2f);
            p2_warrior[p2_warrior_move - 1].transform.position = new Vector3(posx, posy + 1, 0);
            p2_warrior_move--;
        }
    }
    IEnumerator p1_magician_move_fun(float posx, float posy)
    {
        yield return new WaitForSeconds(6); //注意等待时间的写法
        if (p1_magician_move != p1_magician_put_last)
        {
            p1_magician[p1_magician_move - 1].transform.position = new Vector3(posx + 0.5f, posy + 1, 0);
            yield return new WaitForSeconds(0.2f);
            p1_magician[p1_magician_move - 1].transform.position = new Vector3(posx, posy + 1, 0);
            p1_magician_move--;
        }
    }
    IEnumerator p2_magician_move_fun(float posx, float posy)
    {
        yield return new WaitForSeconds(6); //注意等待时间的写法
        if (p2_magician_move != p2_magician_put_last)
        {
            p2_magician[p2_magician_move - 1].transform.position = new Vector3(posx - 0.5f, posy + 1, 0);
            yield return new WaitForSeconds(0.2f);
            p2_magician[p2_magician_move - 1].transform.position = new Vector3(posx, posy + 1, 0);
            p2_magician_move--;
        }
    }
    IEnumerator monk_countdown(float posx, float posy)
    {
        yield return new WaitForSeconds(2); //注意等待时间的写法
        monk_audio.Play();
        copy_monk_attack.Add(Instantiate(p1_attack_monk, new Vector3(posx, posy, 0), new Quaternion(0, 0, 0, 0)) as GameObject);
    }
    IEnumerator knight_countdown(float posx, float posy)
    {
        yield return new WaitForSeconds(3); //注意等待时间的写法
        knight_audio.Play();
        copy_knight_attack.Add(Instantiate(p1_attack_knight, new Vector3(posx, posy, 0), new Quaternion(0, 0, 0, 0)) as GameObject);
    }

    IEnumerator archer_countdown(float posx, float posy, List<List<string>> left_archer, List<List<string>> right_archer, List<string> left_knight_defend, List<string> right_knight_defend, List<string> same_archer_attack, List<string> word)
    {
        yield return new WaitForSeconds(4); //注意等待时间的写法
        archer_audio.Play();
        copy_archer_attack.Add(Instantiate(p1_attack_archer, new Vector3(posx, posy, 0), new Quaternion(0, 0, 0, 0)) as GameObject);


        //將結果顯示在畫面上

        //佔領領土計算
        char[] left_archer_arr;

        for (int i = 0; i < left_archer.Count; i++)
        {
            for (int j = 3; j < left_archer[i].Count; j++)
            {

                //燒到騎士的防守領域
                if (right_knight_defend.Contains(left_archer[i][j]))
                {
                    continue;
                }



                left_archer_arr = left_archer[i][j].ToCharArray();


                int row = word.IndexOf(left_archer_arr[0].ToString());
                int col = int.Parse(left_archer_arr[1].ToString()) - 1;


                checker[row][col].GetComponent<Image>().color = Color.red;



            }
        }


        char[] right_archer_arr;

        for (int i = 0; i < right_archer.Count; i++)
        {
            for (int j = 3; j < right_archer[i].Count; j++)
            {

                //砍到騎士的防守領域

                if (left_knight_defend.Contains(right_archer[i][j]))
                {
                    continue;
                }



                right_archer_arr = right_archer[i][j].ToCharArray();

                int row = word.IndexOf(right_archer_arr[0].ToString());
                int col = int.Parse(right_archer_arr[1].ToString()) - 1;


                checker[row][col].GetComponent<Image>().color = Color.blue;

            }
        }


        char[] same_archer_arr;

        //把剛剛重複攻擊的領土叫出來

        for (int i = 0; i < same_archer_attack.Count - 1; i++)
        {
            if (same_archer_attack[i + 1] == "red")
            {
                same_archer_arr = same_archer_attack[i].ToCharArray();


                int row = word.IndexOf(same_archer_arr[0].ToString());
                int col = int.Parse(same_archer_arr[1].ToString()) - 1;

                checker[row][col].GetComponent<Image>().color = Color.red;
            }

            if (same_archer_attack[i + 1] == "blue")
            {
                same_archer_arr = same_archer_attack[i].ToCharArray();


                int row = word.IndexOf(same_archer_arr[0].ToString());
                int col = int.Parse(same_archer_arr[1].ToString()) - 1;

                checker[row][col].GetComponent<Image>().color = Color.blue;
            }


        }

    }



    IEnumerator warrior_countdown(float posx, float posy, List<List<string>> left_warrior, List<List<string>> right_warrior, List<string> left_knight_defend, List<string> right_knight_defend, List<string> same_warrior_attack, List<string> word)
    {
        yield return new WaitForSeconds(5); //注意等待时间的写法
        warrior_audio.Play();
        copy_warrior_attack.Add(Instantiate(p1_attack_warrior, new Vector3(posx, posy + 0.3f, 0), new Quaternion(0, 0, 0, 0)) as GameObject);


        //佔領領土計算
        char[] left_warrior_arr;

        for (int i = 0; i < left_warrior.Count; i++)
        {
            for (int j = 3; j < left_warrior[i].Count; j++)
            {

                //燒到騎士的防守領域
                if (right_knight_defend.Contains(left_warrior[i][j]))
                {
                    continue;
                }



                left_warrior_arr = left_warrior[i][j].ToCharArray();


                int row = word.IndexOf(left_warrior_arr[0].ToString());
                int col = int.Parse(left_warrior_arr[1].ToString()) - 1;


                checker[row][col].GetComponent<Image>().color = Color.red;



            }
        }


        char[] right_warrior_arr;

        for (int i = 0; i < right_warrior.Count; i++)
        {
            for (int j = 3; j < right_warrior[i].Count; j++)
            {

                //砍到騎士的防守領域

                if (left_knight_defend.Contains(right_warrior[i][j]))
                {
                    continue;
                }



                right_warrior_arr = right_warrior[i][j].ToCharArray();

                int row = word.IndexOf(right_warrior_arr[0].ToString());
                int col = int.Parse(right_warrior_arr[1].ToString()) - 1;


                checker[row][col].GetComponent<Image>().color = Color.blue;

            }
        }


        char[] same_warrior_arr;

        //把剛剛重複攻擊的領土叫出來

        for (int i = 0; i < same_warrior_attack.Count - 1; i++)
        {
            if (same_warrior_attack[i + 1] == "red")
            {
                same_warrior_arr = same_warrior_attack[i].ToCharArray();


                int row = word.IndexOf(same_warrior_arr[0].ToString());
                int col = int.Parse(same_warrior_arr[1].ToString()) - 1;

                checker[row][col].GetComponent<Image>().color = Color.red;
            }

            if (same_warrior_attack[i + 1] == "blue")
            {
                same_warrior_arr = same_warrior_attack[i].ToCharArray();


                int row = word.IndexOf(same_warrior_arr[0].ToString());
                int col = int.Parse(same_warrior_arr[1].ToString()) - 1;

                checker[row][col].GetComponent<Image>().color = Color.blue;
            }


        }



    }

    IEnumerator magician_countdown(float posx, float posy, List<List<string>> left_magician, List<List<string>> right_magician, List<string> left_knight_defend, List<string> right_knight_defend, List<string> same_magician_attack, List<string> word)
    {
        yield return new WaitForSeconds(6); //注意等待时间的写法
        magician_audio.Play();
        copy_magician_attack.Add(Instantiate(p1_attack_magician, new Vector3(posx, posy + 0.2f, 0), new Quaternion(0, 0, 0, 0)) as GameObject);


        //將結果顯示在畫面上

        //佔領領土計算
        char[] left_magician_arr;

        for (int i = 0; i < left_magician.Count; i++)
        {
            for (int j = 3; j < left_magician[i].Count; j++)
            {

                //燒到騎士的防守領域
                if (right_knight_defend.Contains(left_magician[i][j]))
                {
                    continue;
                }



                left_magician_arr = left_magician[i][j].ToCharArray();


                int row = word.IndexOf(left_magician_arr[0].ToString());
                int col = int.Parse(left_magician_arr[1].ToString()) - 1;


                checker[row][col].GetComponent<Image>().color = Color.red;



            }
        }


        char[] right_magician_arr;

        for (int i = 0; i < right_magician.Count; i++)
        {
            for (int j = 3; j < right_magician[i].Count; j++)
            {

                //砍到騎士的防守領域

                if (left_knight_defend.Contains(right_magician[i][j]))
                {
                    continue;
                }



                right_magician_arr = right_magician[i][j].ToCharArray();

                int row = word.IndexOf(right_magician_arr[0].ToString());
                int col = int.Parse(right_magician_arr[1].ToString()) - 1;


                checker[row][col].GetComponent<Image>().color = Color.blue;

            }
        }

        char[] same_magician_arr;

        //把剛剛重複攻擊的領土叫出來

        for (int i = 0; i < same_magician_attack.Count - 1; i++)
        {
            if (same_magician_attack[i + 1] == "red")
            {
                same_magician_arr = same_magician_attack[i].ToCharArray();


                int row = word.IndexOf(same_magician_arr[0].ToString());
                int col = int.Parse(same_magician_arr[1].ToString()) - 1;

                checker[row][col].GetComponent<Image>().color = Color.red;
            }

            if (same_magician_attack[i + 1] == "blue")
            {
                same_magician_arr = same_magician_attack[i].ToCharArray();


                int row = word.IndexOf(same_magician_arr[0].ToString());
                int col = int.Parse(same_magician_arr[1].ToString()) - 1;

                checker[row][col].GetComponent<Image>().color = Color.blue;
            }


        }





    }

    IEnumerator battle_end_count_down()
    {
        yield return new WaitForSeconds(7); //注意等待时间的写法

        /*  回合結束  */


        //顯示用
        int red = 0;
        int blue = 0;


        //先把目前棋盤顏色記起來 之後要用來戰鬥 恢復原本顏色用的
        for (int i = 0; i < checker.Count; i++)
        {
            for (int j = 0; j < checker[i].Count; j++)
            {
                if (checker[i][j].GetComponent<Image>().color == Color.red)
                {
                    color_pos[i][j] = "red";
                    red++;
                }
                else
                {
                    color_pos[i][j] = "blue";
                    blue++;
                }
            }
        }


        red_territory.text = red.ToString();
        blue_territory.text = blue.ToString();


        battle_end = true;
    }


    IEnumerator win_lose_interface_count_down()
    {
        yield return new WaitForSeconds(10);


        win_picture.SetActive(true);
        win_picture.SetActive(true);


        battle_end_notice.SetActive(false);

        //換場景 換回island
        Change_Screen();

    }


}
