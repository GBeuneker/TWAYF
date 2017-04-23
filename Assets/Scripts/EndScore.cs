using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScore : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GetComponent<Text>().text = "Your Score:\n" + GameObject.Find("Highscore").GetComponent<HighscoreBehaviour>().Highscore.ToString("0.00") + " meter";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
