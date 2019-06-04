using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Geo.Command;
using Projectile.Command;

namespace Weapon.Command
{
    public class DotWeapon : IWeapon
    {
        /** Weapon Stats **/
        private float FireRate;
        private float Damage = 3.0f;
        private float Piercing = 0.0f;
        private float ProjectileLifetime = 2.5f;
        private IProjectile Projectile;

        /** Cosmetics **/
        private float SpawnOffset;

        /** Script variables **/
        // The owner of the weapon
        private IGeo Owner;
        // Keeps track of the player's last input values.
        private Vector3 lastMove;
        // Keeps track of the last time the weapon was shot.
        private float ShootCounter;

        public DotWeapon(IGeo GeoOwner, GameObject ProjectilePrefab, float Offset, float Rate)
        {
            this.Owner = GeoOwner;
            this.Projectile = new DotProjectile(ProjectilePrefab, Damage, Piercing, ProjectileLifetime);
            this.SpawnOffset = Offset;
            this.FireRate = Rate;
        }

        private void Start()
        {
            ShootCounter = 0;
        }

        public void SetDamage(float Damage, float Piercing)
        {
            this.Damage = Damage;
            this.Piercing = Piercing;
        }

        public void Fire(Vector3 movementDir, Vector3 pos, float push)
        {
            ShootCounter += Time.deltaTime;
            if (ShootCounter >= this.FireRate)
            {
                ShootCounter = 0.0f;
                Vector3 spawnLoc = pos + lastMove * SpawnOffset;
                GameObject bullet = Projectile.Spawn(spawnLoc);
                Rigidbody bullet_rigidbody;
                bullet_rigidbody = bullet.GetComponent<Rigidbody>();
                bullet_rigidbody.AddRelativeForce(lastMove * (push + Owner.GetCurrentSpeed()), ForceMode.Impulse);
            }
            if (movementDir.magnitude > 0)
            {
                lastMove = movementDir;
                if (lastMove.x > 0 && lastMove.z == 0) { lastMove.x = 1; }
                if (lastMove.x < 0 && lastMove.z == 0) { lastMove.x = -1; }
                if (lastMove.z > 0 && lastMove.x == 0) { lastMove.z = 1; }
                if (lastMove.z < 0 && lastMove.x == 0) { lastMove.z = -1; }
                Debug.Log(lastMove);
            }

            return;
        }
    }
}