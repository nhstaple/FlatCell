// DotController.cs
// Nick S. & Kyle C
// Game Logic - AI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotBehaviour.Command;
using Pickup.Command;

/*
 * Simple Dot Controller
 * 
 * This is a basic AI controller for a Dot object. All this AI does is move.
 * This AI can:
   - move

 * TODO comment
*/

public class DotController : DotObject
{
    protected GameObject player;

    protected float xMovementDir;
    protected float zMovementDir;
    protected Vector3 MovementDirection;

    protected float timer = 0.0f;

    [SerializeField] protected float DirectionChangeTimer = 1f;
    [SerializeField] protected float DirectionChangeWeight = 10;
    protected float initSpeed;
    protected float initDamage;

    protected IDotBehaviour behaviour;

    public void SetBehaviour(IDotBehaviour b)
    {
        if(b != null)
        {
            Debug.Log("Set a new behaviour: ");
            behaviour = b;
        }
    }

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

        if(gameObject.GetComponent<ShieldDotBehaviour>())
        {
            var cast = (ShieldDotBehaviour)gameObject.GetComponent<ShieldDotBehaviour>();
            behaviour = cast;
            behaviour.init(this);
        }
        else if (gameObject.GetComponent<ShooterDotBehaviour>())
        {
            var cast = (ShooterDotBehaviour)gameObject.GetComponent<ShooterDotBehaviour>();
            behaviour = cast;
            behaviour.init(this);
        }
        else
        {
            behaviour = gameObject.AddComponent<SimpleDotBehaviour>();
            behaviour.init(this);
        }
    }

    new public void Update()
    {
        base.Update();
        if(behaviour != null)
        {
            if (behaviour.GetType() == "SimpleDot")
            {
                SimpleDotBehaviour b = (SimpleDotBehaviour)behaviour;
                b.exec();
            }
            else if (behaviour.GetType() == "ShieldDot")
            {
                ShieldDotBehaviour b = (ShieldDotBehaviour)behaviour;
                b.exec();
            }
            else if (behaviour.GetType() == "ShooterDot")
            {
                ShooterDotBehaviour b = (ShooterDotBehaviour)behaviour;
                b.exec();
            }
        }
    }

    new public void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.ToString().Contains("Dot") ||
            collision.gameObject.ToString().Contains("Player"))
        {
            movementDirection = Vector3.zero;
        }

        return;
    }
}