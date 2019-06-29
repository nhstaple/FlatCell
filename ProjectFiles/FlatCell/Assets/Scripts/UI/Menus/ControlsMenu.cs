using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class ControlsMenu : MonoBehaviour
{
    [SerializeField] RawImage[] images;
    [SerializeField] Image[] imgs;
    [SerializeField] TextMeshProUGUI[] textBoxes;
    [SerializeField] float fadeInLerpTime = 0.75f;

    float fadeInCounter;

    // Start is called before the first frame update
    void Start()
    {
        foreach(RawImage img in images)
        {
            img.color = new Color(255, 255, 255, 0);
        }
        foreach (Image img in imgs)
        {
            img.color = new Color(255, 255, 255, 0);
        }
        foreach (TextMeshProUGUI text in textBoxes)
        {
            text.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -1f);
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

        foreach (RawImage img in images)
        {
            img.color = new Color(255, 255, 255, alpha + 1);
        }

        foreach (TextMeshProUGUI text in textBoxes)
        {
            text.fontSharedMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, alpha);
        }
        foreach (Image img in imgs)
        {
            img.color = new Color(0, 0, 0, alpha + 1 - 0.25f);
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
