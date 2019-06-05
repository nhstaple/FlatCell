using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Geo.Command;
using Weapon.Command;
using Projectile.Command;

public class DotObject : GeoObject
{
    /** Cosmetics **/
    [SerializeField] public float SpawnOffset = 10f;
    private Mesh DotMesh;

    // Start is called before the first frame update
    new public void Start()
    {
        // Call parent class's method.
        base.Start();

        // Add the weapon.
        DotWeapon dotGun = new DotWeapon(this, SpawnOffset, FireRate);
        weapon.Add(dotGun);

        // Add a sphere mesh and collider to the game object.
        SphereCollider volume = gameObject.AddComponent<SphereCollider>();
        volume.radius = 0.5f;
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GetComponent<MeshFilter>().mesh = sphere.GetComponent<MeshFilter>().mesh;
        Destroy(sphere);

        // Set physics constraints
        Debug.Log(base.gameObject.ToString());
        Rigidbody body = base.gameObject.AddComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
    }

    // Update is called once per frame
    new public void Update()
    {
        base.Update();
    }

}
