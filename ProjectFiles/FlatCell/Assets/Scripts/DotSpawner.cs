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

    public void Start()
    {

    }

    public void Spawn()
    {
        counter++;
        Location.x = UnityEngine.Random.Range(-SpawnOffset, SpawnOffset);
        Location.z = UnityEngine.Random.Range(-SpawnOffset, SpawnOffset);
        GameObject Dot = new GameObject("Dot" + counter);
        // Dot.AddComponent<DotObject>();//Makes Dot a DotObject object (with component)
        Dot.AddComponent<DotController>();//allows for AIController to be applied to dots
        Dot.transform.position = Location;
        Dot.transform.localScale = new Vector3(25, 25, 25);
        // GameObject.Destroy(Dot, 5f);
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

    void Kill(GameObject Dot)
    {
        if(Alive.Contains(Dot))
        {
            Alive.Remove(Dot);
            Destroy(Dot);
        }
    }
}
