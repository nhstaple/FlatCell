using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Utils.UIManager;

public class UI_Speed : MonoBehaviour
{
    [SerializeField] GameObject Manager;
    [SerializeField] TextMeshProUGUI speedText;
    UIManager manager;

    void GetManager()
    {
        if (manager == null)
        {
            manager = Manager.GetComponent<UIManager>();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GetManager();
    }

    // Update is called once per frame
    void Update()
    {
        GetManager();

        UpdateText();
    }

    void UpdateText()
    {
        speedText.text = "Speed: " + (Mathf.Round(100f * manager.controller.geo.GetSpeedPercent())).ToString() + "%";
    }
}
