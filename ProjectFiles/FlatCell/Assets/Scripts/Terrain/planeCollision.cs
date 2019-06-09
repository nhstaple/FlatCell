using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;

public class planeCollision : MonoBehaviour
{
    GameObject player;

    bool locked = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void lerpWithoutThread(Color targetColor)
    {
        const float refreshModifier = 32;
        const float lerpTime = 5f;
        float t = 0.0f;
        int count = 0;
        Material mat = GetComponent<Renderer>().material;
        while (t <= lerpTime)
        {
            count++;
            t += refreshModifier * Time.deltaTime;
            Color c = Color.Lerp(mat.color,
                                 targetColor,
                                 t / lerpTime);

            mat.color = c;
        }
    }

    IEnumerator lerpColorToPlayer(Color targetColor)
    { 
        const float refreshModifier = 32;
        const float lerpTime = 5f;
        float t = 0.0f;
        int count = 0;
        Material mat = GetComponent<Renderer>().material;
        while (t <= lerpTime)
        {
            count++;
            t += refreshModifier * Time.deltaTime;
            Color c = Color.Lerp(mat.color,
                                 targetColor,
                                 t / lerpTime);

            mat.color = c;
            yield return new WaitForSeconds(0.25f * refreshModifier * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // if(other.gameObject.ToString().Contains("Player") && !other.gameObject.ToString().Contains("Projectile"))
        if (!other.gameObject.ToString().Contains("Projectile"))
        {
            IGeo[] res = other.gameObject.GetComponents<IGeo>();
            if(res.Length > 0)
            {
                IGeo geo = res[0];
                if (geo != null)
                {
                    // a primitive synch lock
                    if(locked == false)
                    {
                        locked = true;
                        StartCoroutine("lerpColorToPlayer", geo.GetColor());
                        // lerpWithoutThread(geo.GetColor());
                        locked = false;
                    }
                }
            }
            // lerpColorToPlayer(GetComponent<Renderer>().material);
        }
    }

}
