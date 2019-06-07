// SimpleDotController.cs
// Nick S. & Kyle C
// Game Logic - AI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Simple Dot Controller
 * 
 * This is a basic AI controller for a Dot object. All this AI does is move.
 * This AI can:
   - move

 * TODO comment
*/

public class SimpleDotController : DotObject
{
    protected GameObject player;

    protected float xMovementDir;
    protected float zMovementDir;
    protected Vector3 MovementDirection;

    protected float timer = 0.0f;

    [SerializeField] protected float DotProjectilePush = 100;
    [SerializeField] protected float DirectionChangeTimer = 1f;
    [SerializeField] protected float DirectionChangeWeight = 10;
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

    new public void Update()
    {
        base.Update();

        timer += Time.deltaTime;
        CheckScore();
        Move();
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

    void CheckScore()
    {
        const int maxKills = 10;
        const int killWeight = 5;
        if (GetScore() < maxKills)
        {
            Speed = initSpeed + killHistory["Dot"] * killWeight;
            Damage = initDamage + killHistory["Dot"] * 0.1f;
        }
        else
        {
            Speed = initSpeed + maxKills * killWeight;
            Damage = initDamage + maxKills * 0.1f;
        }
    }

    void Move()
    {
        float step = Speed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(movementDirection.x, 0, movementDirection.z), step, 0.0f);
        // Move our position a step closer to the target.
        transform.position = Vector3.MoveTowards(transform.position, transform.position + movementDirection * step, step);
        transform.forward = newDir;

        if (timer >= DirectionChangeTimer)
        {
            timer = 0.0f;
            movementDirection = new Vector3(Random.Range(-1, 1) * DirectionChangeWeight,
                                            0.0f,
                                            Random.Range(-1, 1) * DirectionChangeWeight);
        }
    }
}