using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

using Geo.Command;
using Spawner.Command;

public class DotSpawner : MonoBehaviour, ISpawner
{
    private Vector3 Location = new Vector3(0, 25, 0);
    private const float SpawnOffset = 250.0f;
    private List<GameObject> Alive = new List<GameObject>();
    private int counter = -1;

    [SerializeField] public float Speed;
    [SerializeField] public float MaxHealth;
    [SerializeField] public float FireRate;
    [SerializeField] public float FireChance;
    [SerializeField] public float Damage;

    GameObject player;

    public void Start()
    {
        player = GameObject.FindWithTag("Player");
        Alive.Add(player);
    }

    public void Spawn()
    {
        counter++;
        Location.x = UnityEngine.Random.Range(-SpawnOffset, SpawnOffset);
        Location.z = UnityEngine.Random.Range(-SpawnOffset, SpawnOffset);
        GameObject Dot = new GameObject("Dot" + counter);
        Dot.transform.position = Location;
        Dot.transform.localScale = new Vector3(25, 25, 25);

        // Add the ai controller and set it's values to the serialize field
        DotController ai = Dot.AddComponent<DotController>();
        ai.Speed = Speed;
        ai.MaxHealth = MaxHealth;
        ai.FireRate = FireRate;
        ai.FireChance = FireChance;
        ai.health = MaxHealth;
        Alive.Add(Dot);
    }
    void Update()
    {
        if(Alive.Count < 10)
        {
            Spawn();
        }
        // .1% to kill a random dot
        if(UnityEngine.Random.Range(0, 1000) <= 1 && Alive.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, Alive.Count - 1);
            Kill(Alive[index]);
        }
    }

    public void Kill(GameObject Dot, bool KilledByPlayer = false)
    {
        if(Dot.ToString().Contains("Player"))
        {
            PlayerController controller = Dot.GetComponent<PlayerController>();
            Transform t = controller.GetComponent<Transform>();
            // Reset the player to spawn location. 
            t.position = new Vector3(0, 25, 0);
            controller.health = controller.MaxHealth;
            var score = controller.killHistory["Dot"];
            controller.killHistory["Dot"] = (int)UnityEngine.Random.Range(score * 0.50f, score * 0.75f);
            /*
            var scale = controller.transform.localScale;
            if(score >= 25)
            {
                scale = new Vector3(50, 1, 1);
                controller.SpawnOffset = controller.initSpawnOffset + 15;
            }
            else
            {
                scale = new Vector3(25 + score, 25 - score, 25 - score);
                controller.SpawnOffset = controller.initSpawnOffset + (int)score;
            }
            controller.transform.localScale = scale;
            */           
            Debug.Log("You died! Fool");
        }
        else if(Alive.Contains(Dot))
        {
            Alive.Remove(Dot);
            Destroy(Dot);
            if(KilledByPlayer)
            {
                PlayerController p = player.GetComponent<PlayerController>();
                p.killHistory["Dot"] = p.killHistory["Dot"] + 1;
            }
        }
    }
}
