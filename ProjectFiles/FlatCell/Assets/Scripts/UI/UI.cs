/*
 * 
\* UI.cs
 *
\* TODO David
 *
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Geo.Command;
using Geo.Meter;
using Controller.Player;

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
    private float Throttle = 0.5f;// = Random.Range(0.0f,1.0f);

    GameObject player;
    public Text scoretext;
    public Text speedtext;
    public Text armortext;
    public Text boosttext;
    public Text shieldtext;
    public Text shieldtime;
    public Text healthtext;
    public Text healthlives;
    public Text healthmax;

    //Nick's stuff for shield Fill
    IGeo p;
    Shield shieldCopy;
    public float fillPercent;
    public float boostfillPercent;
    float minHeartShieldPercent = 0.33f;
    public Image ShieldBar;
    public Image BoostBar;
    public Image HealthBar;
    Color ogColor;
    void GetPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }

    //public PlayerController p;
    // Start is called before the first frame update
    void OnAwake()
    {
        if (player != null)
        {
            PlayerController controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                health = controller.geo.GetHealth();
                maxhealth = controller.geo.GetMaxHealth();
                p = controller.geo;
                if (p != null)
                {
                    shieldCopy = p.GetShield();
                    fillPercent = shieldCopy.GetPercent();
                    ShieldBar.fillAmount = fillPercent;
                    boostfillPercent = controller.GetBoostPercent();
                }
            }
            else
            {
                health = maxhealth = 1;
            }
            ogColor = Color.white;
        }   
        this.RandomizeThrottle();
    }

    float boostCounter = 0f;
    float boostToggleTime = 0.175f;
    bool boostToggle = false;

    float shieldCounter = 0f;
    float shieldToggleTime = 0.175f;
    bool shieldToggle = false;

    private void RandomizeThrottle()
    {
       Throttle = Random.Range(0.0f, 1.0f);
    }
    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            GetPlayer();
        }
        IGeo controller = player.GetComponent<PlayerController>().geo;
        if(controller != null)
        {
            myscore = controller.GetScore();
        }
        if (TimetoWatch >= Throttle)
        {
            health = controller.GetHealth();
            maxhealth = controller.GetMaxHealth();
        }

        if (controller != null)
        {
            fillPercent = controller.GetShield().GetPercent();
            if (ShieldBar != null)
            {
                if (player.GetComponent<PlayerController>().geo.GetShield().IsCharging())
                {
                    shieldCounter += Time.deltaTime;
                    if (shieldCounter >= shieldToggleTime)
                    {
                        shieldCounter = 0f;
                        if (shieldToggle)
                        {
                            shieldToggle = false;
                            ShieldBar.enabled = false;
                        }
                        else
                        {
                            shieldToggle = true;
                            ShieldBar.enabled = true;
                        }
                    }
                }
                else
                {
                    ShieldBar.enabled = true;
                }
                ShieldBar.fillAmount = fillPercent;
            }
            if (BoostBar != null)
            {
                if(player.GetComponent<PlayerController>().IsBoostCharging())
                {
                    BoostBar.fillAmount = player.GetComponent<PlayerController>().GetBoostRechargePercent();
                    boostCounter += Time.deltaTime;
                    if (boostCounter >= boostToggleTime)
                    {
                        boostCounter = 0f;
                        if (boostToggle)
                        {
                            boostToggle = false;
                            BoostBar.enabled = false;
                        }
                        else
                        {
                            boostToggle = true;
                            BoostBar.enabled = true;
                        }
                    }
                }
                else
                {
                    BoostBar.enabled = true;
                    BoostBar.fillAmount = player.GetComponent<PlayerController>().GetBoostPercent();
                }
            }
            if(HealthBar != null)
            {
                
                float r = controller.GetHealth();
                while(r - 1 > 0)
                {
                    r--;
                }
                if(r < 0)
                {
                    r++;
                }
                HealthBar.fillAmount = r;
            }
        }

        if (scoretext != null)
        {
            scoretext.text = "Score: " + myscore.ToString();
        }
        if(speedtext != null && player != null)
        {
            speedtext.text = "Speed: " + (Mathf.Round(100f * player.GetComponent<PlayerController>().geo.GetSpeedPercent())).ToString() + "%";
        }
        if(armortext != null && player != null)
        {
            armortext.text = "Armor: " + (Mathf.Round(player.GetComponent<PlayerController>().geo.GetArmor())).ToString() + "%";
        }
        if(boosttext != null)
        {
            if (player.GetComponent<PlayerController>().IsBoostCharging())
            {
                boosttext.text = "Boost Charging!";
            }
            else
            {
                boosttext.text = "Boost    " + (Mathf.Round(100f * player.GetComponent<PlayerController>().GetBoostPercent())).ToString() + "%";
            }
        }
        if(shieldtext != null)
        {
            if (player.GetComponent<PlayerController>().geo.GetShield().IsCharging())
            {
                shieldtext.text = "Shield Charging!";
            }
            else
            {
                shieldtext.text = "Shield    " + (Mathf.Round(100f * fillPercent)).ToString() + "%";
            }
        }
        if(shieldtime != null)
        {
            shieldtime.text = "Time " + Mathf.Round(player.GetComponent<PlayerController>().geo.GetShield().GetMaxDuration()) + "s";
        }
        float remainder = controller.GetHealth();
        if (healthtext != null)
        {
            while (remainder - 1 > 0)
            {
                remainder--;
            }
            if (remainder < 0)
            {
                remainder++;
            }
            if (player.GetComponent<PlayerController>().geo.GetShield().active)
            {
                // healthtext.text = "Energy    " + Mathf.Round(remainder*100) + "%\n";
                healthtext.text = "Protected by shields!";
                HealthBar.color = Color.Lerp(Color.red + new Color(0, 0, 80 / 255f), Color.blue + new Color(0, 80 / 255f, 0), player.GetComponent<PlayerController>().geo.GetShield().GetPercent() + minHeartShieldPercent);
            }
            else
            {
                healthtext.text = "Energy    " + Mathf.Round(remainder * 100) + "%\n";
                HealthBar.color = Color.red + new Color(0, 0, 80/255f);
            }
        }
        if (healthlives != null)
        {
            healthlives.text = "Lives " + Mathf.Floor(controller.GetHealth());
        }
        if (healthmax != null)
        {
            healthmax.text = "Max " + Mathf.Ceil(controller.GetHealth());
        }

        if (myscore >= 20.0f)
        {
            SceneManager.LoadScene("WIN");
        }
        if (health > maxhealth)
        {
            health = maxhealth;
        }
        for (int i = 0; i < Numhearts.Length; i++)
        {

            Numhearts[i].enabled = false;
            /*
            if (player.GetComponent<PlayerController>().geo.GetShield().active)
            {
                Numhearts[i].color = Color.Lerp(Color.white, Color.blue, player.GetComponent<PlayerController>().geo.GetShield().GetPercent() + minHeartShieldPercent);
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
            */
        }
    }
}