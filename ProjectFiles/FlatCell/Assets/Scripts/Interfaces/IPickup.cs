// IPickup.cs
// Nick S.
// Game Logic - AI

using UnityEngine;
using Geo.Command;

/*
 * IPickup- Pickup Interface
 * 
 * This bad boy is attached to a GameObject, sits there, and waits to be called in DotSpawner.Kill() when an AI
 * dies. All values of the pickup are determined to the parent's value on IPickup.Init().
 * 
 * Change this logic to determine the values in IPickup.Spawn() if you want the pickups to get the values
 * of the owner at death instead of at spawn.
 * 
*/
  
namespace Pickup.Command
{
    public interface IPickup
    {
        // initializes the pickup after attaching it to a game object.
        // this sets the values of the pickup to be used when the player "eats it"
        void Init(IGeo geo, EPickup_Type t);

        // programtically creates the pickup drop at the location
        GameObject Spawn(Vector3 Location);

        // the collision logic to ignore projectiles and AI
        void OnCollisionEnter(Collision geo);

        // wreck dat ho
        void Destroy();

        // returns the type of pickup, ie HP, armor, etc
        EPickup_Type GetType();

        // sets the type. will only affect successive calls to Spawn()
        void SetType(EPickup_Type t);
    }
}
