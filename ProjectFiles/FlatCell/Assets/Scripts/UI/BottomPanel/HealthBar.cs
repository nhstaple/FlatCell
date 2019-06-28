using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utils.UIManager;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI hpLives;
    [SerializeField] TextMeshProUGUI hpMaxLives;
    [SerializeField] TextMeshProUGUI hpMaxLivesText;
    [SerializeField] Image hpBar;
    [SerializeField] Image hpBackground;

    UIManager manager;

    [SerializeField] float MIN_HEART_SHIELD_COLOR;
    [SerializeField] float barLerpTime = 0.25f;

    float hp = 0f;
    float hpPercent = 0f;
    float oldHpPercent = 0f;
    float lerpCounter = 0f;
    bool hpChange = false;
    bool hpChangeIncrease = false;
    float barLerpCounter = 0f;
    float maxHp = 0f;

    void GetManager()
    {
        if(manager == null)
        {
            manager = UI.GetComponent<UIManager>();
        }
    }

    float ComputeHPPercent(float h)
    {
        while(h - 1 > 0)
        {
            h--;
        }
        if(h < 0)
        {
            h++;
        }
        return h;
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
            var newHp = manager.controller.geo.GetHealth();
            var newMax = manager.controller.geo.GetMaxHealth();

            if (maxHp != newMax)
            {
                if (newMax > maxHp)
                {
                    hpChangeIncrease = true;
                }
                else
                {
                    hpChangeIncrease = false;
                }
                maxHp = manager.controller.geo.GetMaxHealth();
            }

            if (hp != newHp)
            {
                hp = newHp;
                oldHpPercent = hpPercent;
                hpPercent = ComputeHPPercent(hp);
                hpChange = true;
            }
            else
            {
                hpChange = false;
            }

            UpdateHPPercent();
            UpdateHPLives();
            UpateHealthBar();
        }
    }

    float shieldTime = 0;
    float lastLerp = 0f;

    void UpdateHPPercent()
    {
        if (manager.controller.geo.GetShield().active)
        {
            if (lerpCounter != 0)
            {
                lastLerp = lerpCounter;
            }

            lerpCounter = 0f;

            hpText.text = "HP    " + Mathf.Round(hpPercent * 100) + "%\n";
            hpBar.color = Color.Lerp(Color.red + new Color(0, 0, 80 / 255f), Color.blue + new Color(0, 80 / 255f, 0), 1 + 1.00f*MIN_HEART_SHIELD_COLOR + -1 * manager.controller.geo.GetShield().GetPercent());
            hpBackground.color = Color.Lerp(new Color(1, 0, 0, 64f / 255f), new Color(0, 0, 1, 64f / 255f), 1 + 0.25f*MIN_HEART_SHIELD_COLOR + - 1 * manager.controller.geo.GetShield().GetPercent());

            float alpha = Mathf.SmoothStep(128f / 255f, 0, shieldTime/lastLerp);
            float beta = Mathf.SmoothStep(16f / 255f, 0, shieldTime/lastLerp);

            hpLives.color = new Color(hpLives.color.r, hpLives.color.g, hpLives.color.b, alpha);
            hpMaxLives.color = new Color(hpMaxLives.color.r, hpMaxLives.color.g, hpMaxLives.color.b, beta);
            hpMaxLivesText.color = new Color(hpMaxLivesText.color.r, hpMaxLivesText.color.g, hpMaxLivesText.color.b, beta);

            shieldTime += Time.deltaTime;
        }
        else
        {
            shieldTime = 0f;

            hpText.text = "HP    " + Mathf.Round(hpPercent * 100) + "%\n";

            float alpha = Mathf.SmoothStep(hpLives.color.a, 128f / 255f, lerpCounter / manager.controller.geo.GetShield().GetMaxDuration());
            float beta = Mathf.SmoothStep(hpMaxLivesText.color.a, 16f / 255f, lerpCounter / manager.controller.geo.GetShield().GetMaxDuration());

            hpBar.color = Color.red + new Color(0, 0, 80 / 255f);
            hpBackground.color = new Color(1, 0, 0, 64f / 255f);

            hpLives.color = new Color(hpLives.color.r, hpLives.color.g, hpLives.color.b, alpha);
            hpMaxLives.color = new Color(hpMaxLives.color.r, hpMaxLives.color.g, hpMaxLives.color.b, alpha);
            hpMaxLivesText.color = new Color(hpMaxLivesText.color.r, hpMaxLivesText.color.g, hpMaxLivesText.color.b, beta);

            lerpCounter += Time.deltaTime;
        }
    }

    void UpdateHPLives()
    {
        hpLives.text = "Lives " + Mathf.Floor(manager.controller.geo.GetHealth());
        hpMaxLives.text = Mathf.Ceil(manager.controller.geo.GetMaxHealth()).ToString();
    }

    void UpateHealthBar()
    {
        if(hpChange)
        {
            barLerpCounter = 0f;
        }

        if(hpBar.fillAmount != hpPercent)
        {
            float fill = Mathf.SmoothStep(oldHpPercent, hpPercent, barLerpCounter / barLerpTime);
            hpBar.fillAmount = fill;
            barLerpCounter += Time.deltaTime;
        }
        else
        {
            barLerpCounter = 0f;
        }
    }
}
