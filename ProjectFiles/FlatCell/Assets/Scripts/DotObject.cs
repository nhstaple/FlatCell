using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Geo.Command;
using Weapon.Command;
using Projectile.Command;

public class DotObject : MonoBehaviour, IGeo
{
    /** Geo Stats **/
    [SerializeField] public float Speed = 100.0f;
    [SerializeField] public float BoostFactor = 4.0f;
    [SerializeField] public float MaxHealth = 10.0f;
    [SerializeField] public float FireRate = 0.25f;

    public float Health;
    public float Armor = 0.0f;
    public float ShieldMana = 0.0f;
    public IWeapon Weapon;

    /** Cosmetics **/
    [SerializeField] public float SpawnOffset = 10f;
    private Mesh DotMesh;

    /** Prefabs **/
    private GameObject DotProjectile;

    /** Script variables **/
    public Vector3 lastMove;
    public Vector3 MovementDirection;
    public float currentSpeed;
    public Vector3 prevPos;

    void SetProjectile(GameObject Prefab)
    {
        DotProjectile = Prefab;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = 0;
        this.Weapon = new DotWeapon(this, DotProjectile, SpawnOffset, FireRate);
        Health = MaxHealth;

        gameObject.AddComponent<MeshFilter>();
        // Add a sphere.
        //gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        SphereCollider volume = gameObject.AddComponent<SphereCollider>();
        volume.radius = 0.5f;
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GetComponent<MeshFilter>().mesh = sphere.GetComponent<MeshFilter>().mesh;
        Destroy(sphere);
    }

    private void FixedUpdate()
    {
        currentSpeed = (transform.position - prevPos).magnitude / Time.deltaTime;
        prevPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Vector3 Direction, Vector3 Location, float Force)
    {
        this.Weapon.Fire(Direction, Location, Force);
        return;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    // Check for collisions with other objects.
    public void OnCollisionEnter(Collision collision)
    {
        /*
        Debug.Log("collided with ");
        Debug.Log(collision.gameObject.transform.parent.gameObject.ToString());
        
        bool IsProjectile = typeof(IProjectile).IsAssignableFrom(collision.gameObject.transform.parent.gameObject.GetType());
        if(IsProjectile)
        {
            Debug.Log("interface match");
            Destroy(collision.collider);
        }
        */
        return;
    }
}
