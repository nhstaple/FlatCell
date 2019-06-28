using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utils.UIManager;

public class BoostBar : MonoBehaviour
{
    [SerializeField] GameObject Manager;
    [SerializeField] TextMeshProUGUI boostText;
    [SerializeField] Image boostBar;
    [SerializeField] Image boostBackground;

    UIManager manager;

    float boostEnergy = 0f;
    float boostRechargePercent = 0f;

    float flashCounter = 0f;
    float boostRechargeFlashPoll = 0.25f;
    bool boostFlash = false;
    float colorCounter = 0f;

    void GetManager()
    {
        if(manager == null)
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

        if(manager != null)
        {
            boostEnergy = manager.controller.GetBoostPercent();
            boostRechargePercent = manager.controller.GetBoostRechargePercent();
        }

        UpdateText();
        UpdateBar();
    }

    void UpdateText()
    {
        if (manager.controller.IsBoostCharging())
        {
            boostText.text = "Boost Charging";
        }
        else
        {
            boostText.text = "Boost " + (Mathf.Round(100f * boostEnergy)).ToString() + "%";
        }
    }

    void UpdateBar()
    {
        if (manager.controller.IsBoostCharging())
        {
            
            flashCounter += Time.deltaTime;
            if (flashCounter >= boostRechargeFlashPoll)
            {
                flashCounter = 0f;
                if (boostFlash)
                {
                    boostFlash = false;
                    colorCounter = 0f;
                }
                else
                {
                    boostFlash = true;
                    colorCounter = 0f;
                }
            }

            if (boostFlash)
            {
                boostBar.color = Color.Lerp(new Color(0, 1, 80f / 255, 1), new Color(0, 1, 80f / 255, 0.25f), colorCounter / (2f * boostRechargeFlashPoll));
                colorCounter += Time.deltaTime;
            }
            else
            {
                boostBar.color = Color.Lerp(new Color(0, 1, 80f / 255, 0.25f), new Color(0, 1, 80f / 255, 1), colorCounter / (2f * boostRechargeFlashPoll));
                colorCounter += Time.deltaTime;
            }
            
            boostBar.fillAmount = boostRechargePercent;
        }
        else
        {
            flashCounter = boostRechargeFlashPoll;
            boostBar.color = new Color(0, 1, 80f / 255, 1);
            boostBar.fillAmount = boostEnergy;
        }
    }
}
