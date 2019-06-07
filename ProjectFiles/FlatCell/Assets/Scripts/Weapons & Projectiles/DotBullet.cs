// DotBullet.cs
// Nick S.
// Game Logic - Combat

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Command;

/*
 * Dot Bullet Script
 * 
 * This is the script that's attached to a bullet gameObject when it spawns.
 * 
    - IProjectile is the logical interface
    - 
 * 
*/

namespace Projectile.Command
{
    public class DotBullet : ProjectileObject
    {

        new public GameObject Spawn(Vector3 Location)
        {
            GameObject projectile = base.Spawn(Location);

            // For damage calc
            DotBullet bullet = projectile.AddComponent<DotBullet>();
            bullet.SetDamage(Damage, Piercing);
            bullet.SetLifeTime(LifeTime);
            bullet.SetOwner(Owner);

            return projectile;
        }

        new public void Start()
        {
            base.Start();
        }

        new public void Update()
        {
            base.Update();
        }
    }
}