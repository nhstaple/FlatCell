using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

using Geo.Command;
using Spawner.Command;

public class DotSpawner : MonoBehaviour, ISpawner
{
    private List<GameObject> Alive = new List<GameObject>();
    private int counter = -1;

    [SerializeField] public int NumAi = 15;
    [SerializeField] public float SpawnOffset = 250f;
    [SerializeField] public Vector3 SpawnLocation = new Vector3(0, 25, 0);
    [SerializeField] public bool EnableTrail;
    [SerializeField] public float Speed = 50;
    [SerializeField] public float MaxHealth = 3;
    [SerializeField] public float FireRate = 0.05f;
    [SerializeField] public float FireChance = 25;
    [SerializeField] public float ShieldChance = 25;
    [SerializeField] public float Damage = 1;

    GameObject player;

    public void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if(Alive.Count < NumAi)
        {
            Spawn();
        }
        /*
        // .1% to kill a random dot
        if(UnityEngine.Random.Range(0, 1000) <= 1 && Alive.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, Alive.Count - 1);
            Kill(Alive[index]);
        }*/
    }

    public void Spawn()
    {
        counter++;
        Vector3 Location = SpawnLocation;
        Location.x = UnityEngine.Random.Range(-SpawnOffset, SpawnOffset);
        Location.z = UnityEngine.Random.Range(-SpawnOffset, SpawnOffset);

        // Add a random Ai controller to the List
        IGeo ai;
        int res = UnityEngine.Random.Range(1, 3 + 1);
        if (res == 1)
        {
            GameObject Dot = new GameObject("SimpleDot" + counter);
            Dot.transform.position = Location;
            Dot.transform.localScale = new Vector3(25, 25, 25);
            ai = Dot.AddComponent<SimpleDotController>();
            ai.init(Speed, MaxHealth, FireRate, FireChance, ShieldChance, EnableTrail);
            Alive.Add(Dot);
        }
        else if(res == 2)
        {
            GameObject Dot = new GameObject("ShooterDot" + counter);
            Dot.transform.position = Location;
            Dot.transform.localScale = new Vector3(25, 25, 25);
            ai = Dot.AddComponent<ShooterDotController>();
            ai.init(Speed, MaxHealth, FireRate, FireChance, ShieldChance, EnableTrail);
            Alive.Add(Dot);
        }
        else
        {
            GameObject Dot = new GameObject("ShieldDot" + counter);
            Dot.transform.position = Location;
            Dot.transform.localScale = new Vector3(25, 25, 25);
            ai = Dot.AddComponent<ShieldDotController>();
            ai.init(Speed, MaxHealth, FireRate, FireChance, ShieldChance, EnableTrail);
            Alive.Add(Dot);
        }
    }

    public void Kill(GameObject dead, GameObject killer)
    {
        /** Update the killer's score **/
        IGeo geo;
        if (dead.ToString().Contains("Dot") ||
            dead.ToString().Contains("Player"))
        {
            if (killer.gameObject.ToString().Contains("SimpleDot"))
            {
                geo = killer.gameObject.GetComponent<SimpleDotController>();
                geo.AddKill("Dot");
            }
            else if (killer.gameObject.ToString().Contains("ShieldDot"))
            {
                geo = killer.gameObject.GetComponent<ShieldDotController>();
                geo.AddKill("Dot");
            }
            else if (killer.gameObject.ToString().Contains("ShooterDot"))
            {
                geo = killer.gameObject.GetComponent<ShooterDotController>();
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
        Debug.Log(killer.gameObject.ToString() + " killed " + dead.ToString());
        Destroy(dead);
    }
}
