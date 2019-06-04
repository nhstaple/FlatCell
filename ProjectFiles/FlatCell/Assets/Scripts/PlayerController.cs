using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Command;
using Geo.Command;

[RequireComponent(typeof(TrailRenderer))]
public class PlayerController : MonoBehaviour, IGeo
{
    /** Player Stats **/
    [SerializeField] private float Speed = 100.0f;
    [SerializeField] private float BoostFactor = 4.0f;
    [SerializeField] private float MaxHealth = 10.0f;
    [SerializeField] private float FireRate = 0.25f;
    // The player's current HP.
    private float Health;
    // The player's armor.
    private float Armor = 0.0f;
    // The player's shield mana. Ie, burns through when active and recharges when inactive.
    private float ShieldMana = 0.0f;
    // The player's weapon. 
    private IWeapon Weapon;

    /** Cosmetics **/
    [SerializeField] private float TrailDecay = 2.5f;
    [SerializeField] private float SpawnOffset = 10f;
    // The force applied to the projectile. 
    [SerializeField] private float push = 100.0f;

    /** Prefabs **/
    [SerializeField] public GameObject PlayerProjectile;

    /** Script variables **/
    private Vector3 lastMove;
    private float ModifiedSpeed;
    private Vector3 MovementDirection;
    private TrailRenderer trail;
    private float currentSpeed;
    private Vector3 prevPos;
    // Added to track the 3 moves we can use
    private int weaponSelect;
    // Components for a Shield
    [SerializeField] public GameObject sampleShield;
    // gets sampleShield passed into it with instantiation
    private GameObject shield;
    //if shield is on, maybe disable other actions
    private int shieldOn;
    private float ShieldTimer;

    private void Start()
    {
        prevPos = new Vector3(0.0f, 0.0f, 0.0f);
        currentSpeed = 0;
        this.Weapon = new DotWeapon(this, PlayerProjectile, SpawnOffset, FireRate);
        weaponSelect = 1;
        shieldOn = 0;
        ShieldTimer = 0.0f;
        Health = MaxHealth;
    }

    void Awake()
    {
        transform.position = new Vector3(0, 5, 0);
        this.trail = this.GetComponent<TrailRenderer>();
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public Vector3 GetMovementDirection()
    {
        return MovementDirection;
    }

    private void FixedUpdate()
    {
        currentSpeed = (transform.position - prevPos).magnitude / Time.deltaTime;
        prevPos = this.transform.position;
    }

    void Update()
    {
        //Debug.Log(currentSpeed);
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

        if (Health == 0)
        {
            Destroy(this);
        }

        if (Input.GetButton("Fire1"))
        {
            Debug.Log("Fired");
            if (weaponSelect == 1)
            {
                this.Weapon.Fire(GetMovementDirection(), transform.position, push);
            }
        }

        if(Input.GetButtonDown("Fire2") && shieldOn == 0)
        {
            Shield();
        }

        if (Input.GetButtonDown("SortWeapon"))
        {
            Debug.Log("Switching Weapons");
            SwitchWeapon();
        }


        ModifiedSpeed = Speed;
        if (Input.GetButton("Jump"))
        {
            ModifiedSpeed *= BoostFactor;
            trail.widthMultiplier = BoostFactor;
        }
        else
        {
            if (trail.widthMultiplier >= 1.0f)
            {
                trail.widthMultiplier -= Time.deltaTime * TrailDecay;
            }
        }
        MovementDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        gameObject.transform.Translate(MovementDirection * Time.deltaTime * ModifiedSpeed);
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

    public void Shoot()
    {
        return;
    }
}