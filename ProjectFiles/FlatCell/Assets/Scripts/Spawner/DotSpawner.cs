// DotSpawner.cs
// Nick S.
// Game Logic - AI

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;
using Spawner.Command;
using DotBehaviour.Command;
using Pickup.Command;

/*
 * Dot Spawner
 * 
 * Spawns Dots as enemies. The serialized fields are passed to the AI's constructors.
 * 
*/

public class DotSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] public int NumDots = 15;
    [SerializeField] public int ArchetypeCount = 3;
    [SerializeField] public float SpawnOffset = 400f;
    [SerializeField] public Vector3 SpawnLocation = new Vector3(0, 25, 0);
    [SerializeField] public Vector3 InitScale = new Vector3(25, 25, 25);
    [SerializeField] public bool EnableTrail;
    [SerializeField] public float Speed = 50;
    [SerializeField] public float MaxHealth = 3;
    [SerializeField] public float FireRate = 0.05f;
    [SerializeField] public float FireChance = 25;
    [SerializeField] public float ShieldChance = 25;
    [SerializeField] public float Damage = 1;

    private List<GameObject> Alive;
    private int counter = -1;

    public void Start()
    {
        Alive = new List<GameObject>();
    }

    void Update()
    {
        Lottery();

        if(Alive.Count < NumDots)
        {
            Spawn();
        }
    }

    // .1% to kill a random dot
    public void Lottery()
    {
        if (UnityEngine.Random.Range(0, 1000) <= 1 && Alive.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, Alive.Count - 1);
            Kill(Alive[index]);
        }
    }

    public void Spawn()
    {
        counter++;
        Vector3 Location = SpawnLocation;
        Location.x = UnityEngine.Random.Range(-SpawnOffset, SpawnOffset);
        Location.z = UnityEngine.Random.Range(-SpawnOffset, SpawnOffset);

        // Add a random Ai controller to the List
        int res = UnityEngine.Random.Range(1, ArchetypeCount + 1);
        if (res == 1)
        {
            GameObject Dot = new GameObject("Geo Simple Dot" + counter);
            Dot.transform.position = Location;
            Dot.transform.localScale = InitScale;

            IGeo ai = Dot.AddComponent<DotController>();
            ai.init(Speed, MaxHealth, FireRate, FireChance, ShieldChance, EnableTrail);
            Alive.Add(Dot);
        }
        else if(res == 2)
        {
            GameObject Dot = new GameObject("Geo Shooter Dot" + counter);
            Dot.transform.position = Location;
            Dot.transform.localScale = InitScale;

            IGeo ai = Dot.AddComponent<DotController>();
            ai.init(Speed, MaxHealth, FireRate, FireChance, ShieldChance, EnableTrail);
            ShooterDotBehaviour b = Dot.AddComponent<ShooterDotBehaviour>();
            b.init(ai);
            Alive.Add(Dot);
        }
        else
        {
            GameObject Dot = new GameObject("Geo Shield Dot" + counter);
            Dot.transform.position = Location;
            Dot.transform.localScale = InitScale;

            IGeo ai = Dot.AddComponent<DotController>();
            ai.init(Speed, MaxHealth, FireRate, FireChance, ShieldChance, EnableTrail);
            ShieldDotBehaviour b = Dot.AddComponent<ShieldDotBehaviour>();
            b.init(ai);
            Alive.Add(Dot);
        }
    }

    public void Kill(GameObject dead, GameObject killer = null)
    {
        /** Update the killer's score **/
        IGeo geo;
        if (killer != null && (
            dead.ToString().Contains("Dot") ||
            dead.ToString().Contains("Player")))
        {
            if (killer.gameObject.ToString().Contains("Simple Dot"))
            {
                geo = killer.gameObject.GetComponent<DotController>();
                geo.AddKill("Dot");
            }
            else if (killer.gameObject.ToString().Contains("Shield Dot"))
            {
                geo = killer.gameObject.GetComponent<DotController>();
                geo.AddKill("Dot");
            }
            else if (killer.gameObject.ToString().Contains("Shooter Dot"))
            {
                geo = killer.gameObject.GetComponent<DotController>();
                geo.AddKill("Dot");
            }
            else if (killer.gameObject.ToString().Contains("Player"))
            {
                geo = killer.gameObject.GetComponent<PlayerController>();
                geo.AddKill("Dot");
            }
        }

        if (dead.ToString().Contains("Player"))
        {
            IGeo p = dead.GetComponent<PlayerController>();
            p.Respawn();
            Debug.Log("You died! Fool- score: " + p.GetScore());
            return;
        }

        /** Kill the dead object **/
        Alive.Remove(dead);
        if(killer != null)
        {
            Debug.Log(killer.gameObject.ToString() + " killed " + dead.ToString());
        }
        IPickup pickup = dead.GetComponent<PickupObject>();
        if(pickup != null)
        {
            pickup.Spawn(dead.transform.position + dead.transform.forward*10);
        }
        Destroy(dead);
    }
}
