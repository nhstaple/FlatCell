using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;

namespace Pickup.Command
{
    public interface IPickup
    {
        void init(IGeo geo, string t);

        GameObject Spawn(Vector3 Location);

        void OnCollisionEnter(Collision geo);

        void Destroy();

        string GetType();

        void SetType(string t);
    }
}
