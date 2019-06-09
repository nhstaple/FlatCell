using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Spawner.Command;
using Geo.Command;

public class UI : MonoBehaviour //, IGeo
{
    public Image[] Numhearts;
    public Sprite YesHeart;
    public Sprite NoHeart;

    public float health;
    public float maxhealth;
    public float myscore;

    private float TimetoWatch = 0.5f;
    private float Throttle;

    GameObject player;
    GameObject Doty;

    public Text scoretext;

    void GetPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }

    //public PlayerController p;
    // Start is called before the first frame update
    void Awake()
    {
        //        score = GetComponent<Text>();
        player = GameObject.FindWithTag("Player");
        if(player != null)
        {
            IGeo controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                health = controller.GetHealth();
                maxhealth = controller.GetMaxHealth();
            }
            else
            {
                health = maxhealth = 1;
            }
        }
        this.RandomizeThrottle();
    }
    private void RandomizeThrottle()
    {
        Throttle = Random.Range(0.4f, 0.5f);
    }
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
        if (TimetoWatch >= Throttle)
        {
            //player = GameObject.FindWithTag("Player");
            //PlayerController controller = player.GetComponent<PlayerController>();
            health = controller.GetHealth();
            maxhealth = controller.GetMaxHealth();
            //myscore = controller.killHistory["Dot"];
            //if (scoretext != null)
            //{
            //    scoretext.text = "Score: " + myscore.ToString();
            //}
            //Debug.Log(myscore);
        }

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
            if (Numhearts[i] != null)
            {
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
            } 
            else
            {
                // Debug.Log("UI Error!");
            }
        }
    }
}