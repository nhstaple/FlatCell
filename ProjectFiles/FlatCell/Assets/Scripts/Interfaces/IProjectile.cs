using UnityEngine;

using Weapon.Command;

namespace Projectile.Command
{
    public interface IProjectile
    {
        // Spawn the projectile.
        GameObject Spawn(Vector3 Location);

        // Sets the damage and piercing
        void SetDamage(float Damage, float Piercing);
    }
}