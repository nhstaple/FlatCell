﻿/*
\* SpeedPickup.cs
\* Nick S. 
*/


using Pickup.Command;
using UnityEngine;
using Geo.Command;
using Controller.Player;

namespace Pickup.Stats
{

    class ColorPickup : PickupObject, IPickup
    {
        [SerializeField] protected Color color;
        protected float ColorLowRange = 0.10f;
        protected float ColorHighRange = 0.25f;
        float ColorIntensity = 10f;

        public void Start()
        {
            // Debug.Log("Color set!");
            type = EPickup_Type.Color;
        }

        new public void Init(IGeo geo)
        {
            // Debug.Log("Colorpickup init");
            base.Init(geo);
            this.Start();
        }

        new public void UpdateValues()
        {
            Debug.Log("Updating color pickup- " + owner.GetColor().ToString());
            this.color = GetStatFromGeo(owner.GetColor(), ColorLowRange, ColorHighRange);
            Debug.Log(this.color);
        }

        new public void OnCollisionEnter(Collision geo)
        {
            base.OnCollisionEnter(geo);
            if (playerHit)
            {
                IGeo p = geo.gameObject.GetComponent<PlayerController>().geo;
                if (p != null)
                {
                    Debug.Log("The player's mesh hit the pickup!");
                    Debug.Log(this.color);
                    p.AddColor(this.color);
                    Destroy(this.gameObject);
                }
            }
        }

        new public GameObject Spawn(Vector3 Location)
        {
            // Debug.Log("Making a color pickup!");
            GameObject p = base.Spawn(Location);
            p.name += "Color";

            ColorPickup pickup = p.AddComponent<ColorPickup>();

            pickup.Init(owner);
            pickup.color = this.color;

            rend.material.color = Pickup_Colors.Color_c;
            rend.material.SetColor("_EmissionColor", this.color * ColorIntensity);
            rend.material.EnableKeyword("_EMISSION");

            Destroy(p, lifeTime);
            return p;
        }
    }
}
