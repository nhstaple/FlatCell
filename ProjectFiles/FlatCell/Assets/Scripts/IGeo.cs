using UnityEngine;

namespace Geo.Command
{
    public interface IGeo
    {
        // Fires a projectile.
        void Shoot(int WeaponIndex, float SpawnOffset);

        // Returns the current speed.
        float GetCurrentSpeed();

        // Check for collisions with other objects.
        void OnCollisionEnter(Collision collision);

        Vector3 GetForward();

        Color GetColor();
    }
}