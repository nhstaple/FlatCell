using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;

class Animation
{
    public bool busy { get; private set; } = false;

    public Animation()
    {
        busy = false;
    }

    public void lerpColorWithoutThread(float lerpTime, Material mat, Color targetColor, float frameMulti = 2f)
    {
        if(busy == false)
        {
            busy = true;
            float t = 0.0f;
            int count = 0;
            while (t <= lerpTime)
            {
                count++;
                t += frameMulti * Time.deltaTime;
                Color c = Color.Lerp(mat.color,
                                     targetColor,
                                     t / lerpTime);

                mat.color = c;
            }
            busy = false;
        }
    }

    public IEnumerator lerpColor(float lerpTime, Material mat, Color targetColor, float frameMulti = 2f, float juiceLowerRange = 1f, float juiceUpperRange = 2f)
    {
        Debug.Log("busy?");
        Debug.Log(busy);
        if(busy == false)
        {
            busy = true;
            float t = 0.0f;
            int count = 0;
            while (t <= lerpTime)
            {
                count++;
                float roll = Random.Range(juiceLowerRange, juiceUpperRange);
                float tick = roll * frameMulti * Time.deltaTime;
                t += tick;
                Color c = Color.Lerp(mat.color,
                                     targetColor,
                                     t / lerpTime);

                mat.color = c;
                yield return new WaitForSeconds(tick);
            }
            busy = false;
        }
    }

}



public class planeCollision : MonoBehaviour
{
    GameObject player;

    bool locked = false;

    Animation anim = new Animation();
    IEnumerator coroutine;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }


    void OnTriggerEnter(Collider other)
    {
        // if(other.gameObject.ToString().Contains("Player") && !other.gameObject.ToString().Contains("Projectile"))
        if (!other.gameObject.ToString().Contains("Projectile"))
        {
            IGeo[] res = other.gameObject.GetComponents<IGeo>();
            if (res.Length > 0)
            {
                IGeo geo = res[0];
                if (geo != null)
                {
                    // a primitive synch lock
                    if (locked == false)
                    {
                        locked = true;
                        coroutine = anim.lerpColor(2.5f, GetComponent<Renderer>().material, geo.GetColor());
                        StartCoroutine(coroutine);
                        // lerpWithoutThread(geo.GetColor());
                    }
                }
            }
            // lerpColorToPlayer(GetComponent<Renderer>().material);
        }
    }

}
