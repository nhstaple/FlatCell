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
                ShieldBar.fillAmount = fillPercent;
            }
        }

        if (scoretext != null)
        {
            scoretext.text = "Score: " + myscore.ToString();
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
            
        }
    }
}