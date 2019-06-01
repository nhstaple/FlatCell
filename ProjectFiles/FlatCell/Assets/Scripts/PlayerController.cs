using System.Collections;
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

    private Vector3 lastMove;
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
        transform.position = new Vector3(0, 40, 0);
        trail = GetComponent<TrailRenderer>();
        //        if (GeneratedTerrain == null)
        //        {
        //            Debug.Log("You need pass a TrarrainGenerator component to the player.");
        //            throw new MissingComponentException();
        //        }
    }

    public float GetCurrentSpeed()
    {
        return ModifiedSpeed;
    }

    public Vector3 GetMovementDirection()
    {
        return MovementDirection;
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            //this.GeneratedTerrain.ChangeTerrainHeight(this.gameObject.transform.position, this.Power);
            Shoot();

        }
        //        if (Input.GetButton("Fire2"))
        //       {
        //           GeneratedTerrain.ChangeTerrainHeight(gameObject.transform.position, -Power);
        //       }
        //        if (Input.GetButton("ToggleWeapon"))
        //        {
        //            GeneratedTerrain.ChangeTerrainHeight(gameObject.transform.position, Power);
        //        }
        //        if (Input.GetButton("ToggleArmor"))
        //        {
        //            GeneratedTerrain.ChangeTerrainHeight(gameObject.transform.position, -Power);
        //        }



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

    void Shoot()
    {
        GameObject bullet = Instantiate(sampleProjectile, transform.position, Quaternion.identity) as GameObject;

        Rigidbody bullet_rigidbody;
        bullet_rigidbody = bullet.GetComponent<Rigidbody>();
        //        bullet_rigidbody.AddRelativeForce()

        Rigidbody player_rigidbody = GetComponent<Rigidbody>();
        //Debug.Log(player_rigidbody.velocity);
        //this.transform.up;
        if (GetMovementDirection().magnitude > 0)
        {
            lastMove = GetMovementDirection();
        }

        bullet_rigidbody.AddRelativeForce(lastMove * push, ForceMode.Impulse);
        Debug.Log("Fired");
        Debug.Log(bullet_rigidbody);

        Destroy(bullet, 4.0f);//destroy bullet after 3 seconds
    }
}