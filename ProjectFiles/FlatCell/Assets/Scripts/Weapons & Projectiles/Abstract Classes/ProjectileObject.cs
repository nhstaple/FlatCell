// ProjectileObject.cs
// Nick S.
// Game Logic - Combat

using UnityEngine;
using Weapon.Command;
using Geo.Command;
using Utils.Vectors;

/*
 * Dot Projectile 
*/
namespace Projectile.Command
{
    public class ProjectileObject : MonoBehaviour, IProjectile
    {
        /** Projectile Stats **/
        [SerializeField] protected float Damage = 1;
        [SerializeField] protected float Piercing = 0;
        [SerializeField] protected float LifeTime = 2.5f;

        /** Script variables **/
        protected float counter = -1;

        // The gun that this bullet was fired from
        public IWeapon Owner;

        // Pointer to a renderer
        protected Renderer rend;

        public void Init(IWeapon owner, float Damage, float Piercing, float ProjectileLifetime)
        {
            this.Owner = owner;
            this.Damage = Damage;
            this.Piercing = Piercing;
            this.LifeTime = ProjectileLifetime;
        }

        public void Start()
        {
        }

        public void Update()
        {
        }

        public void SetDamage(float Damage, float Piercing)
        {
            this.Damage = Damage;
            this.Piercing = Piercing;
        }

        // Spawn the projectile.
        public GameObject Spawn(Vector3 Location)
        {
            counter++;
            IGeo owner = Owner.GetOwner();

            // Create a new projectile object.
            GameObject proj = new GameObject(owner.ToString() + " Projectile " + counter);

            // Add mesh components
            rend = proj.AddComponent<MeshRenderer>();
            MeshFilter filter = proj.AddComponent<MeshFilter>();
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            filter.mesh = cube.GetComponent<MeshFilter>().mesh;
            GameObject.Destroy(cube);

            // Add collision components and set rotation constraints.
            proj.AddComponent<BoxCollider>();
            Rigidbody body = proj.AddComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            proj.transform.position = Location;

            // Set the size/
            proj.transform.localScale = Scales.DotProjScale;

            rend.material = GameObject.Instantiate(Resources.Load("Geo Mat", typeof(Material)) as Material);
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            if (Owner.GetOwner().GetColor().maxColorComponent > 100)
            {
                rend.material.color = Owner.GetOwner().GetColor();
            }
            else
            {
                rend.material.color = Color.white;
            }

            // Destroys projectile after LifeTime seconds.
            GameObject.Destroy(proj, LifeTime);
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

        public float GetPiercing()
        {
            return this.Piercing;
        }

        public void SetLifeTime(float time)
        {
            this.LifeTime = time;
        }

        public float GetLifeTime()
        {
            return this.LifeTime;
        }

        public void SetOwner(IWeapon w)
        {
            Owner = w;
        }

    }
}