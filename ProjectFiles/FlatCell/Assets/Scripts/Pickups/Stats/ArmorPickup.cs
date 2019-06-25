/*
\* ArmorPickup.cs
\* Nick S. 
*/


using Pickup.Command;
using UnityEngine;
using Geo.Command;
using Controller.Player;

namespace Pickup.Stats
{

    class ArmorPickup : PickupObject, IPickup
    {
        [SerializeField] protected float armor;
        protected float ArmorLowRange = 1f;
        protected float ArmorHighRange = 2.25f;

        public void Start()
        {
            type = EPickup_Type.Armor;
        }

        new public void Init(IGeo geo)
        {
            base.Init(geo);
            this.Start();
            UpdateValues();
        }

        new public void UpdateValues()
        {
            this.armor = GetStatFromGeo(1f, ArmorLowRange, ArmorHighRange);
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
                    p.ModifyArmor(this.armor);
                    Destroy(this.gameObject);
                }
            }
        }

        new public GameObject Spawn(Vector3 Location)
        {
            // Debug.Log("Making a color pickup!");
            GameObject p = base.Spawn(Location);
            p.name += "Armor";

            ArmorPickup pickup = p.AddComponent<ArmorPickup>();

            pickup.Init(owner);
            pickup.armor = this.armor;

            rend.material.color = Pickup_Colors.Armor_c;

            Destroy(p, lifeTime);
            return p;
        }
    }
}

