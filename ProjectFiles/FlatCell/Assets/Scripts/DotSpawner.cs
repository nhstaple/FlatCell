using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Geo.Command;
using Spawner.Command;

public class DotSpawner :  ISpawner
{
    private Vector3 Location = new Vector3(50, 5, 0);
    private const float SpawnOffset = 100.0f;
    private List<GameObject> Alive;
    private int counter = -1;
    public void Spawn()
    {
        counter++;
        Location.x = Random.Range(-SpawnOffset, SpawnOffset); 
        Location.z = Random.Range(-SpawnOffset, SpawnOffset);
        GameObject Dot = new GameObject("Dot" + counter);
        Dot.AddComponent<DotObject>();
        Dot.transform.position = Location;
        Dot.transform.localScale = new Vector3(5, 5, 5);
        // GameObject.Destroy(Dot, 5f);
        // Alive.Add(Dot);
    }
}
