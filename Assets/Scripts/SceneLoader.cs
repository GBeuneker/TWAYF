using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public GameObject menuCanvas, scoreCanvas, pauseCanvas;
    public bool isPlaying = false, isPaused = false;
    public bool restart = false;

    // Use this for initialization
    void Start()
    {
        PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PlayMusic()
    {
        AudioManager.Play(Resources.Load<AudioClip>("Sounds/music"), AudioManager.Channel.Music, 1, false, true);
    }

    public void StartGame()
    {
        Destroy(GameObject.Find("Main Menu"));
        isPlaying = true;
        restart = false;
        PlayMusic();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        if (!pauseCanvas)
            pauseCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/Pause Menu"));
        else
            pauseCanvas.SetActive(isPaused);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        pauseCanvas.SetActive(isPaused);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
        isPlaying = true;
        isPaused = false;
        restart = true;
    }

    public void ShowScore()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Score Overlay"));
        isPlaying = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
