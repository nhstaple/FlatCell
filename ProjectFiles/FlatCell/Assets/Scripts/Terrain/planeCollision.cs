using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class planeCollision : MonoBehaviour
{
    GameObject player;

    const float lerpTime = 5.0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    IEnumerator lerpColor(float tick)
    {
        float t = 0.0f;
        int count = 0;
        while (t <= lerpTime)
        {
            count++;
            t += tick;
            Color c = Color.Lerp(GetComponent<Renderer>().material.color,
                                 player.GetComponent<PlayerController>().GetColor(),
                                 t / lerpTime);

            GetComponent<Renderer>().material.color = c;
            yield return new WaitForSeconds(tick);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.ToString().Contains("Player") && !other.gameObject.ToString().Contains("Projectile"))
        {
            StartCoroutine("lerpColor", Time.deltaTime);
        }
    }

}
