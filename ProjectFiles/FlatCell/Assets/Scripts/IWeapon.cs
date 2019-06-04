using UnityEngine;

using Geo.Command;

namespace Weapon.Command
{
    public interface IWeapon
    {

        // Sets the damage and piercing
        void SetDamage(float Damage, float Piercing);

        // Fires a projectile.
        void Fire(Vector3 movementDir, Vector3 pos, float push);
    }
}