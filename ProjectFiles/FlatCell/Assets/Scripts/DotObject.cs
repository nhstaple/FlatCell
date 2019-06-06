using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Geo.Command;
using Weapon.Command;
using Projectile.Command;

public class DotObject : GeoObject
{
    /** Cosmetics **/
    [SerializeField] public float SpawnOffset = 20f;
    private Mesh DotMesh;
    public Color color;
    private Renderer renderer;
    const float colorRefreshPoll = 0.5f;
    private float refreshCounter = 0;
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
        Rigidbody body = base.gameObject.AddComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;

        // Set material
        color = Color.clear;

        var res = Random.Range(1, 100);
        if(1 <= res && res <= 33)
        {
            color = Color.red;
        } else if(res > 33 && res < 66)
        {
            color = Color.blue;
        } else
        {
            color = Color.green;
        }

        renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = Instantiate(Resources.Load("Geo Mat", typeof(Material)) as Material);
        renderer.material.color = this.color;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    // Update is called once per frame
    new public void Update()
    {
        base.Update();
        refreshCounter += Time.deltaTime;
        if(refreshCounter >= colorRefreshPoll)
        {
            renderer.material.color = this.color;
            refreshCounter = 0;
        }
    }

}
