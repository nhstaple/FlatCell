using UnityEngine;
using Weapon.Command;

namespace Projectile.Command
{
    public interface IProjectile
    {
        void init(IWeapon owner, float Damage, float Piercing, float ProjectileLifetime);
        // Spawn the projectile.
        GameObject Spawn(Vector3 Location);

        // Sets the damage and piercing
        void SetDamage(float Damage, float Piercing);

        IWeapon GetOwner();

        float GetDamage();

        float GetPiercing();

        void SetLifeTime(float time);

        float GetLifeTime();

        void SetOwner(IWeapon w);
    }
}