/*
 * 
\* PlayerController.cs
 *
\* Nick S.
\* Game Logic - AI
 *
\* Kyle C.
\* TODO
 *
*/

using UnityEngine;
using Utils.Vectors;    // For Locations
using Geo.Meter;        // For Boost and Shield
using Geo.Command;
using Utils.InputManager;

/*
 * Player Controller
 * 
 * The player controller script that handles user input and stuff.
 * 
*/

namespace Controller.Player
{
    class PlayerController : MonoBehaviour // DotObject
    {
        [SerializeField] public InputManager inputManger;

        [SerializeField] protected Boost boost;
        [SerializeField] float InitSpeed = 25f;
        [SerializeField] float InitMaxHP = 3f;
        [SerializeField] float FireRate = 0.25f;
        [SerializeField] float trailDecay = 0.25f;
        [SerializeField] protected bool DrawDebugLine = true;

    // Input variables
        // The left stick / WASD.
        protected Vector3 movementDirection;
        // The right stick.
        protected Vector3 lookDir;
        // The mouse.
        protected Vector3 mouseLookDir;

        // Script variables
        private float initSpawnOffset;
        [SerializeField] public bool EnableBoost = true;
        [SerializeField] protected float BoostFactor = 2f;
        private bool initColorSet = false;
        private float modifiedSpeed;
        // Added to track the 3 moves we can use
        private int weaponSelect;

        // Evolution variables
        float EvoThrottle = 0.5f;
        float EvoCounter = 0f;

        // The current piece of geometry the player is controlling.
        public IGeo geo;

    // Cosemetics
        TrailRenderer trail;

        private void Start()
        {
            addComponents();
            grabComponents();
            initValues();
        }

        void Awake()
        {
            transform.position = Locations.SpawnLocation;
            transform.localScale = Scales.InitDotScale;
        }

        // Adds components to the object.
        private void addComponents()
        {
            geo = this.gameObject.AddComponent<DotObject>();
            geo.Init(InitSpeed, InitMaxHP, FireRate, 0, 0, true);
            geo.SetColor(Color.clear);
            inputManger = this.gameObject.AddComponent<InputManager>();
        }

        // Grabs components from the attached objects.
        private void grabComponents()
        {
            if (trail == null)
            {
                trail = this.gameObject.GetComponent<TrailRenderer>();
            }
            // TODO move to GeoObject if we want AI to be boostable.
            if(boost == null)
            {
                GameObject boop = (GameObject) Instantiate(Resources.Load("Player Items/Boost"));
                boost = boop.GetComponent<Boost>();
                boop.transform.SetParent(this.gameObject.transform);
            }
        }

        // Initial values.
        private void initValues()
        {
            weaponSelect = 1;
        }

        private void Update()
        {
            // Check the debug flags.
            checkDebug();
            // Check the player's stats and evolution.
            checkStats();

            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            modifiedSpeed = geo.GetSpeed();

            // Do input related behaviour.
            handleInput();
        }

        public void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Boundary")
            {
                Debug.Log("Player hit the wall!");
                Physics.IgnoreCollision(this.gameObject.GetComponent<BoxCollider>(),
                                        collision.gameObject.GetComponent<Collider>());
            }
            if(collision.gameObject.tag != "Terrain")
            {
                Physics.IgnoreCollision(this.gameObject.GetComponent<BoxCollider>(),
                                        collision.gameObject.GetComponent<Collider>());
            }
        }

        // Draw's the object's debug lines.
        private void checkDebug()
        {
            if (DrawDebugLine)
            {
                geo.DrawDebug(DrawDebugLine);
            }
        }

        // Does all of the input stuff. Hey Kyle!
        private void handleInput()
        {
            // TODO implement left & right bumper and immplement in InputManager
            if (Input.GetButtonDown("SortWeapon"))
            {
                Debug.Log("Switching Weapons");
                switchWeapon();
            }
            // The player pressed the boost button.
            if (inputManger.GetBoost() && !boost.charging)
            {
                modifiedSpeed *= BoostFactor;
                trail.widthMultiplier = BoostFactor;
                boost.TurnOn();
                boostedMove();
            }
            // Else the boost button isn't pressed.
            else if(!inputManger.GetBoost())
            {
                boost.TurnOff();
                // Check for shields.
                if (inputManger.GetFire2() && !geo.GetShield().IsCharging())
                {
                    geo.FlameOn();
                }
                // Check for guns.
                // If the mouse was clicked or the stick was pushed.
                else if (!geo.GetShield().active && inputManger.GetFire1(lookDir))
                {
                    // Turn off the shields.
                    geo.FlameOff();

                    // Fire me matey!
                    geo.Shoot();
                }
                // Default case- no input, use released the shield button, etc.
                else
                {
                    // Turn off the shields.
                    geo.FlameOff();

                    if (trail.widthMultiplier >= 1.0f)
                    {
                        trail.widthMultiplier -= Time.deltaTime * trailDecay * 0.5f;
                    }
                }
                // Always move.
                move();
            }
            // All other cases, includes player running out of boost.
            else
            {
                // Just keep swimming.
                move();
            }
        }

        // Grab input information and move the character.
        private void move()
        {
            // Get the input information.
            inputManger.GetSticks(ref movementDirection, ref lookDir);

            // Move the player.
            if(movementDirection.magnitude != 0 )
            {
                geo.MoveTo(transform.position, movementDirection, modifiedSpeed, false);
            }

            // Move the player's gun vector.
            // Used for controllers.
            if (lookDir.magnitude != 0)
            {
                geo.LookAt(lookDir);
            }
        }

        // Grab the movement information and apply the boosted information,
        private void boostedMove()
        {
            // Get the input information.
            inputManger.GetSticks(ref movementDirection, ref lookDir);

            movementDirection *= BoostFactor;
            geo.MoveTo(transform.position, movementDirection, modifiedSpeed, false);
            if (lookDir.magnitude != 0)
            {
                geo.LookAt(lookDir);
            }
            else if (mouseLookDir.magnitude != 0)
            {
                geo.LookAt(mouseLookDir);
            }
        }

        // TODO implment switching with LB / RB
        private void switchWeapon()
        {
            if (weaponSelect == 1)
            {
                weaponSelect = 2;
                Debug.Log("Switching to Weapon 2");
            }
            else if (weaponSelect == 2)
            {
                weaponSelect = 3;
                Debug.Log("Switching to Weapon 3");
            }
            else if (weaponSelect == 3)
            {
                weaponSelect = 1;
                Debug.Log("Switching to Weapon 1");
            }

        }

        // Checks the player's score and changes the scale of the game object.
        private void incrementEvolve()
        {
            EvoCounter += Time.deltaTime;
            if(EvoCounter >= EvoThrottle)
            {
                EvoCounter = 0f;
                var score = geo.GetScore();
                Vector3 newScale = Scales.InitDotScale;
                newScale.x += score;
                newScale.y -= score;
                newScale.z -= score;
                this.gameObject.transform.localScale = newScale;
            }
        }

        // Checks the initial color change, updates the trail decay
        private void checkStats()
        {
            if (!initColorSet)
            {
                geo.SetColor(Color.clear);
                initColorSet = true;
                trail.time = trailDecay; // move to outside of the if statement to update each frame
            }
            incrementEvolve();
        }

        // Returns the last acquired input direction applied by the user.
        public Vector3 GetMovementDirection()
        {
            return geo.GetMovementDirection();
        }
    }
}