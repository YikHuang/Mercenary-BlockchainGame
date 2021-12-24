using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class network_control : MonoBehaviour
{

    static network_control instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            name = "network";
        }

        else if(instance != this)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            Debug.Log("刪除" + sceneName + "的" + name);
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
