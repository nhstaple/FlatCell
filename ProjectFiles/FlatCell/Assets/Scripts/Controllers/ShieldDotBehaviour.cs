using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;

namespace DotBehaviour.Command
{
    class ShieldDotBehaviour : SimpleDotBehaviour
    {
        private float period = 0.0f;
        private Shield shield;

        new void Start()
        {
            base.Start();
            type = "Shield Dot";
            shield = owner.GetShield();
        }

        new public void exec()
        {
            base.exec();
            Shields();
        }

        new public void Update()
        {
            base.Update();
            Shields();
        }

        new public void Shields()
        {
            if(shield == null)
            {
                shield = owner.GetShield();
            }

            if (Random.Range(0, 100) <= owner.GetShieldChance())
            {
                if (Random.Range(0, 100) <= owner.GetShieldChance() * 2)
                {
                    period = Random.Range(0, shield.MaxEnergy);
                }
            }

            if (period >= 0 && !shield.IsCharging())
            {
                owner.FlameOn();
            }
            else if (shield.IsCharging())
            {
                owner.FlameOff();
            }

            period -= Time.deltaTime;
        }
    }
}
