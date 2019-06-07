// DotObject.cs
// Nick S.
// Game Logic - AI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Command;

/*
 * Dot Object
 * 
 * This is a derivation of Geo object.
 * 
*/

public class DotObject : GeoObject
{
    /** Cosmetics **/
    private Mesh DotMesh;

    // Start is called before the first frame update
    new public void Start()
    {
        // Call parent class's method.
        base.Start();

        // Add the weapon.
        DotWeapon dotGun = gameObject.AddComponent<DotWeapon>();  // DotWeapon(this, FireRate);

        dotGun.init(this, DotDamage, DotPiercing, FireRate);
        weapon.Add(dotGun);

        // Add a sphere mesh and collider to the game object.
        SphereCollider volume = gameObject.AddComponent<SphereCollider>();
        volume.radius = 0.5f;
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GetComponent<MeshFilter>().mesh = sphere.GetComponent<MeshFilter>().mesh;
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
