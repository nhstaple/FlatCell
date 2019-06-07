using UnityEngine;

namespace Geo.Command
{
    public interface IGeo
    {
        void init(float Speed, float MaxHP, float FireRate, float FireChance, float ShieldChance);

        // Fires a projectile.
        void Shoot(int WeaponIndex, float SpawnOffset);

        void FlameOn();
        void FlameOff();

        // Returns the current speed.
        float GetCurrentSpeed();

        // Check for collisions with other objects.
        void OnCollisionEnter(Collision collision);

        Vector3 GetForward();

        Color GetColor();

        Shield GetShield();

        bool SetMaxHealth(float h);
        float GetMaxHealth();

        void Hurt(float d);

        void Respawn();

        void AddKill(string name);

        GameObject GetGameObject();
    }
}