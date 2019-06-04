using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Projectile.Command;

namespace Projectile.Command
{
    public class DotProjectile : IProjectile
    {
        private GameObject DotProjPrefab;

        private Vector3 Force;

        private float Damage;

        private float Piercing;

        private float LifeTime;

        public DotProjectile(GameObject Projectile,
                             float Damage, float Piercing, float LifeTime)
        {
            DotProjPrefab = Projectile;
            this.Damage = Damage;
            this.Piercing = Piercing;
            this.LifeTime = LifeTime;
        }

        public void SetDamage(float Damage, float Piercing)
        {
            this.Damage = Damage;
            this.Piercing = Piercing;
        }

        // Spawn the projectile.
        public GameObject Spawn(Vector3 Location)
        {
            GameObject proj = GameObject.Instantiate(DotProjPrefab, Location, Quaternion.identity);
            GameObject.Destroy(proj, LifeTime);
            return proj;
        }

        // Check for collisions with other objects.
        public void OnCollisionEnter(Collision collision)
        {
            //if(Collider.GameObject == Enemy || Collider.GameObject == Boss)
            GameObject.Destroy(this.DotProjPrefab);
            //and then destroy Enemy or inflict Damage on Boss
        }

        public GameObject GetPrefab()
        {
            return this.DotProjPrefab;
        }
    }
}