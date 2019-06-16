﻿using UnityEngine;

namespace Terrain.Command
{
    public class colorPlane : MonoBehaviour
    {
        void Awake()
        {
            // Pick a random, saturated and not-too-dark color
            GetComponent<Renderer>().material.color = Color.white; // Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
    }
}