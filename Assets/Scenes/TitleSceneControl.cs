using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class TitleSceneControl : MonoBehaviour
{
    public NetworkManagerHUD hud;

    // Start is called before the first frame update
    void Start()
    {
        hud = gameObject.GetComponent<NetworkManagerHUD>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "title")
        {
            hud.showGUI = true;
            hud.offsetX = Screen.width / 2 - 100;
            hud.offsetY = Screen.height - 375;
        }
        else
        {
            hud.showGUI = false;
            hud.offsetX = 0;
            hud.offsetY = 0;
        }
    }
}
