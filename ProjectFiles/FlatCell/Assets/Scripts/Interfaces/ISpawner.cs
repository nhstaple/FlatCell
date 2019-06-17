/*
 * 
\* ISpawner.cs
 *
\* Nick S.
\* Game Logic - AI
 *
*/


using UnityEngine;

/*
 * ISpawner - Spawner Interface
 * 
 * This interface handles the spawning for all AI. See DotSpawner.cs for an example implmentation of the spawner.
 * 
*/

namespace Spawner.Command
{
    public interface ISpawner
    {
        // Spawns an AI
        void Spawn();

        // Kills an AI. If it's the player, they respawn. See IGeo.Respawn()
        void Kill(GameObject geo, GameObject killer);

        // Kills an AI if Random.Range(0, cap) <= 1
        void Lottery(float cap);
    }
}