// DotWeapon.cs
// Nick S.
// Game Logic - Combat

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;
using Projectile.Command;

/*
 * Dot Weapon
*/

namespace Weapon.Command
{
    public class DotWeapon : WeaponObject
    {
        public new void init(IGeo GeoOwner, float Damage, float Pierce = 0, float Rate = 0.01f, float lifeTime = 2.5f)
        {
            base.init(GeoOwner, Damage, Pierce, Rate, lifeTime);
            this.Projectile = gameObject.AddComponent<DotBullet>();
            DotBullet boopCoast = (DotBullet) this.Projectile;
            boopCoast.init(this, Damage, Piercing, ProjectileLifetime);
        }

        new public void Fire(Vector3 movementDir, Vector3 pos, float push, float SpawnOffset)
        {
            base.Fire(movementDir, pos, push, SpawnOffset);

            if (shootCounter >= this.FireRate)
            {
                Vector3 spawnLoc = pos + Owner.GetForward() * SpawnOffset;
                shootCounter = 0.0f;
                DotBullet boopCast = (DotBullet)Projectile;
                GameObject bullet = boopCast.Spawn(spawnLoc);
                Rigidbody bullet_rigidbody;
                bullet_rigidbody = bullet.GetComponent<Rigidbody>();
                bullet_rigidbody.AddRelativeForce(lastMove * (push + Owner.GetCurrentSpeed()), ForceMode.Impulse);
            }
        }
    }
}