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
            type = "ShieldDot";
        }

        new public void exec()
        {
            CheckScore();
            Move();
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
            }
            else
            {
                owner.FlameOff();
            }
        }
    }
}
