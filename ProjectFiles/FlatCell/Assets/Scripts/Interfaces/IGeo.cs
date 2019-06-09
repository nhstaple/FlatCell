// IGeo.cs
// Nick S.
// Game Logic - AI

using UnityEngine;
using Weapon.Command;

/*
 * IGeo - geo object interface
 * 
*/

namespace Geo.Command
{
    public interface IGeo
    {
/** Player **/
        void Respawn();

/** AI **/
    // Constructors
        // Initializes the Component when you add it to game object. ie, initializing an AI
        void init(float Speed,          // The speed at which the AI moves.
                  float MaxHP,          // The MaxHP of the AI.
                  float FireRate,       // The time to wait between firing bullets in seconds.
                  float FireChance,     // 0-100 how likely the AI is to fire their gun.
                  float ShieldChance,   // 0-100 how likely the AI is to turn on their shield.
                  bool ShowTrail);      // Display the trail of the geometry.

    // Get Methods
        // Returns the first weapon in the weapon list
        IWeapon GetWeapon();
        // Returns the current speed.
        float GetCurrentSpeed();
        // Used for scripting behaviour controllers
        float GetShieldChance();
        float GetFireChance();
        // Returns the aggregated score of the kill record.
        float GetScore();

    // Controller Methods
        // Fires a projectile.
        void Shoot();
        void Shoot(float SpawnOffset);

        // Toggles the shields on/off
        void FlameOn();
        void FlameOff();

        // Returns the gameobject
        GameObject GetGameObject();

    // Game Logic 
        // Add a kill to the geo's record
        void AddKill(string name);
        // Returns the forward vector
        Vector3 GetForward();

    // Physics
        // Check for collisions with other objects.
        void OnCollisionEnter(Collision collision);


/** Mutators **/
    // Sets the maxhp to h, if health == maxhp false otherwise
        bool SetMaxHealth(float h);
        void Hurt(float d);
        void Heal(float d);
        void ModifyArmor(float a);
        // void IncreaseArmor(float a);
        // void DecreaseArmor(float a);

/** Stats  **/
    // Get
        float GetHealth();
        float GetMaxHealth();
        float GetArmor();
        float GetDamage();
        float GetSpeed();
        Color GetColor();
        Shield GetShield();
    // Set
        void SetDamage(float d);
        void SetSpeed(float s);
    }
}