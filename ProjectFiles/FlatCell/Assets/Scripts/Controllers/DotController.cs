// DotController.cs
// Nick S.
// Game Logic - AI

using UnityEngine;
using DotBehaviour.Command;
using AI.Command;
using Geo.Command;

/*
 * Simple Dot Controller - dot AI that just moves
 * 
 * This is a basic AI controller for a Dot object. I chose to use inheritence
 * (C++ what up) to derive: Geo -> DotObject -> DotController.
 * 
 * This object will have a script attached to it determining it's behaviour,
 * see IDotBehaviour.cs for more information. 
 * 
 * This AI can have one of the follow IDotBehaviours
 * V0.1.0
   - Simple : Move (Default behaviour)
   - Shield : Move, Toggle Shields
   - Shoot  : Move, Toggle Weapon
 
 * All IDotBehaviours set to this object need to be set with init after attaching.
 * For example, from DotSpawner.Spawn()

   // Make a Game Object.
   GameObject Dot = new GameObject("Geo Shooter Dot" + counter);

   // Attach an AI controller.
   IGeo ai = Dot.AddComponent<DotController>();
   ai.init(Speed, MaxHealth, FireRate, FireChance, ShieldChance, EnableTrail);

   // Attach a shooter behaviour and init.
   ShooterDotBehaviour b = Dot.AddComponent<ShooterDotBehaviour>();
   b.init(ai); 

 Public:
   // Removes the currents ai beaviour and updates to the input.
   // @b needs to be initialized with IDotBehaviour.init(IGeo geo)
   void SetBehaviour(IDotBehaviour b)
   
   // If this ai collides with the player or another geo,
   // set velocity to 0.
   void OnCollisionEnter(Collision collision)

 Private

*/

public class DotController : DotObject, IAI
{
    protected GameObject player;

    protected float timer = 0.0f;

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

    public void init(IDotBehaviour behaviour, float Speed, float MaxHP, float FireRate, float FireChance, float ShieldChance, bool ShowTrail = false, bool DrawDebugLine = false)
    {
        if(behaviour != null)
        {
            this.behaviour = behaviour;
        }
        this.DrawDebugLine = DrawDebugLine;
        base.init(Speed, MaxHP, FireRate, FireChance, ShieldChance, ShowTrail);
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
            simple.init(this);
            behaviour = simple;
        }
    }

    new public void Update()
    {
        base.Update();
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