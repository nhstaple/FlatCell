/*
 * 
\* DotSpawner.cs
 *
\* Nick S.
\* Game Logic - AI
 *
*/


using System.Collections.Generic;
using UnityEngine;
using Geo.Command;
using DotBehaviour.Command;
using Pickup.Command;
using AI.Command;
using Utils.Vectors;

/*
 * Dot Spawner
 * 
 * Spawns Dots as enemies. The serialized fields are passed to the AI's constructors.
 *
 Public
   // Randomly kills an AI. This mimics Darwainism.
   void Lottery()

   // Spawns a new Dot.
   void Spawn()

   // Kills a dot- removes it from the list and updates the killer's info.
   void Kill(GameObject dead, GameObject killer = null)
*/

namespace Spawner.Command
{
    public class DotSpawner : MonoBehaviour, ISpawner
    {

        bool DEBUG_TEXT = false;

        [SerializeField] public GameObject Player;
        /** Spawner Stats **/
        [SerializeField] public int NumDots = 15;
        [SerializeField] public int ArchetypeCount = 3;
        [SerializeField] public bool EnableSimple = true;
        [SerializeField] public float SimpleLowerRange = 0.66f;
        [SerializeField] public float SimpleUpperRange = 0.90f;
        [SerializeField] public bool EnableShield = true;
        [SerializeField] public float ShieldLowerRange = 0.75f;
        [SerializeField] public float ShieldUpperRange = 1.15f;
        [SerializeField] public bool EnableShooter = true;
        [SerializeField] public float ShooterLowerRange = 0.9f;
        [SerializeField] public float ShooterUpperRange = 1.25f;
        [SerializeField] public float SpawnOffset = 400f;
        [SerializeField] public Vector3 SpawnLocation = Locations.SpawnLocation;
        [SerializeField] public Vector3 InitScale = Scales.InitDotScale;
        [SerializeField] public bool EnableTrail = true;
        [SerializeField] public bool DrawDebugLine = true;

        /** Dot AI Stats **/
        [SerializeField] public float Speed = 50;
        [SerializeField] public float MaxHealth = 3;
        [SerializeField] public float FireRate = 0.05f;
        [SerializeField] public float FireChance = 25;
        [SerializeField] public float ShieldChance = 25;
        [SerializeField] public float Damage = 1;

        // Container of alive objects.
        private List<GameObject> Alive;

        // Global counter to keep track of unique dots.
        private int counter = -1;

        public void Start()
        {
            Alive = new List<GameObject>();
            transform.position = SpawnLocation;
            /*
            if (Player != null)
            {
                var boop = GameObject.Instantiate(Player, transform);
                boop.tag = "Player";
            }
            */
        }

        void Update()
        {
            Lottery(1000f);

            if (Alive.Count < NumDots)
            {
                Spawn();
            }
        }

        // .1% to kill a random dot
        public void Lottery(float cap)
        {
            if (UnityEngine.Random.Range(0, cap) <= 1 && Alive.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, Alive.Count - 1);
                Kill(Alive[index]);
            }
        }

        public void Spawn()
        {
            counter++;
            Vector3 Location = SpawnLocation;
            Location.x = UnityEngine.Random.Range(-SpawnOffset, SpawnOffset);
            Location.z = UnityEngine.Random.Range(-SpawnOffset, SpawnOffset);

            // Add a random Ai controller to the List
            int res = UnityEngine.Random.Range(1, 100);
            if (EnableSimple && res < 33)
            {
                if(DEBUG_TEXT) { Debug.Log("Spawned simple dot ai"); }
                GameObject Dot = new GameObject("Geo Simple Dot" + counter);
                Dot.transform.SetParent(this.transform);
                Dot.transform.position = Location;
                Dot.transform.localScale = InitScale * Random.Range(SimpleLowerRange, SimpleUpperRange);

                IAI ai = Dot.AddComponent<DotController>();
                ai.Init(null, Speed, MaxHealth, FireRate, FireChance, ShieldChance, EnableTrail, DrawDebugLine);
                Alive.Add(Dot);
            }
            else if (EnableShooter && res < 66)
            {
                if(DEBUG_TEXT) { Debug.Log("Spawned shooter dot ai"); }
                GameObject Dot = new GameObject("Geo Shooter Dot" + counter);
                Dot.transform.SetParent(this.transform);
                Dot.transform.position = Location;
                Dot.transform.localScale = InitScale * Random.Range(ShooterLowerRange, ShooterUpperRange);
                

                IAI ai = Dot.AddComponent<DotController>();
                ShooterDotBehaviour b = Dot.AddComponent<ShooterDotBehaviour>();
                b.Init(ai);
                ai.Init(b, Speed, MaxHealth, FireRate, FireChance, ShieldChance, EnableTrail, DrawDebugLine);
                Alive.Add(Dot);
            }
            else if (EnableShield)
            {
                if(DEBUG_TEXT) { Debug.Log("Spawned shield dot ai"); }
                GameObject Dot = new GameObject("Geo Shield Dot" + counter);
                Dot.transform.SetParent(this.transform);
                Dot.transform.position = Location;
                Dot.transform.localScale = InitScale * Random.Range(ShieldLowerRange, ShieldUpperRange);

                IAI ai = Dot.AddComponent<DotController>();
                ShieldDotBehaviour b = Dot.AddComponent<ShieldDotBehaviour>();
                b.Init(ai);
                ai.Init(b, Speed, MaxHealth, FireRate, FireChance, ShieldChance, EnableTrail, DrawDebugLine);
                Alive.Add(Dot);
            }
        }

        public void Kill(GameObject dead, GameObject killer = null)
        {
            // Update the killer's score if the thing that died was a Dot or the player.
            IGeo geo;
            if (killer != null && (
                dead.ToString().Contains("Dot") || dead.ToString().Contains("Player")
                ))
            {
                if (killer.gameObject.ToString().Contains("Simple Dot"))
                {
                    geo = killer.gameObject.GetComponent<DotController>();
                    geo.AddKill("Dot");
                }
                else if (killer.gameObject.ToString().Contains("Shield Dot"))
                {
                    geo = killer.gameObject.GetComponent<DotController>();
                    geo.AddKill("Dot");
                }
                else if (killer.gameObject.ToString().Contains("Shooter Dot"))
                {
                    geo = killer.gameObject.GetComponent<DotController>();
                    geo.AddKill("Dot");
                }
                else if (killer.gameObject.ToString().Contains("Player"))
                {
                    PlayerController p = killer.gameObject.GetComponent<PlayerController>();
                    p.geo.AddKill("Dot");
                }
            }

            if (dead.ToString().Contains("Player"))
            {
                PlayerController p = dead.gameObject.GetComponent<PlayerController>();
                p.geo.Respawn();
                Debug.Log("You died! Fool- score: " + p.geo.GetScore());
                return;
            }

            // Kill the dead object in the game.
            Alive.Remove(dead);
            IAI[] res = dead.GetComponents<IAI>();
            if (res.Length > 0)
            {
                IAI ai = res[0];
                ai.Kill();
            }

            // Spawn the stat drop.
            IPickup pickup = dead.GetComponent<PickupObject>();
            if (pickup != null)
            {
                pickup.Spawn(dead.transform.position + dead.transform.forward * 10);
            }

            Destroy(dead);
            // Log debug information.
            if (killer != null)
            {
                Debug.Log(killer.gameObject.ToString() + " killed " + dead.ToString());
            }
            //
        }
    }
}
