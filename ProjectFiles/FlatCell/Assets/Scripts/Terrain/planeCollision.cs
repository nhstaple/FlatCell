using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class planeCollision : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    IEnumerator lerpColorToPlayer(Material mat)
    {
        const float lerpTime = 5.0f;
        float t = 0.0f;
        int count = 0;
        while (t <= lerpTime)
        {
            count++;
            t += Time.deltaTime;
            Color c = Color.Lerp(GetComponent<Renderer>().material.color,
                                 player.GetComponent<PlayerController>().GetColor(),
                                 t / lerpTime);

            mat.color = c;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.ToString().Contains("Player") && !other.gameObject.ToString().Contains("Projectile"))
        {
            StartCoroutine("lerpColorToPlayer", GetComponent<Renderer>().material);
        }
    }

}
