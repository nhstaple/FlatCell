using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    private string Start = "Game";
    private string Controls = "Control";
    private string About = "Abeut";

    public GameObject StartScreen;
    // Update is called once per frame
    public void StarMenu()
    {
        SceneManager.LoadScene(Start);
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
