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

        public new void init(IGeo GeoOwner, AudioClip Sound, float Damage, float Pierce = 0, float Rate = 0.25f, float lifeTime = 2.5f)
        {
            base.init(GeoOwner, Sound, Damage, Pierce, Rate, lifeTime);
            this.Projectile = gameObject.AddComponent<DotProjectile>();
            DotProjectile boopCoast = (DotProjectile) this.Projectile;
            boopCoast.init(this, Damage, Piercing, ProjectileLifetime);
        }

        void Start()
        {
            this.Projectile = gameObject.AddComponent<DotProjectile>();
            DotProjectile boopCoast = (DotProjectile)this.Projectile;
            boopCoast.init(this, Damage, Piercing, ProjectileLifetime);
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
                    Debug.Log("Value: " + Owner.GetMovementMagnitude());
                    float modified = 1;
                    if (Random.Range(1, 100) < PlayerChargeChance)
                    {
                        modified = Random.Range(ChargeMinMod, ChargeMaxMod);
                    }
                    // bullet_rigidbody.AddRelativeForce(lastMove * (push*modified + Owner.GetCurrentSpeed()), ForceMode.Impulse);
                    bullet_rigidbody.AddRelativeForce(lastMove * (push * 0.125f * (1 + Owner.GetMovementMagnitude()) ), ForceMode.Impulse);
                }
                else
                {
                    // bullet_rigidbody.AddRelativeForce(lastMove * (push + Owner.GetCurrentSpeed()), ForceMode.Impulse);
                    bullet_rigidbody.AddRelativeForce(lastMove * (push * 0.125f * (1 + Owner.GetMovementMagnitude()) ), ForceMode.Impulse);
                }
            }
        }
    }
}