using UnityEngine;

namespace Geo.Command
{
    public interface IGeo
    {
        // Fires a projectile.
        void Shoot(Vector3 Direction, Vector3 Location, float Force);
        // Returns the current speed.
        float GetCurrentSpeed();

        // Check for collisions with other objects.
        void OnCollisionEnter(Collision collision);
    }
}