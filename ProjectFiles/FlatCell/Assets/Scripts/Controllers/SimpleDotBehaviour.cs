// SimpleDotBehaviour.cs
// Nick S.
// Game Logic - AI

using UnityEngine;
using Geo.Command;
using Utils.Vectors;

/*
 * DotBehaviour - The implemntation of the IDotBehaviour interface.
 * 
 * This script will be attached to an IAI object and control it's movement,
 * guns, shields, movement, etc.
 * 
 Public
   // Returns the beheviour type. Ie, "Simple Dot", "Shield Dot", ...
   string GetType()

   // Initializes the script.
   void init(IGeo geo)

   // The Move logic.
   void Move()

   // The Firing logic.
   void Fire()

   // Checks' the AI kill record and update's its stats accordingly.
   // This is how AI "get stronger."
   void CheckScore()
   
*/
namespace DotBehaviour.Command
{
    public enum EDot_Behaviour
    {
        Simple = 0,
        Shield,
        Shooter
    }

    class SimpleDotBehaviour : MonoBehaviour, IDotBehaviour
    {
        [SerializeField] protected float DotProjectilePush = 100;
        [SerializeField] protected float DirectionChangeTimer = 0.5f;
        [SerializeField] protected float moveMax = 0.5f;
        [SerializeField] protected int maxKills = 10;
        [SerializeField] protected int killWeight = 5;

        protected float timer = 0.0f;
        protected float xMovementDir;
        protected float zMovementDir;
        protected Vector3 movementDirection;
        protected float initSpeed;
        protected float initDamage;

        protected IGeo owner;

        public EDot_Behaviour type;

        public bool ResetToSpawn = false;
        protected bool PlayerVisionLocked = false;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Boundary")
            {
                ResetToSpawn = true;
                Debug.Log("Hit the wall!");
                movementDirection.x *= -1;
                movementDirection.y *= -1;
                Vector3 pos = -1 * transform.position;
                pos.y = 0;
                pos.Normalize();

                Rigidbody body = GetComponent<Rigidbody>();
                body.AddForce(25f * pos, ForceMode.Impulse);
                owner.MoveTo(Locations.SpawnLocation, movementDirection, owner.GetSpeed());
            }
        }

        public void Start()
        {
            movementDirection = new Vector3(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f));
            // transform.LookAt(movementDirection, new Vector3(0, 1, 0));
            Vector3 newDir = Vector3.RotateTowards(transform.forward, movementDirection, 360 * Mathf.Deg2Rad, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            // Debug.Log("speed = " + owner.GetSpeed());
            Vector3 target = transform.position + movementDirection * 100 * owner.GetSpeed();
            Rigidbody b = GetComponent<Rigidbody>();
            b.AddForce(movementDirection * Random.Range(0.25f*owner.GetSpeed(), owner.GetSpeed()), ForceMode.VelocityChange);
            type = EDot_Behaviour.Simple;
        }

        new public EDot_Behaviour GetType()
        {
            return type;
        }

        public void Init(IGeo geo)
        {
            owner = geo;
            if (owner != null)
            {
                initSpeed = owner.GetSpeed();
                initDamage = owner.GetSpeed();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Spawner")
            {
                ResetToSpawn = false;
            }
        }

        public void Update()
        {
            CheckScore();
            Move();
        }

        public void Move()
        {
            // Increment the timer and check if it's exceed our poll rate.
            timer += Time.deltaTime;
            if (timer >= DirectionChangeTimer)
            {
                // Reset the timer.
                timer = 0.0f;

                // If the dot flipped axis.
                var flipped = false;

                // Add input to the movement vector. This is adding input to a virtual stick.
                movementDirection += new Vector3(Random.Range(-moveMax * Random.Range(-1f, 1f),
                                                               moveMax * Random.Range(-1f, 1f)),
                                                 0.0f,
                                                 Random.Range(-moveMax * Random.Range(-1f, 1f),
                                                               moveMax * Random.Range(-1f, 1f)));
                var res = Random.Range(0, 100f);
                // 10% to flip vertical dir
                if (res <= 10)
                {
                    movementDirection.x *= -1;
                    flipped = true;
                }

                // 10% to flip horizontal dir
                res = Random.Range(0, 100f);
                if (!flipped && res <= 10) { movementDirection.z *= -1; }

                // 1% to flip both directions.
                if (!flipped && res <= 1)
                {
                    movementDirection.x *= -1;
                    movementDirection.z *= -1;
                }

                // Check horizontal overflow.
                if (movementDirection.x >= 1)   { movementDirection.x =  1; }
                else if (movementDirection.x <= -1 ) { movementDirection.x = -1; }
                // Check vertical overflow.
                if (movementDirection.z >= 1)   { movementDirection.z =  1; }
                else if (movementDirection.z <= -1)  { movementDirection.z = -1; }

            }

            // If the geo is in a 255 radious circle around the spawn.
            if(this.gameObject.transform.position.magnitude < 255)
            {
                ResetToSpawn = false;
            }

            if(ResetToSpawn)
            {
                Vector3 pos = transform.position;
                float speed = owner.GetSpeed();
                owner.MoveTo(pos, 2 * (Locations.SpawnLocation - pos).normalized, speed);
            }

            else
            {
                // Move the object.
                Vector3 pos = transform.position;
                float speed = owner.GetSpeed();
                owner.MoveTo(pos, movementDirection, speed, PlayerVisionLocked);
            }
        }

        // TODO- implement in child classes.
        public void Shields()
        {
            return;
        }

        // TODO- implement in child classes.
        public void Fire()
        {
            return;
        }

        public void CheckScore()
        {
            if(owner != null)
            {
                initSpeed = owner.GetSpeed();
                initDamage = owner.GetDamage();
                if(owner.GetScore() <= maxKills)
                {
                    owner.SetSpeed(initSpeed + owner.GetScore() * killWeight);
                }
            }
        }
    }
}
