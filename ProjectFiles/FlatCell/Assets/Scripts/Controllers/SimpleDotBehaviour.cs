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
        [SerializeField] protected float DirectionChangeTimer = 0.5f;

        protected float timer = 0.0f;
        protected float xMovementDir;
        protected float zMovementDir;
        protected Vector3 movementDirection;
        protected float initSpeed;
        protected float initDamage;
        protected float moveMax = 0.5f;

        public string type;

        public void Start()
        {
            movementDirection = new Vector3(Random.Range(0.5f, 1f), 0.0f, Random.Range(0.5f, 1f));
            // transform.LookAt(movementDirection, new Vector3(0, 1, 0));
            Vector3 newDir = Vector3.RotateTowards(transform.forward, movementDirection, 360 * Mathf.Deg2Rad, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            float step = owner.GetSpeed() * Time.deltaTime;
            Debug.Log("speed = " + owner.GetSpeed());
            Vector3 target = transform.position + movementDirection * owner.GetSpeed();
            owner.MoveTo(target, step);
            // Rigidbody b = GetComponent<Rigidbody>();
            // b.AddForce(movementDirection * Random.Range(0.5f*owner.GetSpeed(), owner.GetSpeed()), ForceMode.VelocityChange);
            type = "Simple Dot";
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

        public void Update()
        {
            CheckScore();
            Move();
        }

        public void Move()
        {
            timer += Time.deltaTime;
            if (timer >= DirectionChangeTimer)
            {
                timer = 0.0f;
                movementDirection += new Vector3(Random.Range(-moveMax * Random.Range(0, 1f),
                                                               moveMax * Random.Range(0, 1f)),
                                                 0.0f,
                                                 Random.Range(-moveMax * Random.Range(0, 1f),
                                                               moveMax * Random.Range(0, 1f)));

                if(movementDirection.x > 1)         { movementDirection.x = 1; }
                else if (movementDirection.x < -1 ) { movementDirection.x = -1; }
                if (movementDirection.z > 1)        { movementDirection.z = 1; }
                else if (movementDirection.z < -1)  { movementDirection.z = -1; }

                // Roatate the object.
                Vector3 newDir = Vector3.RotateTowards(transform.forward, movementDirection, 360 * Mathf.Deg2Rad, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);

                // Move the object.
                Vector3 loc = owner.GetGameObject().transform.position;
                float speed = Random.Range(0.5f*owner.GetSpeed(), owner.GetSpeed());
                float step = speed * Time.deltaTime;
                owner.MoveTo(loc + speed * movementDirection, step);
                // Rigidbody b = GetComponent<Rigidbody>();
                // b.AddForce(movementDirection * Random.Range(0.5f*owner.GetSpeed(), owner.GetSpeed()), ForceMode.VelocityChange);
            }
        }

        public void Shields()
        {
            return;
        }

        public void Fire()
        {
            return;
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
