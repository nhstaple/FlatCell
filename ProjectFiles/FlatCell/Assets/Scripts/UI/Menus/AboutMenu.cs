using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AboutMenu : MonoBehaviour
{
    [SerializeField] RawImage[] images;
    [SerializeField] TextMeshProUGUI[] textBoxes;
    [SerializeField] float fadeInLerpTime = 0.75f;
    [SerializeField] Image background;

    float fadeInCounter;

    // Start is called before the first frame update
    void Start()
    {
        foreach (RawImage img in images)
        {
            img.color = new Color(255, 255, 255, 0);
        }

        foreach (TextMeshProUGUI text in textBoxes)
        {
            text.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -1f);
        }
        if (background != null)
        {
            background.color = new Color(1, 1, 1, 1);
        }
        fadeInCounter = 0f;
    }

    private void Awake()
    {
        fadeInCounter = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = Mathf.SmoothStep(-1f, 0f, fadeInCounter / fadeInLerpTime);
        Color newColor = Color.Lerp(new Color(1, 1, 1, 1), new Color(0, 0, 0, 200f / 255), fadeInCounter / fadeInLerpTime);

        foreach (RawImage img in images)
        {
            img.color = new Color(255, 255, 255, alpha + 1);
        }

        foreach (TextMeshProUGUI text in textBoxes)
        {
            text.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, alpha);
        }

        if (background != null)
        {
            background.color = newColor;
        }

        fadeInCounter += Time.deltaTime;
    }

    private string Scene = "Title";
    public void StartMenu()
    {
        Debug.Log("Loading");
        Time.timeScale = 1f;
        SceneManager.LoadScene(Scene);
    }
}
