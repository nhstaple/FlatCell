using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Geo.Command;

namespace Spawner.Command
{
    public interface ISpawner
    {
        // Spawns an AI
        void Spawn();

        // Kills an AI. If it's the player, they respawn. See IGeo.Respawn()
        void Kill(GameObject geo, GameObject killer);
    }
}