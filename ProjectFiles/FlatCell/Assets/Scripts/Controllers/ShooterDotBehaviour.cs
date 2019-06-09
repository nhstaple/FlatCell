using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Command;

namespace DotBehaviour.Command
{
    class ShooterDotBehaviour : SimpleDotBehaviour
    {
        new void Start()
        {
            base.Start();
            type = "Shooter Dot";
        }

        new public void exec()
        {
            base.exec();
            Fire();
        }

        new public void Update()
        {
            base.Update();
            Fire();
        }

        new public void Fire()
        {
            if (Random.Range(0, 100) <= owner.GetFireChance())
            {
                if (Random.Range(0, 100) <= owner.GetFireChance() * owner.GetFireChance() / 2)
                {
                    owner.Shoot();
                }
            }
        }
    }
}
