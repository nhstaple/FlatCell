using UnityEngine;

namespace Geo.Command
{
    public interface IGeo
    {
        // Fires a projectile.
        void Shoot();
        // Returns the current speed.
        float GetCurrentSpeed();
    }
}