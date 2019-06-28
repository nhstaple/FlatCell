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

    [SerializeField] float fadeInLerpTime = 3f;
    [SerializeField] float menuDelay = 1f;
    float fadeInCounter;


    void Start()
    {
        title.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -1f);
        foreach(TextMeshProUGUI sub in menu)
        {
            sub.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -1f);
        }
        fadeInCounter = 0f;
    }

    private void Update()
    {
        float alpha = Mathf.SmoothStep(-1f, -.25f, fadeInCounter / fadeInLerpTime);
        float beta = Mathf.SmoothStep(-.8f, 0, (fadeInCounter - menuDelay) / fadeInLerpTime);

        title.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, alpha);

        foreach (TextMeshProUGUI sub in menu)
        {
            sub.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, beta);
        }

        fadeInCounter += Time.deltaTime;
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
