// ShooterDotController.cs
// Nick S.
// Game Logic - AI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Command;

/*
 * Shooter Dot Controller
 * 
 * This AI can:
   - move
   - shoot
 * 
*/

public class ShooterDotController : SimpleDotController
{
    new void Start()
    {
        base.Start();
    }
    void Awake()
    {
   
    }

    new public void Update()
    {
        base.Update();
        
        if(Random.Range(0, 100) <= FireChance)
        {
            if(Random.Range(0, 100) <= FireChance*FireChance/2)
            {
                DotWeapon gun = (DotWeapon)weapon[0];
                gun.Fire(transform.forward, transform.position, DotProjectilePush, ProjectileSpawnOffset);
            }
        }
    }
}