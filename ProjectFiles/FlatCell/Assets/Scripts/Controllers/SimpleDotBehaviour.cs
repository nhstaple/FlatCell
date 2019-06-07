using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;

namespace DotBehaviour.Command
{
    class SimpleDotBehaviour : MonoBehaviour, IDotBehaviour
    {

        protected IGeo owner;

        [SerializeField] protected float DotProjectilePush = 100;
        [SerializeField] protected float DirectionChangeTimer = 1f;
        [SerializeField] protected float DirectionChangeWeight = 10;

        protected float timer = 0.0f;
        protected float xMovementDir;
        protected float zMovementDir;
        protected Vector3 movementDirection;
        protected float initSpeed;
        protected float initDamage;

        public string type;

        public void Start()
        {
            xMovementDir = Random.Range(-1f, 1f);
            zMovementDir = Random.Range(-0.5f, 0.5f);
            movementDirection = new Vector3(xMovementDir, 0.0f, zMovementDir);
            type = "SimpleDot";
        }

        new public string GetType()
        {
            return type;
        }

        public void init(IGeo geo)
        {
            owner = geo;
            if(owner != null)
            {
                initSpeed = owner.GetSpeed();
                initDamage = owner.GetSpeed();
            }
        }

        public void exec()
        {
            CheckScore();
            Move();
        }

        public void Move()
        {
            float step = owner.GetSpeed() * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(movementDirection.x, 0, movementDirection.z), step, 0.0f);
            // Move our position a step closer to the target.
            transform.position = Vector3.MoveTowards(transform.position, transform.position + movementDirection * step, step);
            transform.forward = newDir;

            if (timer >= DirectionChangeTimer)
            {
                timer = 0.0f;
                movementDirection = new Vector3(Random.Range(-1, 1) * DirectionChangeWeight,
                                                0.0f,
                                                Random.Range(-1, 1) * DirectionChangeWeight);
            }
        }

        public void Shields()
        {
            return;
        }

        public void Fire()
        {

        }

        public void CheckScore()
        {
            const int maxKills = 10;
            const int killWeight = 5;
            if (owner.GetScore() < maxKills)
            {
                owner.SetSpeed(initSpeed + owner.GetScore() * killWeight);
                owner.SetDamage(initDamage + owner.GetScore() * 0.1f);
            }
            else
            {
                owner.SetSpeed(initSpeed + maxKills * killWeight);
                owner.SetDamage(initDamage + maxKills * 0.1f);
            }
        }
    }
}
