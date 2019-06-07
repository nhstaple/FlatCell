using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Projectile.Command;
using Weapon.Command;
using Geo.Command;

namespace Projectile.Command
{
    public class DotProjectile : IProjectile
    {
        private float Damage;

        private float Piercing;

        private float LifeTime;

        private float Counter = -1;

        public IWeapon Owner;

        private Renderer renderer;

        public DotProjectile(IWeapon gun, float Damage, float Piercing, float LifeTime)
        {
            this.Owner = gun;
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
            IGeo owner = Owner.GetOwner();

            // Create a new projectile object.
            GameObject proj = new GameObject(owner.ToString() + " Projectile " + Counter);

            // Add mesh components
            renderer = proj.AddComponent<MeshRenderer>();
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
            bullet.SetOwner(Owner);

            renderer.material = GameObject.Instantiate(Resources.Load("Geo Mat", typeof(Material)) as Material);
            renderer.material.color = Owner.GetOwner().GetColor();
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            return proj;
        }

        public IWeapon GetOwner()
        {
            return Owner;
        }

        public float GetDamage()
        {
            return Damage;
        }
    }
}