/*
\* SpeedPickup.cs
\* Nick S. 
*/


using Pickup.Command;
using UnityEngine;
using Geo.Command;
using Controller.Player;

namespace Pickup.Stats
{

    class SpeedPickup : PickupObject, IPickup
    {
        [SerializeField] protected float speed;
        protected float SpeedLowRange = 0.10f;
        protected float SpeedHighRange = 0.15f;

        public void Start()
        {
            type = EPickup_Type.Speed;
        }

        new public void Init(IGeo geo)
        {
            base.Init(geo);
            this.Start();
            UpdateValues();
        }

        new public void UpdateValues()
        {
            this.speed = GetStatFromGeo(owner.GetSpeed(), SpeedLowRange, SpeedHighRange);
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
                    p.SetSpeed(p.GetSpeed() + this.speed);
                    Destroy(this.gameObject);
                }
            }
        }

        new public GameObject Spawn(Vector3 Location)
        {
            // Debug.Log("Making a color pickup!");
            GameObject p = base.Spawn(Location);
            p.name += "Speed";

            SpeedPickup pickup = p.AddComponent<SpeedPickup>();

            pickup.Init(owner);
            pickup.speed = this.speed;

            rend.material.color = Pickup_Colors.Speed_c;

            Destroy(p, lifeTime);
            return p;
        }
    }
}

