using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Command;
using Geo.Command;

public class PlayerController : DotObject
{
    /** Player Stats **/
    // The AI spawner
    DotSpawner factory;
    private float spawnCounter = 0.5f;
    private int spawnCount = 10;
    public Dictionary<string, int> killHistory;

    /** Prefabs **/
    [SerializeField] public GameObject PlayerProjectile;

    /** Script variables **/
    private float modifiedSpeed;
    // private TrailRenderer trail;
    // Added to track the 3 moves we can use
    private int weaponSelect;
    //if shield is on, maybe disable other actions
    private int shieldOn;
    public float initSpawnOffset;

    private bool GrowFlag = false;

    new private void Start()
    {
        // Call parent class's method.
        base.Start();
        prevPos = new Vector3(0.0f, 5.0f, 0.0f);
        weaponSelect = 1;
        shieldOn = 0;
        shieldMana = MAXSHIELDMANA;
        killHistory = new Dictionary<string, int>();
        killHistory.Add("Dot", 0);
        initSpawnOffset = SpawnOffset;
        // this.color = Color.clear;
        this.color = Color.grey;
        trail.enabled = true;
    }

    void Awake()
    {
        transform.position = new Vector3(0, 25, 0);
        transform.localScale = new Vector3(25, 25, 25);
    }

    new void Update()
    {
        base.Update();
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        /*
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
            
        // Check for incremental evolution.
        if (killHistory["Dot"] >= 25)
        {
            transform.localScale = new Vector3(50, 1, 1);
            SpawnOffset = initSpawnOffset + 15;
        }
        else
        {
            transform.localScale = new Vector3(25 + killHistory["Dot"], 25 - killHistory["Dot"], 25 - killHistory["Dot"]);
            SpawnOffset = initSpawnOffset + (int)killHistory["Dot"];
        }
        */

        modifiedSpeed = Speed;
        if (Input.GetButton("Jump"))
        {
            modifiedSpeed *= BoostFactor;
            trail.widthMultiplier = BoostFactor;
        }
        else if (Input.GetButton("Fire2") && shieldMana >= 0 && shieldReady)
        { 
            Shield();
        }
        else if (Input.GetButton("Fire1") && !shieldActive)
        {
            // Debug.Log("Fired");
            Shoot(weaponSelect, SpawnOffset);
        }
        else
        {
            shieldActive = false;
            if(shieldMana <= 0)
            {
                shieldReady = false;
            }
            shieldMana += Time.deltaTime;
            if (shieldMana >= MAXSHIELDMANA)
            {
                shieldMana = MAXSHIELDMANA;
                shieldReady = true;
            }
            renderer.material.DisableKeyword("_EMISSION");
            if (trail.widthMultiplier >= 1.0f)
            {
                trail.widthMultiplier -= Time.deltaTime * trailDecay;
            }
        }

        if (Input.GetButtonDown("SortWeapon"))
        {
            Debug.Log("Switching Weapons");
            SwitchWeapon();
        }

        float step = modifiedSpeed * Time.deltaTime;
        movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        Vector3 old = transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(movementDirection.x, 0, movementDirection.z), step, 0.0f);
        // Move our position a step closer to the target.
        transform.position = Vector3.MoveTowards(transform.position, transform.position + movementDirection * step, step);
        transform.forward = newDir;
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
        renderer.material.EnableKeyword("_EMISSION");
        shieldMana -= Time.deltaTime;
        shieldActive = true;
    }
}