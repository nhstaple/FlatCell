using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Command;

public class ProjectileObject : MonoBehaviour
{
    private float Damage;

    private float Piercing;

    private float LifeTime;

    IWeapon owner;

    public void SetDamage(float Damage, float Piercing)
    {
        this.Damage = Damage;
        this.Piercing = Piercing;
    }
    public float GetPiercing()
    {
        return this.Piercing;
    }

    public float GetDamage()
    {
        return this.Damage;
    }

    public void SetLifeTime(float time)
    {
        this.LifeTime = time;
    }

    public float GetLifeTime()
    {
        return this.LifeTime;
    }

    public void SetOwner(IWeapon w)
    {
        owner = w;
    }

    public IWeapon GetOwner()
    {
        return owner;
    }

    // Start is called before the first frame update
    void Start()
    {
        Damage = 1;
        Piercing = 0;
        LifeTime = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
