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
 * Pubilic
 * void SetMaxEnergy(float f)
 * // Returns ernergy/max
 * float GetPercent()
 * // Adds the energy to this.energy. +/- is handle internally.
 * void AddEnergy(float e)
 * 
 * // Turns the shields on or off. ie, sets this.active
 * void TurnOn()
 * void TurnOff()
 * 
 * Private
 * void Drain(float e)
 * void Charge(float e)
*/

namespace Geo.Command
{
    public class Shield : MonoBehaviour // ScriptableObject
    {
        public bool DebugPrint = true;

        // The shield's energy in seconds.
        [SerializeField] public float energy = 0;

        // Indicates if the shield is charging.
        [SerializeField] public bool charging { get; private set; } = false;

        // Indicates if the shields are on.
        [SerializeField] public bool active { get; private set; } = false;

        // The max value of the energy in seconds.
        [SerializeField] public float MaxEnergy = 2;

        public void Start()
        {
            energy = MaxEnergy;
        }

        public void Update()
        {
            if(active && !charging)
            {
                Drain(Time.deltaTime);
            }
            else if(charging)
            {
                Charge(Time.deltaTime);
            }
            else
            {

            }
        }

        public void SetMaxEnergy(float f)
        {
            energy = MaxEnergy = f;
        }

        public float GetPercent()
        {
            return energy / MaxEnergy;
        }

        // Drains energy from the shield.
        // If the energy is empty, then the shield needs to cooldown.
        public void Drain(float e)
        {
            if(e > 0) { e *= -1;  }
            energy += e;
            if (energy < 0)
            {
                energy = 0;
                charging = true;
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
                charging = false;
            }
        }

        public bool IsCharging()
        {
            if(energy <= 0)
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

        public void TurnOn()
        {
            active = true;
        }

        public void TurnOff()
        {
            active = false;
        }
    }
}
