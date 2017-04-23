using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreBehaviour : MonoBehaviour
{
    private float highscore;
    private GameObject player;
    private TextMesh scoreText;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        scoreText = GetComponentInChildren<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHighscore();
    }

    void UpdateHighscore()
    {
        if (!player)
            return;

        highscore = Mathf.Max(highscore, player.transform.position.y + player.GetComponent<Collider2D>().bounds.extents.y);
        transform.position = new Vector2(transform.position.x, highscore);
        string scoreString = highscore.ToString("0.00") + " meter";
        scoreText.text = scoreString;

    }

    public float Highscore
    {
        get { return highscore; }
    }
}
