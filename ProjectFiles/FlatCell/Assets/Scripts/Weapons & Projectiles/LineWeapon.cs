using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Command;

using Projectile.Command;

namespace Weapon.Command
{
    public class LineWeapon : ScriptableObject
    {
        [SerializeField]
        private float FireRate = 0.25f;

        private float currentSpeed;

        private Vector3 lastMove;

        private float ShootCounter;

        private float SpawnOffset = 10f;

        //special projectile type
        private IProjectile lineProjectile;

        private void Start()
        {
            currentSpeed = 0;
            ShootCounter = 0;
        }

        public void Fire(GameObject sampleProjectile, Vector3 movementDir, Rigidbody player_rigidbody, Vector3 pos)
        {
            ShootCounter += Time.deltaTime;
            if (ShootCounter >= this.FireRate)
            {
                Vector3 spawnLoc = pos + lastMove * SpawnOffset;

                GameObject bullet = Instantiate(sampleProjectile, spawnLoc, Quaternion.identity) as GameObject;

                //GameObject bullet = this.dotProjectile.Spawn(sampleProjectile, movementDir, pos, 10, 1, 1);

                Rigidbody bullet_rigidbody;
                bullet_rigidbody = bullet.GetComponent<Rigidbody>();//this.Target.GetComponent<PlayerController>().bullet.GetComponent<Rigidbody>()
                                                                    //        bullet_rigidbody.AddRelativeForce()


                //Debug.Log(player_rigidbody.velocity);
                //this.transform.up;

                // bullet_rigidbody.AddRelativeForce(lastMove * (push + currentSpeed * ImpulseModifier), ForceMode.Impulse);
                bullet_rigidbody.AddRelativeForce((lastMove) * (200), ForceMode.Impulse);
                Debug.Log("Fired");
                Debug.Log(bullet_rigidbody);

                Destroy(bullet, 4.0f);//destroy bullet after 3 seconds
                ShootCounter = 0.0f;
            }
            if (movementDir.magnitude > 0)
            {
                lastMove = movementDir;
                if (lastMove.x > 0 && lastMove.z == 0) { lastMove.x = 1; }
                if (lastMove.x < 0 && lastMove.z == 0) { lastMove.x = -1; }
                if (lastMove.z > 0 && lastMove.x == 0) { lastMove.z = 1; }
                if (lastMove.z < 0 && lastMove.x == 0) { lastMove.z = -1; }
                Debug.Log(lastMove);
            }
            ///////////


            return;
        }

    }
}
