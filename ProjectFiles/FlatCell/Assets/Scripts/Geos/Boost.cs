/*
 * 
\* Boost.cs
 *
\* Nick S.
\* Game Logic - Game Mechanics
 *
*/
using UnityEngine;

/*
 * TODO
 * Abstract Boost and Shield to derive from a Meter class.
*/

namespace Geo.Command
{
    public class Boost : Meter
    {
        [SerializeField] float InitBoostEnergy = 0.5f;
        [SerializeField] float InitMaxBoostEnergy = 0.5f;

        public float RechargeTime = 2f;

        private void setVals()
        {
            MaxEnergy = InitMaxBoostEnergy;
            base.setVals();
        }

        new public void Start()
        {
            base.Start();
            setVals();
        }

        new public void Update()
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
                this.Charge(Time.deltaTime);
            }
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