using UnityEngine;
using Weapon.Command;

namespace Geo.Command
{
    public interface IGeo
    {
        // Initializes the Component when you add it to game object. ie, initializing an AI
        void init(float Speed, float MaxHP, float FireRate, float FireChance, float ShieldChance, bool ShowTrail);

        // Fires a projectile.
        void Shoot(float SpawnOffset);

        void Shoot();

        // Toggles the shields on/off
        void FlameOn();
        void FlameOff();

        // Returns the current speed.
        float GetCurrentSpeed();

        // Check for collisions with other objects.
        void OnCollisionEnter(Collision collision);

        // Returns the forward vector
        Vector3 GetForward();

        // Returns the color/
        Color GetColor();

        // Returns the shield object.
        Shield GetShield();

        // Sets the maxhp to h, if health == maxhp
        // false otherwise
        bool SetMaxHealth(float h);

        // Returns max health.
        float GetMaxHealth();

        //Returns health.
        float GetHealth();

        // Deals damage to the geo
        void Hurt(float d);

        // Respawns the geo- used only for Player
        void Respawn();

        // Add a kill to the geo's record
        void AddKill(string name);

        // Returns the gameobject
        GameObject GetGameObject();

        // Returns the aggregated score of the kill record.
        float GetScore();

        float GetSpeed();

        void SetDamage(float d);

        void SetSpeed(float s);

        float GetShieldChance();

        float GetFireChance();

        IWeapon GetWeapon();
    }
}