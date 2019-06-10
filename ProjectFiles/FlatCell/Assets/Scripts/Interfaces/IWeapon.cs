// IWeapon.cs
// Nick S.
// Game Logic - AI

using UnityEngine;
using Geo.Command;

/*
 * IWeapon - Weapon Interface
 * 
 * Each piece of geometry has a weapon, think of player in Call of Duty has a gun.
 * Each weapon has an associate IProjectile that is used to spawn new projectiles.
 * If you want to make special ammunition, new weapons, etc., see DotProjectile.cs for
 * an example.
 * 
 Public
   // AI
   void init(IGeo GeoOwner, AudioClip Sound, float Damage = 1, float Pierce = 0, float Rate = 0.125f, float lifeTime = 2.5f);
 * 
*/
namespace Weapon.Command
{
    public interface IWeapon
    {
        void init(IGeo GeoOwner, AudioClip Sound,
                  float Damage = 1, float Pierce = 0, float Rate = 0.125f, float lifeTime = 2.5f);

        // Sets the damage and piercing
        void SetDamage(float Damage, float Piercing);

        // Fires a projectile.
        void Fire(Vector3 movementDir, Vector3 pos, float push, float SpawnOffset);

        // Returns the IGeo owner of the weapon.
        IGeo GetOwner();
        void SetOwner(IGeo geo);
    }
}