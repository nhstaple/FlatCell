using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Command;
using Geo.Command;

[RequireComponent(typeof(TrailRenderer))]
public class PlayerController : DotObject
{
    /** Player Stats **/
    // The AI spawner
    DotSpawner factory;
    private float spawnCounter = 0.5f;
    private int spawnCount = 10;

    /** Cosmetics **/
    [SerializeField] private float trailDecay = 1f;

    /** Prefabs **/
    [SerializeField] public GameObject PlayerProjectile;

    /** Script variables **/
    private float modifiedSpeed;
    private TrailRenderer trail;
    // Added to track the 3 moves we can use
    private int weaponSelect;
    // Components for a Shield
    [SerializeField] public GameObject sampleShield;
    // gets sampleShield passed into it with instantiation
    private GameObject shield;
    //if shield is on, maybe disable other actions
    private int shieldOn;
    private float ShieldTimer;

    private bool GrowFlag = false;

    new private void Start()
    {
        // Call parent class's method.
        base.Start();
        prevPos = new Vector3(0.0f, 5.0f, 0.0f);
        weaponSelect = 1;
        shieldOn = 0;
        ShieldTimer = 0.0f;
    }

    void Awake()
    {
        transform.position = new Vector3(0, 25, 0);
        this.trail = this.GetComponent<TrailRenderer>();
        factory = new DotSpawner();
        for (int i = 0; i < spawnCount; i++)
        {
            factory.Spawn();
        }
        transform.localScale = new Vector3(25, 25, 25);
    }

    void Update()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        /*
        SpawnCounter += Time.deltaTime;
        if(SpawnCounter >= 0.5)
        {
            Debug.Log("Spawned");
            Factory.Spawn();
            SpawnCounter = 0;
        }
        
        if(transform.localScale.x <= 25 && GrowFlag)
        {
            transform.localScale += new Vector3(0.1f, 0.00f, 0.0f);
            if(transform.localScale.x >= 25)
            {
                GrowFlag = false;
            }
        }
        else if(transform.localScale.x >= 5 && !GrowFlag)
        {
            transform.localScale -= new Vector3(0.1f, 0.00f, 0.0f);
            if (transform.localScale.x <= 5)
            {
                GrowFlag = true;
            }
        }
        */

        if(shield != null && shieldOn == 1)
        {
            Debug.Log("Shield On");
            this.shield.transform.position = this.transform.position;
            ShieldTimer += Time.deltaTime;
        }

        if (ShieldTimer >= 5.0f)
        {
            Destroy(shield);
            ShieldTimer = 0.0f;
            shieldOn = 0;
        }

        if (health == 0)
        {
            Destroy(this);
        }

        if (Input.GetButton("Fire1"))
        {
            // Debug.Log("Fired");
            Shoot(weaponSelect, SpawnOffset);
        }

        if (Input.GetButtonDown("Fire2") && shieldOn == 0)
        {
            Shield();
        }

        if (Input.GetButtonDown("SortWeapon"))
        {
            Debug.Log("Switching Weapons");
            SwitchWeapon();
        }

        modifiedSpeed = Speed;
        if (Input.GetButton("Jump"))
        {
            modifiedSpeed *= BoostFactor;
            trail.widthMultiplier = BoostFactor;
        }

        else
        {
            if (trail.widthMultiplier >= 1.0f)
            {
                trail.widthMultiplier -= Time.deltaTime * trailDecay;
            }
        }
        float step = modifiedSpeed * Time.deltaTime;
        movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        Vector3 old = transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(movementDirection.x, 0, movementDirection.z), step, 0.0f);
        // Move our position a step closer to the target.
        transform.position = Vector3.MoveTowards(transform.position, transform.position + movementDirection * step, step);
        transform.forward = newDir;
        Debug.Log(transform.forward);
    }

    void SwitchWeapon()
    {
        if (weaponSelect == 1)
        {
            weaponSelect = 2;
            Debug.Log("Switching to Weapon 2");
        }
        else if (weaponSelect == 2)
        {
            weaponSelect = 3;
            Debug.Log("Switching to Weapon 3");
        }
        else if (weaponSelect == 3)
        {
            weaponSelect = 1;
            Debug.Log("Switching to Weapon 1");
        }

    }

    void Shield()
    {
        shield = Instantiate(sampleShield, this.transform.position, Quaternion.identity) as GameObject;
        shieldOn = 1;
    }
}