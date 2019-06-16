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
        const float PlayerChargeChance = 10;
        const float ChargeMinMod = 0f;
        const float ChargeMaxMod = 1f;

        public new void Init(IGeo GeoOwner, AudioClip Sound, float Damage, float Pierce = 0, float Rate = 0.25f, float lifeTime = 2.5f)
        {
            base.Init(GeoOwner, Sound, Damage, Pierce, Rate, lifeTime);
            this.Projectile = gameObject.AddComponent<DotProjectile>();
            DotProjectile cast = (DotProjectile) this.Projectile;
            cast.Init(this, Damage, Piercing, ProjectileLifetime);
        }

        new void Start()
        {
            base.Start();
            this.Projectile = gameObject.AddComponent<DotProjectile>();
            DotProjectile cast = (DotProjectile)this.Projectile;
            cast.Init(this, Damage, Piercing, ProjectileLifetime);
        }

        new public void Fire(Vector3 movementDir, Vector3 pos, float push, float SpawnOffset)
        {
            base.Fire(movementDir, pos, push, SpawnOffset);
            if (shootCounter >= this.FireRate)
            {
                PlaySound();

                Vector3 spawnLoc = pos + Owner.GetForward() * SpawnOffset;
                shootCounter = 0.0f;
                DotProjectile boopCast = (DotProjectile)Projectile;
                GameObject bullet = boopCast.Spawn(spawnLoc);
                Rigidbody bullet_rigidbody;
                bullet_rigidbody = bullet.GetComponent<Rigidbody>();
                bullet_rigidbody.mass = 0.1f;
                if (Owner.ToString().Contains("Player"))
                {
                    float modified = 1;
                    if (Random.Range(1, 100) < PlayerChargeChance)
                    {
                        modified = Random.Range(ChargeMinMod, ChargeMaxMod);
                    }
                    bullet_rigidbody.AddRelativeForce(lastMove * (push * (1 + Owner.GetMovementMagnitude()) ), ForceMode.Impulse);
                }
                else
                {
                    bullet_rigidbody.AddRelativeForce(lastMove * (push * (1 + Owner.GetMovementMagnitude()) ), ForceMode.Impulse);
                }
            }
        }
    }
}