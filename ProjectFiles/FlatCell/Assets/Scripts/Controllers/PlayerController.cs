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
        [SerializeField] protected Boost boost;
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
        private bool initColorSet = false;
    
        // The current geo type,
        public IGeo geo;
        // The trail.
        TrailRenderer trail;
        protected Vector3 movementDirection;
        protected Vector3 lookDir;
        protected Vector3 mouseLookDir;

        float EvoThrottle = 0.5f;
        float EvoCounter = 0f;

        // https://pastebin.com/yJunNcEc
        private float screenH;
        private float screenW;

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
                boop.transform.SetParent(this.gameObject.transform);
            }
        }

        private void initValues()
        {
            screenH = Screen.height / 2;
            screenW = Screen.width / 2;
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

        public void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Boundary")
            {
                Debug.Log("Player hit the wall!");
                Physics.IgnoreCollision(this.gameObject.GetComponent<BoxCollider>(), collision.gameObject.GetComponent<Collider>());
            }
            if(collision.gameObject.tag != "Terrain")
            {
                Physics.IgnoreCollision(this.gameObject.GetComponent<BoxCollider>(),
                        collision.gameObject.GetComponent<Collider>());
            }
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

        void getSticks()
        {
            lookDir = new Vector3(Input.GetAxis("Stick2X"), 0, -1 * Input.GetAxis("Stick2Y"));
            mouseLookDir = new Vector3(Input.mousePosition.x - screenW, 0, Input.mousePosition.y - screenH);
            movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            lookDir.Normalize();
            mouseLookDir.Normalize();
        }

        // Grab input information and move the character.
        private void Move()
        {
            getSticks();
            if(movementDirection.magnitude != 0 )
            {
                geo.MoveTo(transform.position, movementDirection, modifiedSpeed, false);
            }
            // Used for controllers.
            if (lookDir.magnitude != 0)
            {
                geo.LookAt(lookDir);
            }
            // Used for mouse and keyboard
            else if (mouseLookDir.magnitude != 0)
            {
                geo.LookAt(mouseLookDir);
            }
        }

        // Grab the movement information and apply the boosted information,
        private void BoostedMove()
        {
            getSticks();
            movementDirection *= BoostFactor;
            geo.MoveTo(transform.position, movementDirection, modifiedSpeed, false);
            if (lookDir.magnitude != 0)
            {
                Debug.Log(lookDir);
                geo.LookAt(lookDir);
            }
            else if (mouseLookDir.magnitude != 0)
            {
                Debug.Log(mouseLookDir);
                geo.LookAt(mouseLookDir);
            }
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

        void IncrementEvolve()
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

        private void checkStats()
        {
            if (!initColorSet)
            {
                geo.SetColor(Color.clear);
                initColorSet = true;
            }
            trail.time = trailDecay;
            IncrementEvolve();
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