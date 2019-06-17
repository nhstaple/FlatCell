/*
 * 
\* DotObject.cs
 *
\* Nick S.
\* Game Logic - AI
 *
\* Brian
\* TODO
 * 
\* Megan
\* TODO
 * 
*/

using UnityEngine;
using Weapon.Command;

/*
 * Dot Object
 * 
 * This is a derivation of Geo object.
 * 
*/

namespace Geo.Command
{

    public class DotObject : GeoObject
    {
        const string Dot_Shoot_Soundfile = "Audio/Shoot Sound";

        /** Cosmetics **/
        private Mesh DotMesh;

        // Start is called before the first frame update
        new public void Start()
        {
            // Call parent class's method.
            base.Start();

            if (ActiveGun == null)
            {
                // Add the weapon.
                ActiveGun = Instantiate(Resources.Load("Player Items/Dot Gun")) as GameObject;
                DotWeapon dotGun = ActiveGun.GetComponents<IWeapon>()[0] as DotWeapon;
                dotGun.Init(this, Resources.Load<AudioClip>(Dot_Shoot_Soundfile), DotDamage, DotPiercing, FireRate);
                weapon.Add(dotGun);
            }

            // Add a sphere mesh and collider to the game object.
            // SphereCollider volume = gameObject.AddComponent<SphereCollider>();
            // volume.radius = 0.5f;
            MeshCollider coll = gameObject.AddComponent<MeshCollider>();
            coll.convex = true;
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var mesh = sphere.GetComponent<MeshFilter>().mesh;
            GetComponent<MeshFilter>().mesh = mesh;
            coll.sharedMesh = mesh;
            Destroy(sphere);

            // Set physics constraints
            Rigidbody body = base.gameObject.AddComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            trail.enabled = EnableTrail;
        }

        // Update is called once per frame
        new public void Update()
        {
            base.Update();
        }

    }
}