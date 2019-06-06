using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planeCollision : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

}
