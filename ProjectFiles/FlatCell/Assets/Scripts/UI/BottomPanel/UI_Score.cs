using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utils.UIManager;

public class UI_Score : MonoBehaviour
{
    [SerializeField] GameObject Manager;
    [SerializeField] TextMeshProUGUI scoreValue;
    [SerializeField] Image scoreBackground;

    UIManager manager;

    float score = 0f;
    Color playerColor;

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

        if (manager != null)
        {
            score = manager.controller.geo.GetScore();
            playerColor = manager.controller.geo.GetColor();
        }

        UpdateText();
    }

    void UpdateText()
    {
        scoreValue.text = score.ToString();
        scoreBackground.color = new Color(playerColor.r * 2, playerColor.g * 2, playerColor.b * 2, 64f / 255f);
    }

}
