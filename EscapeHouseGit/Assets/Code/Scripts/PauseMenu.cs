using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject PauseMenuCanvas;
    public GameObject player;
    
    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
    }

    void Stop()
    {
        PauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f; // freeze time
        paused = true;
        player.GetComponent<FirstPersonController>().enabled = false;
        player.GetComponent<StarterAssetsInputs>().jump = false;
        player.GetComponent<StarterAssetsInputs>().jumpAudio.enabled = false;
    }
    public void Play()
    {
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        player.GetComponent<FirstPersonController>().enabled = true;
        player.GetComponent<StarterAssetsInputs>().jump = false;
        player.GetComponent<StarterAssetsInputs>().jumpAudio.enabled = true;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); 
    }
}
