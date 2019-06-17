﻿/*
 * 
\* Meter.cs
 *
\* Nick S.
\* Game Logic - Game Mechanics
 *
*/

/*
 * TODO
 * Abstract Boost and Shield to derive from a Meter class.
*/

using UnityEngine;

/*
 * Shield
 * 
 * This is an abstraction for the geometry's shield.
 * 
 Public
   // Sets the max energy
   void SetMaxEnergy(float f)

   // Returns ernergy/max
   float GetPercent()

   // Adds the energy to this.energy. +/- is handle internally.
   void AddEnergy(float e)
  
   // Turns the shields on or off. ie, sets this.active
   void TurnOn()
   void TurnOff()
   
 Private
 
   // Drains energy from the shield.
   // If the energy is empty, then the shield needs to cooldown.
   void Drain(float e)

   // Adds energy back to the shield.
   // If the energy exceeds max then cap it and clear the cooldown flag
   void Charge(float e)
*/

namespace Geo.Command
{
    public class Meter : MonoBehaviour // ScriptableObject
    {
        // Values of the base class, used to be visible via the editor.
        [SerializeField] public float e;
        [SerializeField] public float max;
        //

        // The shield's energy in seconds.
        [SerializeField] public float energy { get; protected set; } = 0;

        // Indicates if the shield is charging.
        [SerializeField] public bool charging { get; protected set; } = false;

        // Indicates if the shields are on.
        [SerializeField] public bool active { get; protected set; } = false;

        // The max value of the energy in seconds.
        [SerializeField] public float MaxEnergy { get; protected set; } = 0;

        // The gameobject that this script is attached to.
        protected GameObject Owner;

        // The geo object that this shield logically belongs to.
        protected IGeo owner;

        // The owner's mesh renderer.
        protected MeshRenderer rend;

        // The owner's trail.
        protected TrailRenderer trail;

        protected void grabComponents()
        {
            Owner = gameObject;
            IGeo[] boop = Owner.GetComponents<IGeo>();
            if (boop.Length > 0)
            {
                owner = boop[0];
                rend = Owner.GetComponent<MeshRenderer>();
                trail = Owner.GetComponent<TrailRenderer>();
            }
        }

        protected void setVals()
        {
            energy = MaxEnergy;
        }

        public void Awake()
        {
            grabComponents();
        }

        public void Start()
        {
            grabComponents();
            setVals();
        }

        public void Update()
        {
            e = energy;
            max = MaxEnergy;
            // If the components are null, grab them.
            if (rend == null || trail == null || Owner == null || owner == null)
            {
                grabComponents();
            }
            // If the shield is active and not busy charging.
            if (active && !charging)
            {
                Drain(Time.deltaTime);
            }
            else
            {
                Charge(Time.deltaTime);
            }
        }

        public void TurnOn()
        {
            active = true;
        }

        public void TurnOff()
        {
            active = false;
        }

        public bool CheckEnergy()
        {
            if (energy < MaxEnergy)
            {
                energy = MaxEnergy;
                charging = true;
                return false;
            }
            return true;
        }

        public void ForceRecharge()
        {
            energy = MaxEnergy;
            charging = false;
        }

        public bool IsCharging()
        {
            if (energy <= 0)
            {
                charging = true;
                energy = 0;
                return charging;
            }
            else
            {
                return charging;
            }
        }

        // Drains energy from the shield.
        // If the energy is empty, then the shield needs to cooldown.
        protected void Drain(float e)
        {
            if (e > 0) { e *= -1; }
            energy += e;
            if (energy < 0)
            {
                energy = 0;
                charging = true;
            }
        }

        // Adds energy back to the shield.
        // If the energy exceeds max then cap it and clear the cooldown flag
        protected void Charge(float e)
        {
            energy += e;
            if (energy >= MaxEnergy)
            {
                energy = MaxEnergy;
                charging = false;
            }
        }

        // Get and Set
        public float GetMaxEnergy()
        {
            return MaxEnergy;
        }

        public float GetPercent()
        {
            return energy / MaxEnergy;
        }

        public void SetMaxEnergy(float f)
        {
            energy = MaxEnergy = f;
        }

    }
}
