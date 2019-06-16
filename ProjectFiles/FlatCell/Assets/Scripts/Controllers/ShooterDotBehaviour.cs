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
        const float ResetVisionTimer = 3f;
        float visionCounter = 0.0f;
        Collider target;
        new void Start()
        {
            base.Start();
            type = "Shooter Dot";
        }

        new public void Update()
        {
            base.Update();
            if(PlayerVisionLocked && target != null)
            {
                transform.LookAt(target.transform);
            }
            Fire();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Entered a trigger: " + other.ToString());
            if(other.tag == "Player")
            {
                target = other;
                PlayerVisionLocked = true;
                Debug.Log("Looking at the player!");
                transform.LookAt(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerVisionLocked = false;
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
