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
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.ToString().Contains("Player") && !other.gameObject.ToString().Contains("Projectile"))
        {
            GetComponent<Renderer>().material.color = player.GetComponent<PlayerController>().color;
        }
    }

}
