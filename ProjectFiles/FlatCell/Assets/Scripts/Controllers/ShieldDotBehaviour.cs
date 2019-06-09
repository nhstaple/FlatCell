using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;

namespace DotBehaviour.Command
{
    class ShieldDotBehaviour : SimpleDotBehaviour
    {
        private float period = 0.0f;

        new void Start()
        {
            base.Start();
            type = "Shield Dot";
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
            Shield shield = owner.GetShield();
            if (!shield.IsActve() && Random.Range(0, 100) <= owner.GetShieldChance())
            {
                if (Random.Range(0, 100) <= owner.GetShieldChance() * 2)
                {
                    period = Random.Range(0, shield.GetMaxEnergy());
                }
            }
            if (period >= 0 && shield.IsReady())
            {
                owner.FlameOn();
                period -= Time.deltaTime;
            }
            else if (!shield.IsReady())
            {
                owner.FlameOff();
            }
        }
    }
}
