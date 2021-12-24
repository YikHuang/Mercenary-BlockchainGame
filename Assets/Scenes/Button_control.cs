using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;


public class Button_control : NetworkBehaviour
{
    public GameObject player;
    public Text ID_text;
    public Button Human;
    public Button Dragon;
    public Button GOD;
    public Button Enter;
    
    void Start()
    {        
        //設定按鈕
        if (isLocalPlayer)
        {
            Dragon = GameObject.Find("Dragon").GetComponent<Button>();
            Human = GameObject.Find("Human").GetComponent<Button>();
            GOD = GameObject.Find("God").GetComponent<Button>();
            Enter = GameObject.Find("Enter").GetComponent<Button>();

            Dragon.onClick.AddListener(() => Choose_Dragon());
            Human.onClick.AddListener(() => Choose_Human());
            GOD.onClick.AddListener(() => Choose_GOD());
            Enter.onClick.AddListener(() => Change_scene());

            GameObject.Find("image_human").GetComponent<SpriteRenderer>().enabled = true;
            GameObject.Find("image_dragon").GetComponent<SpriteRenderer>().enabled = true;
            GameObject.Find("image_god").GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    //把ID顯示在畫面上
    public void SetIDText(int id)
    {
        if (isLocalPlayer)
        {
            ID_text = GameObject.Find("ID").GetComponent<Text>();
            ID_text.text = "Player : " + id.ToString();
        }
    }
    
    //選擇種族的按鍵處理事件
    public void Choose_Dragon()
    {
        if (isLocalPlayer)
        {
            //呼叫Player的Cmd方法 以設定種族
            GameObject.Find("image_dragon").GetComponent<Transform>().position = new Vector3(-5, 0, 0);
            GameObject.Find("image_human").GetComponent<Transform>().position = new Vector3(50, 50, 0);
            GameObject.Find("image_god").GetComponent<Transform>().position = new Vector3(50, 50, 0);
            player.GetComponent<Player>().CmdChoose_Dragon();
        }
    }
    
    public void Choose_Human()
    {
        if (isLocalPlayer)
        {
            GameObject.Find("image_human").GetComponent<Transform>().position = new Vector3(-5, 0, 0);
            GameObject.Find("image_god").GetComponent<Transform>().position = new Vector3(50, 50, 0);
            GameObject.Find("image_dragon").GetComponent<Transform>().position = new Vector3(50, 50, 0);
            player.GetComponent<Player>().CmdChoose_Human();
        }
    }
    
    public void Choose_GOD()
    {
        if (isLocalPlayer)
        {
            GameObject.Find("image_god").GetComponent<Transform>().position = new Vector3(-5, 0, 0);
            GameObject.Find("image_human").GetComponent<Transform>().position = new Vector3(50, 50, 0);
            GameObject.Find("image_dragon").GetComponent<Transform>().position = new Vector3(50, 50, 0);
            player.GetComponent<Player>().CmdChoose_GOD();
        }
    }
    
    //切換場景
    public void Change_scene()
    {
        if (isLocalPlayer)
        {
            SceneManager.LoadScene("island");
        }
    }
}
