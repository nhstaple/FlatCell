/*
 * 
\* ShooterDotBehaviour.cs
 *
\* Nick S.
\* Game Logic - AI
 *
*/
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
        GameObject target;

        new void Start()
        {
            base.Start();
            type = EDot_Behaviour.Shooter;
        }

        new public void Update()
        {
            base.Update();
            if (target != null)
            {
                Vector3 distance = target.transform.position - this.transform.position;
                if(PlayerVisionLocked || distance.magnitude < 100f)
                {
                    owner.LookAt((target.transform.position - this.transform.position).normalized);
                    Fire();
                }
            }
            else
            {
                Fire();
            }
        }

        new public void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("Looking at the player!");
                target = collision.gameObject;
                PlayerVisionLocked = true;
                transform.LookAt(target.transform);
            }
        }

        public void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                PlayerVisionLocked = false;
            }
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
