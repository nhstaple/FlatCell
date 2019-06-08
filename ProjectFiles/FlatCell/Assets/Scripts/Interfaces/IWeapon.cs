using UnityEngine;

using Geo.Command;

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
    }
}