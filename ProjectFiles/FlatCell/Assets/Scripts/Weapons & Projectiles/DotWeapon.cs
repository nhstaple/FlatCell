using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Geo.Command;
using Projectile.Command;

namespace Weapon.Command
{
    public class DotWeapon : WeaponObject
    {
        public DotWeapon(IGeo GeoOwner, float Rate, float lifeTime = 2.5f)
        {
            this.Owner = GeoOwner;
            this.Projectile = new DotProjectile(this, Damage, Piercing, ProjectileLifetime);
            this.FireRate = Rate;
        }

        new public void Fire(Vector3 movementDir, Vector3 pos, float push, float SpawnOffset)
        {
            base.Fire(movementDir, pos, push, SpawnOffset);

            if (shootCounter >= this.FireRate)
            {
                shootCounter = 0.0f;
                Vector3 spawnLoc = pos + Owner.GetForward() * SpawnOffset;
                GameObject bullet = Projectile.Spawn(spawnLoc);
                Rigidbody bullet_rigidbody;
                bullet_rigidbody = bullet.GetComponent<Rigidbody>();
                bullet_rigidbody.AddRelativeForce(lastMove * (push + Owner.GetCurrentSpeed()), ForceMode.Impulse);
            }
        }
    }
}