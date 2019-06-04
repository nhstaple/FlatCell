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

        Weapon = new DotWeapon(this, SpawnOffset, FireRate);

        // Add a sphere mesh and collider to the game object.
        SphereCollider volume = gameObject.AddComponent<SphereCollider>();
        volume.radius = 0.5f;
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GetComponent<MeshFilter>().mesh = sphere.GetComponent<MeshFilter>().mesh;
        Destroy(sphere);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
