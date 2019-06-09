using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;
using DotBehaviour.Command;

namespace AI.Command
{
    public interface IAI : IGeo
    {
        void init(
          IDotBehaviour behaviour, // The dot's behaviour
          float Speed,             // The speed at which the AI moves.
          float MaxHP,             // The MaxHP of the AI.
          float FireRate,          // The time to wait between firing bullets in seconds.
          float FireChance,        // 0-100 how likely the AI is to fire their gun.
          float ShieldChance,      // 0-100 how likely the AI is to turn on their shield.
          bool ShowTrail,
          bool DrawDebugLine);         // Display the trail of the geometry.
    }
}
