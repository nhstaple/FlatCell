// ShieldDotController.cs
// Nick S.
// Game Logic - AI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Shield Dot Controller
 * 
 * This AI can:
   - move
   - use shields
*/

public class ShieldDotController : SimpleDotController
{
    private float period = 0.0f;
    new public void Start()
    {
        base.Start();
    }

    new public void Update()
    {
        base.Update();

        if (!shield.IsActve() && Random.Range(0, 100) <= ShieldChance)
        {
            if (Random.Range(0, 100) <= ShieldChance*2)
            {
                period = Random.Range(0, shield.GetMaxEnergy());
            }
        }
        if(period >= 0 && shield.IsReady())
        {
            FlameOn();
        }
        else
        {
            FlameOff();
        }
    }
}