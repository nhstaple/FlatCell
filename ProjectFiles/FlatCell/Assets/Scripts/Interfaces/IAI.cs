/*
 * 
\* IAI.cs
 *
\* Nick S.
\* Game Logic - AI
 *
*/

using Geo.Command;
using DotBehaviour.Command;

/*
 * IAI - AI Interface
 * 
 * The abstact interface that handles all AI logic. More functions should be added here if we ant the AI
 * to do more special things.
 * 
 * This is the interface for all AI. Usage, from DotSpawner.cs
  
   // Spawns a Simple Dot. Sets name to a list of tags delim by " ",
   // query with gameObject.ToString.Contains("tag")
   GameObject Dot = new GameObject("Geo Simple Dot" + counter);
   IAI ai1 = Dot.AddComponent<DotController>();
   ai1.init(null, Speed, MaxHealth, FireRate, FireChance, ShieldChance, EnableTrail, DrawDebugLine);
 
   // Spawns a Shooter Dot.
   GameObject Dot = new GameObject("Geo Shield Dot" + counter);
   IAI ai2 = Dot.AddComponent<DotController>();
   ShooterDotBehaviour b = Dot.AddComponent<ShooterDotBehaviour>();
   b.init(ai);
   ai2.init(b, Speed, MaxHealth, FireRate, FireChance, ShieldChance, EnableTrail, DrawDebugLine);
*/

/*
 * TODO
 * Update documentation 6/16/19
*/

namespace AI.Command
{
    public interface IAI : IGeo
    {
        void Init(
          IDotBehaviour behaviour, // The dot's behaviour
          float Speed,             // The speed at which the AI moves.
          float MaxHP,             // The MaxHP of the AI.
          float FireRate,          // The time to wait between firing bullets in seconds.
          float FireChance,        // 0-100 how likely the AI is to fire their gun.
          float ShieldChance,      // 0-100 how likely the AI is to turn on their shield.
          bool ShowTrail,          // Display the trail of the geometry.
          bool DrawDebugLine);     // Display the debug line pointing in dir of forward.
    }
}
