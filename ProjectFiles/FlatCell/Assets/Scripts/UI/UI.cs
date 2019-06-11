using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Spawner.Command;
using Geo.Command;

public class UI : MonoBehaviour 
{
    [SerializeField] public Image[] Numhearts;
    public Sprite YesHeart;
    public Sprite NoHeart;

    public float health;
    public float maxhealth;
    public float myscore;
    public float shieldperc;

    private float TimetoWatch = 0.5f;
    private float Throttle;

    GameObject player;
    public Text scoretext;

    //Nick's stuff for shield Fill
    IGeo p;
    Shield shieldCopy;
    public float fillPercent;
    float minHeartShieldPercent = 0.33f;
    public Image ShieldBar;
    Color ogColor;
    void GetPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }

    //public PlayerController p;
    // Start is called before the first frame update
    void Start()
    {
        //        score = GetComponent<Text>();
        if (player != null)
        {
            IGeo controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                health = controller.GetHealth();
                maxhealth = controller.GetMaxHealth();
                p = player.GetComponent<PlayerController>();
                if (p != null)
                {
                    shieldCopy = p.GetShield();
                    fillPercent = shieldCopy.GetPercent();
                    ShieldBar.fillAmount = fillPercent;
                }
            }
            else
            {
                health = maxhealth = 1;
            }
            ogColor = Color.white;
        }   
        //this.RandomizeThrottle();
    }
    //private void RandomizeThrottle()
    //{
    //   Throttle = Random.Range(0.4f, 0.5f);
   //}
    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            GetPlayer();
        }
        IGeo controller = player.GetComponent<PlayerController>();
        if(controller != null)
        {
            myscore = controller.GetScore();
        }
        //if (TimetoWatch >= Throttle)
        //{
            //player = GameObject.FindWithTag("Player");
            //PlayerController controller = player.GetComponent<PlayerController>();
        health = controller.GetHealth();
        maxhealth = controller.GetMaxHealth();

        if (controller != null)
        {
            fillPercent = controller.GetShield().GetPercent();
            Debug.Log(fillPercent);
            if (ShieldBar != null)
            {
                ShieldBar.fillAmount = fillPercent;
            }
        }
        //myscore = controller.killHistory["Dot"];
        //if (scoretext != null)
        //{
        //    scoretext.text = "Score: " + myscore.ToString();
        //}
        //Debug.Log(myscore);
        //}

        if (scoretext != null)
        {
            scoretext.text = "Score: " + myscore.ToString();
        }

        if (health > maxhealth)
        {
            health = maxhealth;
        }
        for (int i = 0; i < Numhearts.Length; i++)
        {
            //Debug.Log(health);
            //if (Numhearts[i] != null)
            //{
            if (player.GetComponent<PlayerController>().GetShield().active)
            {

                Numhearts[i].color = Color.Lerp(Color.white, Color.blue, player.GetComponent<PlayerController>().GetShield().GetPercent() + minHeartShieldPercent);
            }
            else
            {
                Numhearts[i].color = Color.white;
            }
            if (i < health)
                {
                    Numhearts[i].sprite = YesHeart;
                }
                else
                {
                    Numhearts[i].sprite = NoHeart;
                }

                if (i < maxhealth)
                {
                    Numhearts[i].enabled = true;
                }
                else
                {
                    Numhearts[i].enabled = false;
                }
            //} 
            //else
            //{
                // Debug.Log("UI Error!");
            //}
        }
    }
}