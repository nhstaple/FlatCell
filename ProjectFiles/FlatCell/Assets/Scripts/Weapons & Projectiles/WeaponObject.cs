// WeaponObject.cs
// Nick S.
// Game Logic - Combat

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Command;
using Projectile.Command;
using Geo.Command;

public class WeaponObject : MonoBehaviour, IWeapon
{
    /** Weapon Stats **/
    protected float FireRate;
    protected float Damage;
    protected float Piercing;
    protected float ProjectileLifetime;
    // The owner of the weapon
    protected IGeo Owner;
    // The bullet to fire
    protected IProjectile Projectile;

    /** Script variables **/
    // Keeps track of the player's last input values.
    protected Vector3 lastMove;
    // Keeps track of the last time the weapon was shot.
    protected float shootCounter;

    public void init(IGeo GeoOwner, float Damage, float Pierce = 0, float Rate = 0.01f, float lifeTime = 2.5f)
    {
        this.Owner = GeoOwner;
        this.FireRate = Rate;
        this.Damage = Damage;
        this.Piercing = Pierce;
        this.ProjectileLifetime = lifeTime;
    }

    public void Start()
    {
        
    }

    public void Update()
    {
        
    }

    public void SetDamage(float Damage, float Piercing)
    {
        this.Damage = Damage;
        this.Piercing = Piercing;
    }

    public void Fire(Vector3 movementDir, Vector3 pos, float push, float SpawnOffset)
    {
        if (movementDir.magnitude > 0)
        {
            lastMove = movementDir;
            if (lastMove.x > 0 && lastMove.z == 0) { lastMove.x = 1; }
            if (lastMove.x < 0 && lastMove.z == 0) { lastMove.x = -1; }
            if (lastMove.z > 0 && lastMove.x == 0) { lastMove.z = 1; }
            if (lastMove.z < 0 && lastMove.x == 0) { lastMove.z = -1; }
        }
        shootCounter += Time.deltaTime;
        return;
    }

    public IGeo GetOwner()
    {
        return Owner;
    }
}