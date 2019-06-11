using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePause = false;

    private string Scene = "Title";

    public GameObject PauseUI;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
        GamePause = false;
    }
    void Pause()
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
        GamePause = true;
    }

    public void StartMenu()
    {
        Debug.Log("Loading");
        SceneManager.LoadScene(Scene);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
