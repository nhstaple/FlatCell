/*
 * 
\* DotController.cs
 *
\* Nick S.
\* Game Logic - AI
 *
\* Kyle C.
\* TODO
 *
*/ 

/*
 * TODO
 * Make DotController inhereit from from an AIController class that's independend of the DotObject interface.
*/

using UnityEngine;
using DotBehaviour.Command;
using AI.Command;
using Geo.Command;

public class DotController : DotObject, IAI
{
    protected GameObject player;

    protected float timer = 0.0f;

    protected float initSpeed;
    protected float initDamage;

    protected IDotBehaviour behaviour;

    public void Init(IDotBehaviour behaviour, float Speed, float MaxHP, float FireRate, float FireChance, float ShieldChance, bool ShowTrail = false, bool DrawDebugLine = false)
    {
        base.Init(Speed, MaxHP, FireRate, FireChance, ShieldChance, ShowTrail);
        if (behaviour != null)
        {
            this.behaviour = behaviour;
        }
        this.DrawDebugLine = DrawDebugLine;
        ThawColor();
    }

    new public void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        shield.SetMaxEnergy(InitMaxShieldEnergy);

        initSpeed = Speed;
        initDamage = Damage;

        if(behaviour == null)
        {
            SimpleDotBehaviour simple = gameObject.AddComponent<SimpleDotBehaviour>();
            simple.Init(this);
            behaviour = simple;
        }
    }

    new public void Update()
    {
        base.Update();
    }

    public void SetBehaviour(IDotBehaviour b)
    {
        if (b != null)
        {
            Debug.Log("Set a new behaviour: ");
            behaviour = b;
        }
    }

    new public void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.ToString().Contains("Geo") ||
            collision.gameObject.ToString().Contains("Player"))
        {
            movementDirection = Vector3.zero;
        }

        return;
    }
}