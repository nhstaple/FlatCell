using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;

public class Animation
{
    bool busy = false;
    public Animation()
    {
    }

    public void lerpColorWithoutThread(float lerpTime, Material mat, bool locked,  Color targetColor, float frameMulti = 2f)
    {
        if(locked == false && !busy)
        {
            locked = true;
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
            locked = false;
            busy = false;
        }
    }

    public IEnumerator WaitForSecondsThenExecute(Action method, float waitTime)
    {
        Debug.Log("execute delegate coroutine!");
        yield return new WaitForSeconds(waitTime);
        method();
    }

    public IEnumerator lerpColor(float lerpTime, Material mat, Color targetColor, bool locked, float frameMulti = 2f, float juiceLowerRange = 1f, float juiceUpperRange = 2f)
    {
        if(locked == false && !busy)
        {
            locked = true;
            busy = true;
            float t = 0.0f;
            int count = 0;
            while (t <= lerpTime)
            {
                count++;
                float roll = UnityEngine.Random.Range(juiceLowerRange, juiceUpperRange);
                float tick = roll * frameMulti * Time.deltaTime;
                t += tick;
                Color c = Color.Lerp(mat.color,
                                     targetColor,
                                     t / lerpTime);

                mat.color = c;
                yield return new WaitForSeconds(tick);
            }
            locked = false;
            busy = false;
        }
    }

}



public class planeCollision : MonoBehaviour
{
    GameObject player;

    bool locked = false;

    [SerializeField] public Animation anim = new Animation();
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
                        coroutine = anim.lerpColor(2.5f, GetComponent<Renderer>().material, geo.GetColor(), locked);
                        StartCoroutine(coroutine);
                        // lerpWithoutThread(geo.GetColor());
                    }
                }
            }
            // lerpColorToPlayer(GetComponent<Renderer>().material);
        }
    }

}
