/*
 * 
\* Boost.cs
 *
\* Nick S.
\* Game Logic - Game Mechanics
 *
*/
using UnityEngine;

namespace Geo.Command
{
    public class Boost : Shield
    {
        // Values of the base class, used to be visible via the editor.
        [SerializeField] private float e;
        [SerializeField] private float max;
        //

        [SerializeField] private float BoostEnergy = 0.5f;
        [SerializeField] private float MaxBoostEnergy = 0.5f;

        public float RechargeTime = 2f;

        public void setVals()
        {
            energy = BoostEnergy;
            MaxEnergy = MaxBoostEnergy;
        }

        new public void Start()
        {
            setVals();
        }

        new void Update()
        {
            e = energy;
            max = MaxEnergy;

            // If the components are null, grab them.
            if (Owner == null || owner == null)
            {
                grabComponents();
            }
            // If the shield is active and not busy charging.
            if (active && !charging)
            {
                Drain(Time.deltaTime);
            }
            // If the shield is charging.
            else if (charging)
            {
                Charge(Time.deltaTime);
            }
        }

        // Activate boost.
        new public void TurnOn()
        {
            active = true;
        }

        // Deactivate boost.
        new public void TurnOff()
        {
            active = false;
        }

        // Refill the boost energy.
        new protected void Charge(float e)
        {
            energy += e;
            if (energy >= RechargeTime)
            {
                energy = MaxEnergy;
                charging = false;
            }
        }

        // Adds energy back to the shield. Doesn't do any overflow checking.
        public void LazyCharge(float e)
        {
            energy += e;
        }
    }
}