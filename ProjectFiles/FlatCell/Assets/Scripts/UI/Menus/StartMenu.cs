using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenu : MonoBehaviour
{
    private string start = "Game";
    private string Controls = "Control";
    private string About = "Abeut";

    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI[] menu;
    [SerializeField] TextMeshProUGUI[] textFade;

    [SerializeField] float fadeInLerpTime = 3f;
    [SerializeField] float menuDelay = 1f;
    static bool enableTitleAnim = true;
    static bool enableMenuAnim = true;
    static bool init = false;
    
    float fadeInCounter;


    void Start()
    {
        init = true;

        if (enableTitleAnim)
        {
            if (title != null)
            {
                title.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -1f);
            }
        }

        if (enableMenuAnim)
        {
            foreach (TextMeshProUGUI sub in menu)
            {
                sub.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -1f);
            }
        }

        foreach(TextMeshProUGUI txt in textFade)
        {
            txt.color = new Color(0, 0, 0, 0);
        }

        fadeInCounter = 0f;
    }

    void Awake()
    {
        if (enableTitleAnim)
        {
            if (title != null)
            {
                title.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -1f);
            }
        }
        fadeInCounter = 0f;
    }

    private void Update()
    {
        float alpha = Mathf.SmoothStep(-.8f, -.25f, fadeInCounter / fadeInLerpTime);
        float beta = Mathf.SmoothStep(-.775f, 0, (fadeInCounter - menuDelay) / fadeInLerpTime);
        float gamma = Mathf.SmoothStep(0, 0.5f, (fadeInCounter - menuDelay) / fadeInLerpTime);

        foreach (TextMeshProUGUI txt in textFade)
        {
            txt.color = new Color(0, 0, 0, gamma);
        }

        if (enableTitleAnim && enableMenuAnim)
        {
            if (title != null)
            {
                title.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, alpha);
            }
        }

        if (enableMenuAnim)
        {
            foreach (TextMeshProUGUI sub in menu)
            {
                sub.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, beta);
            }
        }
        else
        {
            float betaPrime = Mathf.SmoothStep(-.8f, 0, (fadeInCounter) / (0.25f * fadeInLerpTime));

            foreach (TextMeshProUGUI sub in menu)
            {
                sub.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, betaPrime);
            }

            if (enableTitleAnim)
            {
                float alphaPrime = Mathf.SmoothStep(-1f, -.25f, fadeInCounter / (0.25f * fadeInLerpTime));
                if (title != null)
                {
                    title.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, alphaPrime);
                }
            }
        }

        fadeInCounter += Time.deltaTime;

        if (beta == 0)
        {
            enableMenuAnim = false;
        }
    }

    public GameObject StartScreen;
    // Update is called once per frame
    public void StartGame()
    {
        SceneManager.LoadScene(start);
    }
    public void Control()
    {
        SceneManager.LoadScene(Controls);
    }
    public void Abbout()
    {
        SceneManager.LoadScene(About);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
