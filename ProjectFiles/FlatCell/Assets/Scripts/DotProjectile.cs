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
            // GameObject proj = GameObject.Instantiate(DotProjPrefab, Location, Quaternion.identity);
            GameObject proj = new GameObject("Projectile");
            proj.AddComponent<BoxCollider>();
            MeshFilter filter = proj.AddComponent<MeshFilter>();
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            filter.mesh = cube.GetComponent<MeshFilter>().mesh;
            GameObject.Destroy(cube);
            proj.AddComponent<MeshRenderer>();
            Rigidbody body = proj.AddComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            proj.transform.position = Location;
            GameObject.Destroy(proj, LifeTime);
            return proj;
        }

        public GameObject GetPrefab()
        {
            return this.DotProjPrefab;
        }
    }
}