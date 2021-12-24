using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutotial : MonoBehaviour
{

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("person");
    }
}