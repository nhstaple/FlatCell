/*
 * 
\* IGeo.cs
 *
\* Nick S.
\* Game Logic - AI
 *
*/

using UnityEngine;
using Weapon.Command;
using Geo.Meter;

/*
 * IGeo - Geo Object Interface
 * 
 * This is the base interface for all geometric objects in the game, ie all AI and the player.
 * 
 * GameObjects are constructed programatically rather than through prefabs. This approach was
 * taken because the design approach to his game was to use as many built-in Unity tools as 
 * possible to create Flat Cell.
 * 
*/

/*
 * TODO
 * Update documentation 6/16/19
*/

namespace Geo.Command
{
    public interface IGeo
    {
        /** Player **/
        void MoveTo(Vector3 Position, Vector3 MovementVector, float Speed, bool ForwardLock = true);
        void LookAt(Vector3 Position, bool VisionLock = false);
        void Respawn();
        void Kill();
        void AddColor(Color c);
        float GetMovementMagnitude();
        Vector3 GetMovementDirection();
        Vector3 GetGunDir();
        void ResetColorAfterShield(float time = 1f);
        bool GetAnimLock();
        bool ColorIsFrozen();
        void FreezeColor();
        void ThawColor();

/** AI **/
        // Constructors
        // Initializes the Component when you add it to game object. ie, initializing an AI
        void Init(float Speed,          // The speed at which the AI moves.
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

/** Stats  **/
    // Get
        float GetHealth();
        float GetMaxHealth();
        float GetHealthPercent();
        float GetArmor();
        float GetDamage();
        float GetSpeed();
        float GetMaxSpeed();
        float GetSpeedPercent();
        Color GetColor();
        Shield GetShield();
    // Set
        void SetDamage(float d);
        void SetSpeed(float s);
        void SetColor(Color c);
        void DrawDebug(bool flag = false);
    }
}