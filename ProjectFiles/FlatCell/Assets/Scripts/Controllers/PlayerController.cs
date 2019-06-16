// PlayerController.cs
// Nick S.
// Game Logic

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Geo.Command;

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
        [SerializeField] Vector3 SpawnLocation = new Vector3(0, 25, 0);
        [SerializeField] float InitSpeed = 75f;
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

        private float boostEnergy = 0.5f;
        private float boostMax = 0.5f;
        private bool boostReady = true;
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

            if(boostEnergy < 0)
            {
                boostEnergy = 0;
                boostReady = false;
            }

            handleInput();

            if (!boostReady)
            {
                if (boostEnergy >= boostMax * 4)
                {
                    boostEnergy = boostMax;
                    boostReady = true;
                }
                else
                {
                    boostEnergy += Time.deltaTime;
                }
            }
        }

        void handleInput()
        {

            if (Input.GetButtonDown("SortWeapon"))
            {
                Debug.Log("Switching Weapons");
                SwitchWeapon();
            }

            if (Input.GetButton("Jump") && boostReady)
            {
                modifiedSpeed *= BoostFactor;
                trail.widthMultiplier = BoostFactor;
                boostEnergy -= Time.deltaTime;
            }
            else if (Input.GetButton("Fire2") && !geo.GetShield().IsCharging())
            {
                // Shields on.
                geo.FlameOn();
            }
            else if (Input.GetButton("Fire1") && !geo.GetShield().active)
            {
                // Fire me matey!
                geo.Shoot();
            }
            else
            {
                // Shields off.
                geo.FlameOff();
                if (trail.widthMultiplier >= 1.0f)
                {
                    trail.widthMultiplier -= Time.deltaTime * trailDecay * 0.5f;
                }
            }

            movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
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