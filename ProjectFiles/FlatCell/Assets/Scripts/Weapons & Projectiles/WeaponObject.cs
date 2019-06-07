using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Command;
using Projectile.Command;
using Geo.Command;

public class WeaponObject : IWeapon
{
    /** Weapon Stats **/
    protected float FireRate;
    protected float Damage;
    protected float Piercing;
    protected float ProjectileLifetime;

    /** Script variables **/
    // The owner of the weapon
    protected IGeo Owner;
    // Projectile interface
    protected IProjectile Projectile;
    // Keeps track of the player's last input values.
    protected Vector3 lastMove;
    // Keeps track of the last time the weapon was shot.
    protected float shootCounter;

    public WeaponObject(float rate = 0.125f, float dmg = 1, float pierce = 0, float lifetime = 2.5f)
    {
        this.FireRate = rate;
        this.Damage = dmg;
        this.Piercing = pierce;
        this.ProjectileLifetime = lifetime;
        shootCounter = 0;
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