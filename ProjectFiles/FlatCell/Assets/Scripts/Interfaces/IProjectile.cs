// IProjectile.cs
// Nick S.
// Game Logic - Combat

using UnityEngine;
using Weapon.Command;

/*
 * IProjectile - Projectile Interface
 * 
 * This acts as a programatic spawner for all Projectiles.
   
   - Geos contain Weapons
   - Weapons contain Projectiles
   - Projectiles spawn GameObjects
 
 Usage
   // From DotWeapon.cs
   this.Projectile = gameObject.AddComponent<DotProjectile>();
   DotProjectile boopCast = (DotProjectile) this.Projectile;
   boopCast.init(this, Damage, Piercing, ProjectileLifetime);
*/
namespace Projectile.Command
{
    public interface IProjectile
    {

        void init(IWeapon owner, float Damage, float Piercing, float ProjectileLifetime);

        // Spawn the projectile at the location.
        GameObject Spawn(Vector3 Location);

        // Sets the damage and piercing
        void SetDamage(float Damage, float Piercing);

        // Returns a pointer to the weapon that this projectile belongs to.
        // To get the geo the weapon belongs to call GetOwner on the IWeapon.
        IWeapon GetOwner();

        // Gets the damage of the projectile after it's been fired.
        float GetDamage();

        // Gets the armor piercing of the projectile. Not implemented.
        float GetPiercing();

        // Sets the lifetime. Effects successive calls to spawn.
        void SetLifeTime(float time);

        // Returns how long the projectiles will live.
        float GetLifeTime();

        void SetOwner(IWeapon w);
    }
}