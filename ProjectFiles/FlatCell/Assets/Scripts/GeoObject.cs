using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Command;
using Geo.Command;
using Projectile.Command;
using Spawner.Command;

public class GeoObject : MonoBehaviour, IGeo
{
    /** Geo Stats **/
    [SerializeField] public float Speed;
    [SerializeField] public float BoostFactor;
    [SerializeField] public float MaxHealth;
    [SerializeField] public float FireRate;
    [SerializeField] public float Damage;

    public float health;
    public float armor = 0.0f;
    public float dhieldMana = 0.0f;
    public List<IWeapon> weapon;

    /** Cosemetics **/
    // The force applied to the projectile. 
    [SerializeField] private float Push = 100.0f;
    [SerializeField] public float trailDecay = 1f;

    /** Script variables **/
    public TrailRenderer trail;
    public Vector3 lastMovement;
    public Vector3 movementDirection;
    public float currentSpeed;
    public Vector3 prevPos;
    public bool killedByPlayer = false;

    public Color color;
    private Renderer renderer;
    const float colorRefreshPoll = 0.5f;
    public float refreshCounter = 0;

    // Start is called before the first frame update
    public void Start()
    {
        weapon = new List<IWeapon>();

        // Add components to game object.
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        // Init values. 
        currentSpeed = 0;
        health = MaxHealth;

        transform.forward = new Vector3(1, 0, 0);

        // Set material
        color = Color.clear;

        var res = Random.Range(1, 100);
        if (1 <= res && res <= 33)
        {
            color = Color.red;
        }
        else if (res > 33 && res < 66)
        {
            color = Color.blue;
        }
        else
        {
            color = Color.green;
        }

        renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = Instantiate(Resources.Load("Geo Mat", typeof(Material)) as Material);
        renderer.material.color = this.color;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        trail = gameObject.AddComponent<TrailRenderer>();
        trail.startColor = this.color;
        trail.endColor = Color.white;
        trail.material = new Material(Resources.Load("TrailShader", typeof(Shader)) as Shader);
        trail.enabled = true;
        trail.time = 1.0f;
    }

    public void FixedUpdate()
    {
        currentSpeed = (transform.position - prevPos).magnitude / Time.deltaTime;
        prevPos = this.transform.position;
    }

    // Update is called once per frame
    public void Update()
    {
        refreshCounter += Time.deltaTime;
        if (health <= 0)
        {
            GameObject spawner = GameObject.FindWithTag("DotSpawner");
            ISpawner controller = spawner.GetComponent<DotSpawner>();
            controller.Kill(this.gameObject, killedByPlayer);
        }
        if (refreshCounter >= colorRefreshPoll)
        {
            renderer.material.color = this.color;
            trail.startColor = this.color;
            trail.endColor = Color.black;
            refreshCounter = 0;
        }
    }

    /** IGeo methods **/
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public Vector3 GetMovementDirection()
    {
        return movementDirection;
    }

    public Color GetColor()
    {
        return color;
    }

    public void Shoot(int WeaponIndex = 0, float SpawnOffset = 20)
    {
        if(weapon.Count > 1 && WeaponIndex < weapon.Count)
        {
            weapon[WeaponIndex].Fire(transform.forward, transform.position, Push, SpawnOffset);
        }
        else if(weapon.Count == 1)
        {
            weapon[0].Fire(transform.forward, transform.position, Push, SpawnOffset);
        }
    }

    public Vector3 GetForward()
    {
        return transform.forward;
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("collided with ");
        Debug.Log(collision.gameObject.ToString());

        if (collision.gameObject.ToString().Contains("Projectile"))
        {
            Debug.Log("interface match");
            Destroy(collision.gameObject, .1f);
            ProjectileObject bullet = collision.gameObject.GetComponent<ProjectileObject>();
            health -= bullet.GetDamage();
            if(health <= 0 && collision.gameObject.ToString().Contains("Player"))
            {
                killedByPlayer = true;
            }
        }
                
        return;
    }
}
