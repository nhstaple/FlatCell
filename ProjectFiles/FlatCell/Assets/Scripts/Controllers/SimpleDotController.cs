using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDotController : DotObject
{
    protected GameObject player; //Player object must have the tag 'Player';

    protected float xMovementDir;
    protected float zMovementDir;
    protected Vector3 MovementDirection;

    protected float timer = 0.0f;

    [SerializeField] protected float DotProjectilePush = 100;
    [SerializeField] protected float DirectionChangeTimer = 5.0f;
    private float initSpeed;
    private float initDamage;

    new public void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        xMovementDir = Random.Range(-1f, 1f);
        zMovementDir = Random.Range(-0.5f, 0.5f);
        movementDirection = new Vector3(xMovementDir, 0.0f, zMovementDir);

        shield.SetMaxEnergy(MaxShieldEnergy);
        initSpeed = Speed;
        initDamage = Damage;
    }
    void Awake()
    {

    }

    new public void Update()
    {
        base.Update();
        const int maxKills = 10;
        const int killWeight = 5;
        if(GetScore() < maxKills)
        {
            Speed = initSpeed + killHistory["Dot"] * killWeight;
            Damage = initDamage + killHistory["Dot"] * 0.1f;
        }
        else
        {
            Speed = initSpeed + maxKills * killWeight;
            Damage = initDamage + maxKills * 0.1f;
        }

        float step = Speed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(movementDirection.x, 0, movementDirection.z), step, 0.0f);
        // Move our position a step closer to the target.
        transform.position = Vector3.MoveTowards(transform.position, transform.position + movementDirection * step, step);
        transform.forward = newDir;

        timer += Time.deltaTime;
        if (timer >= DirectionChangeTimer) //If 2.5 seconds reached - reset timer, change direction
        {
            xMovementDir = Random.Range(-SpawnOffset, SpawnOffset);//Code pulled from DotSpawner
            zMovementDir = Random.Range(-SpawnOffset, SpawnOffset);//change movement direction angle coords
            movementDirection = new Vector3(xMovementDir, 0.0f, zMovementDir);
            timer = 0.0f;
        }
    }

    new public void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.ToString().Contains("Dot") || 
            collision.gameObject.ToString().Contains("Player"))
        {
            movementDirection = new Vector3(0, 0.0f, 0);
        }

        return;
    }
}