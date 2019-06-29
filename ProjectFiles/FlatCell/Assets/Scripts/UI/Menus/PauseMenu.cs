using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Utils.InputManager;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] TextMeshProUGUI[] text;
    [SerializeField] Button[] buttons;

    public static bool GamePause = false;

    private string Scene = "Title";

    public GameObject PauseUI;
    // Update is called once per frame

    GameObject player;

    InputManager inManager;

    const float pauseBackAlpha = 200f / 255;

    const float lerpTime = 1f;
    float fadeInCounter = 0f;
    float fadeOutCounter = 1f;
    float lastDelta = 0.01f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inManager = player.GetComponent<InputManager>();
        background.color = new Color(0, 0, 0, 0);
        foreach(TextMeshProUGUI t in text)
        {
            t.color = new Color(1, 1, 1, 0);
            t.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0);
        }
        foreach(Button b in buttons)
        {
            b.enabled = false;
            b.interactable = false;
        }
    }

    private void Awake()
    {
        foreach (TextMeshProUGUI t in text)
        {
            t.color = new Color(1, 1, 1, 0);
        }
    }

    void Update()
    {
        if(Time.deltaTime != 0)
        {
            lastDelta = Time.deltaTime;
        }

        if(inManager == null)
        {
            inManager = player.GetComponent<InputManager>();
        }

        if(inManager != null)
        {
            var res = inManager.GetPause();
            if (inManager.GetPause())
            {
                if (GamePause)
                {
                    Debug.Log("Resume");
                    Resume();
                }
                else
                {
                    Debug.Log("Pause");
                    Pause();
                }
            }
        }

        // Fade in the pause menu
        if(GamePause)
        {
            fadeOutCounter = 0f;

            float alpha = Mathf.SmoothStep(0, pauseBackAlpha, fadeInCounter / lerpTime);
            float beta = Mathf.SmoothStep(0, 1, fadeInCounter / lerpTime);

            background.color = new Color(0, 0, 0, alpha);

            foreach (TextMeshProUGUI t in text)
            {
                t.color = new Color(1, 1, 1, beta);
            }

            fadeInCounter += lastDelta;
        }
        // Fade out the pause menu
        else
        {
            fadeInCounter = 0f;

            float alpha = Mathf.SmoothStep(pauseBackAlpha, 0, fadeOutCounter / lerpTime);
            float beta = Mathf.SmoothStep(1, 0, fadeOutCounter / lerpTime);

            background.color = new Color(0, 0, 0, alpha);

            foreach (TextMeshProUGUI t in text)
            {
                t.color = new Color(1, 1, 1, beta);
            }

            fadeOutCounter += lastDelta;
        }
    }

    public void Resume()
    {
        foreach (Button b in buttons)
        {
            //b.enabled = false;
            b.interactable = false;
        }
        Time.timeScale = 1f;
        GamePause = false;
    }

    void Pause()
    {
        Time.timeScale = 0f;
        GamePause = true;
        foreach (Button b in buttons)
        {
            //b.enabled = true;
            b.interactable = true;
        }
    }

    public void StartMenu()
    {
        Debug.Log("Loading");
        Time.timeScale = 1f;
        GamePause = false;
        SceneManager.LoadScene(Scene);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
