// PlayerController.cs
// Nick S.
// Game Logic

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Player Controller
 * 
 * The player controller script that handles user input and stuff
 * 
*/

public class PlayerController : DotObject
{
    [SerializeField] Vector3 SpawnLocation = new Vector3(0, 25, 0);

    /** Script variables **/
    private float modifiedSpeed;
    // Added to track the 3 moves we can use
    private int weaponSelect;

    /** Evolution variables **/
    private float initSpawnOffset;
    private bool GrowFlag = false;

    new private void Start()
    {
        // Call parent class's method.
        base.Start();

        prevPos = SpawnLocation;
        weaponSelect = 1;
        initSpawnOffset = ProjectileSpawnOffset;
        this.color = Color.clear;
        // this.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
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
        if (GetScore() >= 25)
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
        else if (Input.GetButton("Fire2") && shield.GetEnergy() >= 0 && shield.IsReady())
        {
            // Shields on.
            FlameOn();
        }
        else if (Input.GetButton("Fire1") && !shield.IsActve())
        {
            // Fire me matey!
            Shoot(ProjectileSpawnOffset);
        }
        else
        {
            // Shields off.
            FlameOff();
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

}