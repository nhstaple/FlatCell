using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterDotController : SimpleDotController
{
    new void Start()
    {
        base.Start();
    }
    void Awake()
    {

    }

    new protected void Update()
    {
        base.Update();
        
        if(Random.Range(0, 100) <= FireChance)
        {
            if(Random.Range(0, 100) <= FireChance*2)
            {
                weapon[0].Fire(transform.forward, transform.position, DotProjectilePush, SpawnOffset);
            }
        }
    }
}