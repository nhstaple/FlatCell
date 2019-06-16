// PlayerController.cs
// Nick S.
// Game Logic

using UnityEngine;
using Geo.Command;
using Utils.Vectors;    // For Locations

/*
 * Player Controller
 * 
 * The player controller script that handles user input and stuff.
 * 
*/

namespace Geo.Command
{
    class PlayerController : MonoBehaviour // DotObject
    {
        [SerializeField] Vector3 SpawnLocation = Locations.SpawnLocation;
        [SerializeField] float InitSpeed = 25f;
        [SerializeField] float InitMaxHP = 3f;
        [SerializeField] float FireRate = 0.25f;
        [SerializeField] float trailDecay = 0.25f;
        [SerializeField] protected bool DrawDebugLine = true;


        /** Script variables **/
        [SerializeField] public bool EnableBoost = true;
        [SerializeField] protected float BoostFactor = 2f;
        private float modifiedSpeed;
        // Added to track the 3 moves we can use
        private int weaponSelect;

        /** Evolution variables **/
        private float initSpawnOffset;
        private bool GrowFlag = false;
        [SerializeField] protected Boost boost;
        /*
        private float boostEnergy = 0.5f;
        private float boostMax = 0.5f;
        private bool boostReady = true;
        */
        private bool initColorSet = false;
    
        // The current geo type,
        public IGeo geo;
        // The trail.
        TrailRenderer trail;
        protected Vector3 movementDirection;

        private void Start()
        {
            addComponents();
            grabComponents();
            initValues();
        }

        void Awake()
        {
            transform.position = new Vector3(0, 25, 0);
            transform.localScale = new Vector3(25, 25, 25);
        }

        private void addComponents()
        {
            geo = this.gameObject.AddComponent<DotObject>();
            geo.Init(InitSpeed, InitMaxHP, FireRate, 0, 0, true);
            geo.SetColor(Color.clear);
        }

        private void grabComponents()
        {
            if (trail == null)
            {
                trail = this.gameObject.GetComponent<TrailRenderer>();
            }
            if(boost == null)
            {
                GameObject boop = (GameObject) Instantiate(Resources.Load("Player Items/Boost"));
                boost = boop.GetComponent<Boost>();
            }
        }

        private void initValues()
        {
            weaponSelect = 1;
        }

        void checkDebug()
        {
            if(DrawDebugLine)
            {
                geo.DrawDebug(DrawDebugLine);
            }
        }

        void Update()
        {
            checkDebug();
            grabComponents();
            checkStats();

            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            modifiedSpeed = geo.GetSpeed();

            handleInput();
        }

        void handleInput()
        {

            if (Input.GetButtonDown("SortWeapon"))
            {
                Debug.Log("Switching Weapons");
                SwitchWeapon();
            }

            if (Input.GetButton("Jump") && !boost.charging)
            {
                modifiedSpeed *= BoostFactor;
                trail.widthMultiplier = BoostFactor;
                boost.TurnOn();
                BoostedMove();
            }
            else if(!Input.GetButton("Jump"))
            {
                boost.TurnOff();

                // Check for shields.
                if (Input.GetButton("Fire2") && !geo.GetShield().IsCharging())
                {
                    geo.FlameOn();
                }
                // Check for guns.
                else if (Input.GetButton("Fire1") && !geo.GetShield().active)
                {
                    // Fire me matey!
                    geo.Shoot();
                }
                // Default case- no input.
                else
                {
                    // Shields off.
                    geo.FlameOff();
                    if (trail.widthMultiplier >= 1.0f)
                    {
                        trail.widthMultiplier -= Time.deltaTime * trailDecay * 0.5f;
                    }
                }
                Move();
            }
            else
            {
                Move();
            }
        }

        // Grab input information and move the character.
        private void Move()
        {
            // Move if there's input.
            movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            geo.MoveTo(transform.position, movementDirection, modifiedSpeed);
        }

        // Grab the movement information and apply the boosted information,
        private void BoostedMove()
        {
            movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            movementDirection *= BoostFactor;
            geo.MoveTo(transform.position, movementDirection, modifiedSpeed);
        }

        void SwitchWeapon()
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

        private void checkStats()
        {
            if (!initColorSet)
            {
                geo.SetColor(Color.clear);
                initColorSet = true;
            }
            trail.time = trailDecay;
        }

        public Vector3 GetMovementDirection()
        {
            return geo.GetMovementDirection();
        }

        public float GetCurrentSpeed()
        {
            return geo.GetCurrentSpeed();
        }
    }
}