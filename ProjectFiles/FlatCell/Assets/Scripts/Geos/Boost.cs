// Boost.cs
// Nick S.

using UnityEngine;

namespace Geo.Command
{
    public class Boost : Shield
    {
        // { get; protected set; } 
        // Values of the base class.
        [SerializeField] private float e;
        [SerializeField] private float max;

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

        new public void TurnOn()
        {
            active = true;
        }

        new public void TurnOff()
        {
            active = false;
        }

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