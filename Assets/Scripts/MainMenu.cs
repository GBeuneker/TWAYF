using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (SceneLoader.Instance.restart)
        {
            //gameObject.SetActive(false);
            SceneLoader.Instance.StartGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") && SceneLoader.Instance.isPaused)
            SceneLoader.Instance.ResumeGame();
    }

    public void StartGame()
    {
        SceneLoader.Instance.StartGame();
    }

    public void ResumeGame()
    {
        SceneLoader.Instance.ResumeGame();
    }

    public void RestartGame()
    {
        SceneLoader.Instance.RestartGame();
    }

    public void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }

    public void MuteMusic()
    {
        AudioManager.ToggleSoundMute(AudioManager.Channel.Music);
    }
}
