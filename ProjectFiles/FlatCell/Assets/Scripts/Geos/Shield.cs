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
    public class Shield : ScriptableObject
    {
        private float energy = 0;
        private bool ready = true;
        private bool active = false;
        private float max = 0;

        public void SetMaxEnergy(float f)
        {
            energy = max = f;
        }

        public float GetPercent()
        {
            return energy / max;
        }

        // Drains energy from the shield.
        // If the energy is empty, then the shield needs to cooldown.
        public void Drain(float e)
        {
            energy -= e;
            if (energy <= 0)
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
            if (energy >= max)
            {
                energy = max;
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
            return max;
        }

        public bool IsReady()
        {
            return ready;
        }

        public bool IsActve()
        {
            return active;
        }
    }
}
