using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDotController : SimpleDotController
{
    private float counter = 0.0f;
    private float period = 0.0f;
    new void Start()
    {
        base.Start();
    }
    void Awake()
    {

    }

    new void Update()
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