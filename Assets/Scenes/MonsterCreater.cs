using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCreater : MonoBehaviour
{
    private Ray mouseRay1;    // 宣告一條射線
    private RaycastHit rayHit;  //RaycastHit是用來儲存被射線所打到的位置
    public float posX, posY;  //用來接收滑鼠點擊座標的X、Y值
    public GameObject Monster;
    public Text Text;
    // Start is called before the first frame update
    public void CreatMonster(float x, float y)
    {
        Instantiate(Monster, new Vector3(x, y, 0), Quaternion.identity);
    }
    void Start()
    {
    }

    private void GetPos()
    {
        // 設定射線的行徑方向(螢幕-滑鼠點擊位置)
        mouseRay1 = Camera.main.ScreenPointToRay(Input.mousePosition);
        //如果射線以MouseRay1方向前進(螢幕到滑鼠點擊座標),有打到collider就會執行大括弧裡的程式碼
        if (Physics.Raycast(mouseRay1, out rayHit, 1000f))
        {
            //儲存滑鼠所點擊的座標
            posX = rayHit.point.x;
            posY = rayHit.point.y;
            Text.text = posX.ToString();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            GetPos();
            CreatMonster(posX,posY);
        }
        else
        {
        }
    }
}
