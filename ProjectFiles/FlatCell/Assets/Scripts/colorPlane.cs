using UnityEngine;

public class colorPlane : MonoBehaviour
{
    void Start()
    {
    }
    void Awake()
    {
        // Pick a random, saturated and not-too-dark color
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }
}