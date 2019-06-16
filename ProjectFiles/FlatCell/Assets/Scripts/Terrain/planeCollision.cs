// planeCollision.cs
// Brian C.
// Game Physics
// Nick S.
// Game Feel - Animation Manager

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;

public class Animation
{
    // Indicates if the animator is currently busy with a thread.
    bool busy;

    public Animation()
    {
        busy = false;
    }

    // Lerps material's color to value without threads.
    // FrameMulti skips frames
    // ie, frameMulti = 4 means color will be set 4 times less than normal lerp. This is to increase performance.
    public void lerpColorWithoutThread(float lerpTime, Material mat, Color targetColor, ref bool locked, float frameMulti = 2f)
    {
        if(locked == false && !busy)
        {
            locked = true;
            busy = true;
            float t = 0.0f;
            while (t <= lerpTime)
            {
                t += frameMulti * Time.deltaTime;
                mat.color = Color.Lerp(mat.color,
                                     targetColor,
                                     t / lerpTime);
            }
            locked = false;
            busy = false;
        }
    }

    // Runs a function after waitTime.
    // Used for calls backs 
    // source https://answers.unity.com/questions/516798/executing-an-action-after-a-coroutine-has-finished.html
    public IEnumerator WaitForSecondsThenExecute(Action method, float waitTime)
    {
        // Debug.Log("execute delegate coroutine!");
        yield return new WaitForSeconds(waitTime);
        method();
    }

    // Changing the upper and lower bounds will increase the spread at which this coroutine yields.
    // Ie, makes it look different.
    public IEnumerator lerpColor(float lerpTime, Material mat, Color targetColor, bool locked = false, float frameMulti = 2f, float juiceLowerRange = 1f, float juiceUpperRange = 2f)
    {
        if(locked == false && !busy)
        {
            locked = true;
            busy = true;
            float t = 0.0f;
            while (t <= lerpTime)
            {
                float tick = UnityEngine.Random.Range(juiceLowerRange, juiceUpperRange) * frameMulti * Time.deltaTime;
                t += tick;
                mat.color = Color.Lerp(mat.color,
                                     targetColor,
                                     t / lerpTime);

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

    bool animLock = false;
    float animTime = 2.5f;

    [SerializeField] public Animation anim = new Animation();
    IEnumerator coroutine;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animTime = UnityEngine.Random.Range(1f, 2f);
    }

    void OnCollisionEnter(Collision other)
    {
        Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), other.collider);

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
                    if (animLock == false)
                    {
                        coroutine = anim.lerpColor(UnityEngine.Random.Range(1f, 2f) * animTime, GetComponent<Renderer>().material, geo.GetColor(), animLock);
                        StartCoroutine(coroutine);
                        // float time = UnityEngine.Random.Range(1f, 2f) * animTime;
                        // anim.lerpColorWithoutThread(time, GetComponent<Renderer>().material, geo.GetColor(), ref animLock);
                    }
                }
            }
            // lerpColorToPlayer(GetComponent<Renderer>().material);
        }
    }

}
