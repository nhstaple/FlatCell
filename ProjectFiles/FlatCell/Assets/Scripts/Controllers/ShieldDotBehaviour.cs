/*
 * 
\* ShieldDotBehaviour.cs
 *
\* Nick S.
\* Game Logic - AI
 *
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;
using Geo.Meter;

/*
 * ShieldDotBehaviour - A Shield Dot
 * 
 * This behaviour script extends the functionality of SimpleDotBehaviour.cs.
 * 
 * This AI will move and toggle shields. That's about it.
 * 
*/ 

namespace DotBehaviour.Command
{
    class ShieldDotBehaviour : SimpleDotBehaviour
    {
        // The random period that the AI will toggle their shields.
        private float period = 0.0f;

        // A reference to the dot's shield.
        private Shield shield;

        new void Start()
        {
            base.Start();
            type = EDot_Behaviour.Shield;
            shield = owner.GetShield();
        }

        new public void Update()
        {
            base.Update();
            if (Time.timeScale > 0)
            {
                Shields();
            }
        }

        new public void Shields()
        {
            // Validate the shield.
            if(shield == null)
            {
                shield = owner.GetShield();
            }
            if(shield)
            {
                // Randomly set the peroid.
                if (Random.Range(0, 100) <= owner.GetShieldChance())
                {
                    period = Random.Range(0, shield.GetMaxEnergy());
                }

                // Turn the shields on.
                if (period >= 0 && !shield.IsCharging())
                {
                    owner.FlameOn();
                }

                // Turn the shields off.
                else if (shield.IsCharging())
                {
                    owner.FlameOff();
                }

                // Drain the period.
                period -= Time.deltaTime;
            }
        }
    }
}
