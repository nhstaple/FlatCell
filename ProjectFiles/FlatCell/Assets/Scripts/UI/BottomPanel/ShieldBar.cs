using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utils.UIManager;

public class ShieldBar : MonoBehaviour
{
    [SerializeField] GameObject Manager;
    [SerializeField] TextMeshProUGUI shieldText;
    [SerializeField] TextMeshProUGUI shieldTime;
    [SerializeField] Image shieldBar;
    [SerializeField] Image shieldBackground;

    UIManager manager;

    float shieldEnergy = 0f;
    float shieldMaxTime = 0f;
    float flashCounter = 0f;
    [SerializeField] float shieldRechargeFlashPoll = 0.25f;
    bool shieldFlash = false;
    float colorCounter = 0f;
    [SerializeField] float minAlpha = 0.40f;

    float restoreLerpTime = 0.5f;

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
            shieldEnergy = manager.controller.geo.GetShield().GetPercent();
            shieldMaxTime = manager.controller.geo.GetShield().GetMaxDuration();
        }

        UpdateText();
        UpdateBar();
    }

    float restoreCounter = 0;

    void UpdateText()
    {
        if (manager.controller.geo.GetShield().charging)
        {
            flashCounter += Time.deltaTime;
            restoreCounter = 0f;
            if (flashCounter >= shieldRechargeFlashPoll)
            {
                flashCounter = 0f;
                if (shieldFlash)
                {
                    shieldFlash = false;
                    colorCounter = 0f;
                }
                else
                {
                    shieldFlash = true;
                    colorCounter = 0f;
                }
            }
            if(shieldFlash)
            {
                shieldBar.color = Color.Lerp(new Color(0, 80f / 255, 1, 1), new Color(0, 80f / 255, 1, minAlpha), colorCounter / (2f * shieldRechargeFlashPoll));
                colorCounter += Time.deltaTime;
            }
            else
            {
                shieldBar.color = Color.Lerp(new Color(0, 80f / 255, 1, minAlpha), new Color(0, 80f / 255, 1, 1), colorCounter / (2f * shieldRechargeFlashPoll));
                colorCounter += Time.deltaTime;
            }
            shieldText.text = "Shield Charging";
            shieldTime.color = new Color(shieldTime.color.r, shieldTime.color.g, shieldTime.color.b, 0);
        }
        else
        {
            flashCounter = shieldRechargeFlashPoll;
            shieldBar.color = new Color(0, 80f / 255, 1, 1);
            shieldText.text = "Shield " + (Mathf.Round(100f * shieldEnergy)).ToString() + "%";
            shieldTime.text = "Time   " + Mathf.Round(shieldMaxTime) + "s";
            shieldTime.color = Color.Lerp(new Color(shieldTime.color.r, shieldTime.color.g, shieldTime.color.b, 0), new Color(shieldTime.color.r, shieldTime.color.g, shieldTime.color.b, 128f / 255), restoreCounter / restoreLerpTime);
            restoreCounter += Time.deltaTime;
        }
    }

    void UpdateBar()
    {
        shieldBar.fillAmount = shieldEnergy;
    }
}
