
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Controller.Player;
using TMPro;
using UnityEngine.UI;

namespace Utils.UIManager
{

    public class UIManager : MonoBehaviour
    {
        GameObject player;
        public PlayerController controller;

        [SerializeField] Image[] panels;
        [SerializeField] Image[] meterBars;
        [SerializeField] Image[] meterBackgrounds;
        [SerializeField] TextMeshProUGUI[] textBoxTitles;
        [SerializeField] TextMeshProUGUI[] textBoxInfo;
        [SerializeField] TextMeshProUGUI[] textBoxBackground;

        [SerializeField] float panelFadeIn = 1f;
        float fadeInCounter = 0f;

        public GameObject GetPlayerObject()
        {
            if (player == null)
            {
                GetPlayer();
            }
            return player;
        }

        public PlayerController GetPlayerController()
        {
            if(controller == null)
            {
                GetPlayer();
            }
            return controller;
        }

        void GetPlayer()
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player");
            }
            if (controller == null)
            {
                controller = player.GetComponent<PlayerController>();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            GetPlayer();
            foreach(Image panel in panels)
            {
                panel.color = new Color(0,0,0,0);
            }
            foreach (Image bar in meterBars)
            {
                bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, 0);
            }
            foreach(Image back in meterBackgrounds)
            {
                back.color = new Color(back.color.r, back.color.g, back.color.b, 0);
            }

            foreach (TextMeshProUGUI panel in textBoxInfo)
            {
                panel.color = new Color(1, 1, 1, 0);
            }

            foreach (TextMeshProUGUI text in textBoxTitles)
            {
                text.color = new Color(1, 1, 1, 0);
            }
            foreach (TextMeshProUGUI text in textBoxBackground)
            {
                text.color = new Color(1, 1, 1, 0);
            }
        }

        // Update is called once per frame
        void Update()
        {
            GetPlayer();
            float alpha = Mathf.SmoothStep(0, 240f / 255, fadeInCounter / panelFadeIn);
            float beta = Mathf.SmoothStep(0, 128f / 255, fadeInCounter / panelFadeIn);
            float gamma = Mathf.SmoothStep(0, 64f / 255, fadeInCounter / panelFadeIn);
            float boop = Mathf.SmoothStep(0, 16f / 255, fadeInCounter / panelFadeIn);

            if (fadeInCounter <= panelFadeIn)
            {
                foreach (Image panel in panels)
                {
                    panel.color = new Color(0, 0, 0, alpha);
                }
                foreach (TextMeshProUGUI text in textBoxTitles)
                {
                    text.color = new Color(1, 1, 1, alpha);
                }
                foreach (Image bar in meterBars)
                {
                    bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, alpha);
                }
                foreach (Image back in meterBackgrounds)
                {
                    back.color = new Color(back.color.r, back.color.g, back.color.b, gamma);
                }
                foreach (TextMeshProUGUI panel in textBoxInfo)
                {
                    panel.color = new Color(1, 1, 1, beta);
                }
                foreach (TextMeshProUGUI text in textBoxBackground)
                {
                    text.color = new Color(1, 1, 1, boop);
                }
            }
            fadeInCounter += Time.deltaTime;
        }
    }
}