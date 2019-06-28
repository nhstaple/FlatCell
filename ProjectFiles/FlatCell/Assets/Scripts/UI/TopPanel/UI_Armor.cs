using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Utils.UIManager;

public class UI_Armor : MonoBehaviour
{
    [SerializeField] GameObject Manager;
    [SerializeField] TextMeshProUGUI armorText;
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
        armorText.text = "Armor: " + (Mathf.Round(manager.controller.geo.GetArmor())).ToString() + "%";
    }
}
