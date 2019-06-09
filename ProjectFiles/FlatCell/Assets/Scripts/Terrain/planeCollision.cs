using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;

struct Animation
{
    public void lerpColorWithoutThread(float lerpTime, Material mat, Color targetColor, bool locked = false)
    {
        const float refreshModifier = 16;
        float t = 0.0f;
        int count = 0;
        while (t <= lerpTime)
        {
            count++;
            t += refreshModifier * Time.deltaTime;
            Color c = Color.Lerp(mat.color,
                                 targetColor,
                                 t / lerpTime);

            mat.color = c;
        }
        locked = false;
    }

    public IEnumerator lerpColor(float lerpTime, Material mat, Color targetColor, bool locked = false)
    {
        const float refreshModifier = 4f;
        float t = 0.0f;
        int count = 0;
        while (t <= lerpTime)
        {
            count++;
            float roll = Random.Range(1f, 2f);
            float tick = roll * refreshModifier * Time.deltaTime;
            t += tick;
            Color c = Color.Lerp(mat.color,
                                 targetColor,
                                 t / lerpTime);

            mat.color = c;
            yield return new WaitForSeconds(tick);
        }
        locked = false;
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
