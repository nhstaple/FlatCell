using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Geo.Command;

namespace Spawner.Command
{
    public enum EGeoSpawnType
    {
        Dot = 0,
        Line = 1
    }

    public interface ISpawner
    {
        void Spawn();
    }
}