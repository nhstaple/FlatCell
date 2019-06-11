// ShooterDotBehaviour.cs
// Nick S.
// Game Logic - AI

using UnityEngine;

/*
 * ShieldDotBehaviour - A Shooter Dot
 * 
 * This behaviour script extends the functionality of SimpleDotBehaviour.cs.
 * 
 * This AI will move and fire it's weapon. That's about it.
 * 
*/

namespace DotBehaviour.Command
{
    class ShooterDotBehaviour : SimpleDotBehaviour
    {
        new void Start()
        {
            base.Start();
            type = "Shooter Dot";
        }

        new public void Update()
        {
            base.Update();
            Fire();
        }

        new public void Fire()
        {
            if (Random.Range(0, 100) <= owner.GetFireChance())
            {
                owner.Shoot();
            }
        }
    }
}
