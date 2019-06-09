// Shield.cs
// Nick S. & Kyle C.
// Game Logic - AI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Shield
 * 
 * This is an abstraction for the geometry's shield.
 * 
*/

namespace Geo.Command
{
    public class Shield : MonoBehaviour // ScriptableObject
    {
        // The shield's energy in seconds.
        [SerializeField] private float energy = 0;

        // If the shield is ready. If this is false then the shield must cooldown for @this.max seconds.
        [SerializeField] private bool ready = true;

        // Indicates if the shields are on.
        [SerializeField] private bool active = false;

        // The max value of the energy in seconds.
        [SerializeField] public float MaxEnergy = 0;

        public void SetMaxEnergy(float f)
        {
            energy = MaxEnergy = f;
        }

        public float GetPercent()
        {
            return energy / MaxEnergy;
        }

        public void Update()
        {
        }

        // Drains energy from the shield.
        // If the energy is empty, then the shield needs to cooldown.
        public void Drain(float e)
        {
            energy -= e;
            if (energy < 0 && ready)
            {
                energy = 0;
                ready = false;
            }
        }

        // Adds energy back to the shield.
        // If the energy exceeds max then cap it and clear the cooldown flag
        public void Charge(float e)
        {
            energy += e;
            if (energy >= MaxEnergy)
            {
                energy = MaxEnergy;
                ready = true;
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

        public float GetEnergy()
        {
            return energy;
        }

        public float GetMaxEnergy()
        {
            return MaxEnergy;
        }

        public bool IsReady()
        {
            if(energy <=0 && ready)
            {
                ready = false;
                energy = 0;
                return ready;
            }
            else
            {
                return ready;
            }
        }

        public bool IsActve()
        {
            return active;
        }
    }
}
