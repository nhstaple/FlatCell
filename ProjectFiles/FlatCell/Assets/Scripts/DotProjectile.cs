using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Projectile.Command;

namespace Projectile.Command
{
    public class DotProjectile : IProjectile
    {
        private Vector3 Force;

        private float Damage;

        private float Piercing;

        private float LifeTime;

        private float Counter = -1;

        public DotProjectile(float Damage, float Piercing, float LifeTime)
        {
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
            Counter++;
            // Create a new projectile object.
            GameObject proj = new GameObject("Projectile" + Counter);

            // Add mesh components
            proj.AddComponent<MeshRenderer>();
            MeshFilter filter = proj.AddComponent<MeshFilter>();
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            filter.mesh = cube.GetComponent<MeshFilter>().mesh;
            GameObject.Destroy(cube);

            // Add collision components and set rotation constraints.
            proj.AddComponent<BoxCollider>();
            Rigidbody body = proj.AddComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            proj.transform.position = Location;

            // Destroys projectile after LifeTime seconds.
            GameObject.Destroy(proj, LifeTime);

            // Set the size/
            proj.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

            // For damage calc
            ProjectileObject bullet = proj.AddComponent<ProjectileObject>();
            bullet.SetDamage(Damage, Piercing);
            bullet.SetLifeTime(LifeTime);
            return proj;
        }
    }
}