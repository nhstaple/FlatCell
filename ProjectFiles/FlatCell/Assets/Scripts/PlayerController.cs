﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float Speed = 200.0f;
    [SerializeField] private float BoostFactor = 4.0f;
    [SerializeField] private float Power = 2.0f;
    //Serialized private fields flag a warning as of v2018.3. 
    //This pragma disables the warning in this one case.
    #pragma warning disable 0649
    [SerializeField] private TerrainGenerator GeneratedTerrain;

    private float TrailDecay = 5.0f;
    private float ModifiedSpeed;
    private Vector3 MovementDirection; 
    private TrailRenderer trail;

    //Used for sample projectile shooting test
    public GameObject BulletEmitter;
    public GameObject sampleProjectile;

    public float push;


    void Awake()
    {
        this.transform.position = new Vector3(this.GeneratedTerrain.Width/2, this.GeneratedTerrain.Height/2, this.transform.position.z);
        this.trail = this.GetComponent<TrailRenderer>();
        if(this.GeneratedTerrain == null)
        {
            Debug.Log("You need pass a TrarrainGenerator component to the player.");
            throw new MissingComponentException();
        }
    }

    public float GetCurrentSpeed()
    {
        return this.ModifiedSpeed;
    }

    public Vector3 GetMovementDirection()
    {
        return this.MovementDirection;
    }

    void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            //this.GeneratedTerrain.ChangeTerrainHeight(this.gameObject.transform.position, this.Power);
            Shoot();

        }
        if(Input.GetButton("Fire2"))
        {
            this.GeneratedTerrain.ChangeTerrainHeight(this.gameObject.transform.position, -this.Power);
        }
        if (Input.GetButton("ToggleWeapon"))
        {
            this.GeneratedTerrain.ChangeTerrainHeight(this.gameObject.transform.position, this.Power);
        }
        if (Input.GetButton("ToggleArmor"))
        {
            this.GeneratedTerrain.ChangeTerrainHeight(this.gameObject.transform.position, -this.Power);
        }



        this.ModifiedSpeed = this.Speed;
        if (Input.GetButton("Jump")) 
        {
            this.ModifiedSpeed *= this.BoostFactor;
            this.trail.widthMultiplier = this.BoostFactor;
        }
        else
        {
            if(this.trail.widthMultiplier >= 1.0f) {
                this.trail.widthMultiplier -= Time.deltaTime * this.TrailDecay;
            }
        }
        this.MovementDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        this.gameObject.transform.Translate(this.MovementDirection * Time.deltaTime * this.ModifiedSpeed);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(sampleProjectile, this.transform.position, Quaternion.identity) as GameObject;

        Rigidbody bullet_rigidbody;
        bullet_rigidbody = bullet.GetComponent<Rigidbody>();

        Rigidbody player_rigidbody = this.GetComponent<Rigidbody>();

        //Debug.Log(player_rigidbody.velocity);

        //this.transform.up;

        bullet_rigidbody.AddForce(GetMovementDirection() * push);
        Debug.Log("Fired");
        Debug.Log(bullet_rigidbody);

        Destroy(bullet, 4.0f);//destroy bullet after 3 seconds
    }
}
