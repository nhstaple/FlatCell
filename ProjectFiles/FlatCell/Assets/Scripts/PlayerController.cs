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
    [SerializeField]
    public GameObject sampleProjectile;

    public float push;

    private float currentSpeed;
    private Vector3 prevPos;

    [SerializeField]
    private float FireRate = 0.25f;
    [SerializeField]
    private float SpawnOffset = 10f;
    private float ShootCounter;
    private void Start()
    {
        prevPos = new Vector3(0.0f, 0.0f, 0.0f);
        currentSpeed = 0;
        ShootCounter = 0;
    }

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

    private void FixedUpdate()
    {
        currentSpeed = (transform.position - prevPos).magnitude / Time.deltaTime;
        prevPos = this.transform.position;
    }

    void Update()
    {
        Debug.Log(currentSpeed);
        if (Input.GetButton("Fire1"))
        {
            Debug.Log("Fired");
            //this.GeneratedTerrain.ChangeTerrainHeight(this.gameObject.transform.position, this.Power);
            Shoot();

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

    void Shoot()
    {
        ShootCounter += Time.deltaTime;
        if(ShootCounter >= this.FireRate)
        {
            Vector3 spawnLoc = transform.position + lastMove*SpawnOffset;

            GameObject bullet = Instantiate(sampleProjectile, spawnLoc, Quaternion.identity) as GameObject;

            Rigidbody bullet_rigidbody;
            bullet_rigidbody = bullet.GetComponent<Rigidbody>();
            //        bullet_rigidbody.AddRelativeForce()

            Rigidbody player_rigidbody = GetComponent<Rigidbody>();
            //Debug.Log(player_rigidbody.velocity);
            //this.transform.up;

            // bullet_rigidbody.AddRelativeForce(lastMove * (push + currentSpeed * ImpulseModifier), ForceMode.Impulse);
            bullet_rigidbody.AddRelativeForce((lastMove) * (push), ForceMode.Impulse);
            Debug.Log("Fired");
            Debug.Log(bullet_rigidbody);

            Destroy(bullet, 4.0f);//destroy bullet after 3 seconds
            ShootCounter = 0.0f;
        }
        if (GetMovementDirection().magnitude > 0)
        {
            lastMove = GetMovementDirection();
            if (lastMove.x > 0 && lastMove.z == 0) { lastMove.x = 1; }
            if (lastMove.x < 0 && lastMove.z == 0) { lastMove.x = -1; }
            if (lastMove.z > 0 && lastMove.x == 0) { lastMove.z = 1; }
            if (lastMove.z < 0 && lastMove.x == 0) { lastMove.z = -1; }
            Debug.Log(lastMove);
        }
    }
}