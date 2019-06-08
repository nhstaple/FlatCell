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
            type = "ShooterDot";
        }

        new public void exec()
        {
            CheckScore();
            Move();
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
