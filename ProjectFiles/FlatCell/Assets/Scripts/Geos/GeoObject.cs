// GeoObject.cs
// Nick S.
// Game Logic - AI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Command;
using Geo.Command;
using Projectile.Command;
using Pickup.Command;

/*
 * Geo Object
 * 
 * This is the base abastract class that implements the IGeo interface.
 * You should *NOT* make Geo Objects directly.
*/

namespace Geo.Command
{

    public struct LineDrawer
    {
        private LineRenderer lineRenderer;
        private float lineSize;

        public LineDrawer(float lineSize = 0.2f)
        {
            GameObject lineObj = new GameObject("LineObj");
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            //Particles/Additive
            lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

            this.lineSize = lineSize;
        }

        private void init(float lineSize = 0.2f)
        {
            if (lineRenderer == null)
            {
                GameObject lineObj = new GameObject("LineObj");
                lineRenderer = lineObj.AddComponent<LineRenderer>();
                //Particles/Additive
                lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

                this.lineSize = lineSize;
            }
        }

        //Draws lines through the provided vertices
        public void DrawLineInGameView(Vector3 start, Vector3 end, Color color)
        {
            if (lineRenderer == null)
            {
                init(0.2f);
            }
            Debug.Log(start);
            Debug.Log(end);
            //Set color
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            //Set width
            lineRenderer.startWidth = lineSize;
            lineRenderer.endWidth = lineSize;

            //Set line count which is 2
            lineRenderer.positionCount = 2;

            //Set the postion of both two lines
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }

        public void Destroy()
        {
            if (lineRenderer != null)
            {
                UnityEngine.Object.Destroy(lineRenderer.gameObject);
            }
        }
    }

    public class GeoObject : MonoBehaviour, IGeo
    {
        /** Geo Stats **/
        [SerializeField] protected float DeathPenaltyLower = 0.5f;
        [SerializeField] protected float DeathPenaltyUpper = 0.75f;
        [SerializeField] protected float Speed = 50;
        [SerializeField] protected float BoostFactor;
        [SerializeField] protected float MaxHealth = 3;
        [SerializeField] protected float FireRate = 0.125f;
        [SerializeField] protected float Damage = 1;
        [SerializeField] protected float health;
        [SerializeField] protected float armor = 0.0f;
        [SerializeField] protected Color color = Color.clear;
        [SerializeField] protected Shield shield;
        [SerializeField] public const float InitMaxShieldEnergy = 2.0f;
        protected List<IWeapon> weapon;
        [SerializeField] public GameObject InitWeapon;
        [SerializeField] protected GameObject ActiveGun;
        [SerializeField] public IPickup pickup;

        /** Dot stats **/
        [SerializeField] protected float DotDamage = 1;
        [SerializeField] protected float DotPiercing = 0;

        /** AI vars **/
        public Dictionary<string, int> killHistory;
        [SerializeField] protected float FireChance = 35;
        [SerializeField] protected float ShieldChance = 35;

        /** Cosemetics **/
        // The force applied to the projectile. 
        [SerializeField] public float ProjectileSpawnOffset = 20f;
        [SerializeField] protected float Push = 100.0f;
        [SerializeField] protected float trailDecay = 1f;
        [SerializeField] protected bool EnableTrail;
        LineDrawer forwardLine;
        LineDrawer movementLine;
        LineDrawer velocityLine;

        /** Script variables **/
        protected TrailRenderer trail;
        protected Vector3 lastMovement;
        protected Vector3 movementDirection;
        protected float currentSpeed;
        protected Vector3 prevPos;
        protected GameObject lastHitBy;

        private float shieldLerpCounter = 0;
        private float shieldLerpTime = 1f;
        protected Renderer rend;
        const float colorRefreshPoll = 0.5f;
        protected float refreshCounter = 0;
        private Color newShieldColor;

        /** Audio **/
        const string Geo_Death_Sound = "Audio/Death Sound";
        protected AudioClip deathSound;
        protected float volLowRange = 0.5F;
        protected float volHighRange = 1.0F;
        protected AudioSource deathSource;

        public void init(float Speed, float MaxHP, float FireRate, float FireChance, float ShieldChance, bool ShowTrail)
        {
            this.color = ComputeRandomColor();
            this.Speed = Speed;
            this.MaxHealth = MaxHP;
            this.FireRate = FireRate;
            this.FireChance = FireChance;
            this.ShieldChance = ShieldChance;
            AddTrail();
            trail.enabled = ShowTrail;
            if (pickup == null)
            {
                pickup = gameObject.AddComponent<PickupObject>();
                pickup.init(this, "Pickup");
            }
        }

        public void Kill()
        {
            forwardLine.Destroy();
        }

        public void AddColor(Color c)
        {
            color = new Color(color.r + c.r, color.g + c.g, color.b + c.b, color.a);
        }

        private void AddTrail()
        {
            trail = gameObject.AddComponent<TrailRenderer>();
            trail.startColor = this.color;
            trail.endColor = Color.white;
            trail.material = new Material(Resources.Load("TrailShader", typeof(Shader)) as Shader);
            trail.enabled = true;
            trail.time = 1.0f;
        }

        public void ModifyArmor(float a)
        {
            if (a < 0)
            {
                DecreaseArmor(a);
            }
            else
            {
                IncreaseArmor(a);
            }
        }

        Color ComputeRandomColor()
        {
            Color color;
            var res = Random.Range(1, 100);
            if (1 <= res && res <= 33)
            {
                color = Color.red;
            }
            else if (res > 33 && res < 66)
            {
                color = Color.blue;
            }
            else
            {
                color = Color.green;
            }
            return color;
        }

        // Start is called before the first frame update
        public void Start()
        {
            weapon = new List<IWeapon>();

            if (InitWeapon != null)
            {
                ActiveGun = Instantiate(InitWeapon);
                IWeapon ptr = ActiveGun.GetComponents<IWeapon>()[0];
                ptr.SetOwner(this);
                weapon.Add(ptr);
            }
            forwardLine = new LineDrawer(1f);
            // Add components to game object.
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();

            // Init values. 
            currentSpeed = 0;
            health = MaxHealth;
            transform.forward = new Vector3(1, 0, 0);

            // Set material
            if (color == Color.clear)
            {
                color = ComputeRandomColor();
            }

            rend = gameObject.GetComponent<MeshRenderer>();
            rend.material = Instantiate(Resources.Load("Geo Mat", typeof(Material)) as Material);
            rend.material.color = this.color;
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            if (!trail)
            {
                AddTrail();
                trail.enabled = EnableTrail;
            }
            if (!shield)
            {
                shield = gameObject.AddComponent<Shield>();
                shield.SetMaxEnergy(InitMaxShieldEnergy);
            }
            lastHitBy = this.gameObject;
            killHistory = new Dictionary<string, int>();
            killHistory.Add("Dot", 0);
            deathSource = gameObject.AddComponent<AudioSource>();
            deathSource.enabled = true;
            deathSource.clip = Resources.Load<AudioClip>(Geo_Death_Sound);
            if (rend.material.color == null)
            {
                rend.material.color = Color.white;
            }
            newShieldColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            if (pickup == null)
            {
                pickup = gameObject.AddComponent<PickupObject>();
                pickup.init(this, "Pickup");
            }
        }

        public void FixedUpdate()
        {
            currentSpeed = (transform.position - prevPos).magnitude / Time.deltaTime;
            prevPos = this.transform.position;
        }

        // Update is called once per frame
        public void Update()
        {
            Vector3 pos = gameObject.transform.position;
            pos.y = 25;
            Vector3 forw = transform.forward * 25;
            forw.y = 0;
            forwardLine.DrawLineInGameView(pos + transform.forward, pos + forw, Color.yellow*10);
            refreshCounter += Time.deltaTime;
            if (health <= 0)
            {
                GameObject spawner = GameObject.FindWithTag("Spawner");
                spawner.GetComponent<DotSpawner>().Kill(this.gameObject, lastHitBy);
            }
            if (refreshCounter >= colorRefreshPoll)
            {
                rend.material.color = this.color;
                trail.startColor = this.color;
                trail.endColor = Color.black;
                refreshCounter = 0;
            }   
        }

        /** IGeo methods **/
        public float GetCurrentSpeed()
        {
            return currentSpeed;
        }

        public Vector3 GetMovementDirection()
        {
            return movementDirection;
        }

        public float GetMovementMagnitude()
        {
            return movementDirection.magnitude;
        }

        public Color GetColor()
        {
            return color;
        }

        public void Shoot()
        {
            Shoot(ProjectileSpawnOffset);
        }

        public void Shoot(float SpawnOffset = 20)
        {
            if (weapon.Count >= 1)
            {
                DotWeapon gun = (DotWeapon)weapon[0];
                gun.Fire(transform.forward, transform.position, Push, SpawnOffset);
            }
            else
            {
                Debug.Log("Error- no weapon assigned to " + gameObject.ToString());
            }
        }

        public void MoveTo(Vector3 Location, float Step)
        {
            movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            Vector3 newDir = Vector3.RotateTowards(transform.forward, movementDirection, 360 * Mathf.Deg2Rad, 0.0f);
            // Move our position a step closer to the target.
            transform.position = Vector3.MoveTowards(transform.position, Location, Step);
            transform.rotation = Quaternion.LookRotation(newDir); ;
        }

        public Vector3 GetForward()
        {
            return transform.forward;
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.ToString().Contains("Projectile"))
            {
                DotProjectile bullet = collision.gameObject.GetComponent<DotProjectile>();
                if (bullet != null &&
                   bullet.GetOwner() != null &&
                   bullet.GetOwner().GetOwner() != null)
                {
                    lastHitBy = bullet.GetOwner().GetOwner().GetGameObject();
                }
                if (!shield.IsActve())
                {
                    float armorBuff = armor;
                    if (armorBuff > .5f)
                    {
                        armorBuff = .5f;
                    }
                    Hurt(bullet.GetDamage() * (1 - armorBuff));
                }
                Destroy(collision.gameObject, .1f);
                // deathSource.PlayOneShot(deathSource.clip, 0.4f);
            }
            return;
        }

        public Shield GetShield()
        {
            return shield;
        }

        public GameObject GetGameObject()
        {
            if (this != null && gameObject != null)
            {
                return gameObject;
            }
            return null;
        }

        // Turn the shields on. Sets a flag and toggles emmision field on mat
        public void FlameOn()
        {
            shieldLerpCounter += Time.deltaTime;
            if (shieldLerpCounter >= shieldLerpTime)
            {
                shieldLerpCounter = 0;
                newShieldColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            }

            Color oldColor = rend.material.GetColor("_EmissionColor");

            Color c = Color.Lerp(oldColor,
                                 newShieldColor,
                                 shieldLerpCounter / shieldLerpTime);

            rend.material.SetColor("_EmissionColor", c);
            rend.material.color = Color.grey;
            rend.material.EnableKeyword("_EMISSION");
            shield.TurnOn();
            shield.Drain(Time.deltaTime);
        }

        // Turn the shields off. Clears a flag and toggles emmision field on mat
        public void FlameOff()
        {
            shield.TurnOff();
            rend.material.DisableKeyword("_EMISSION");
            rend.material.color = this.color;
            shieldLerpCounter = 0;
            newShieldColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            shield.Charge(Time.deltaTime);
        }

        public bool SetMaxHealth(float h)
        {
            if (health == MaxHealth)
            {
                MaxHealth = h;
                health = MaxHealth;
                return true;
            }
            return false;
        }

        public float GetHealth()
        {
            return health;
        }

        public float GetMaxHealth()
        {
            return MaxHealth;
        }

        public void Hurt(float d)
        {
            health -= d;
            if (health <= 0)
            {
                GameObject spawner = GameObject.FindWithTag("Spawner");
                spawner.GetComponent<DotSpawner>().Kill(this.gameObject, lastHitBy);
            }
        }

        public void Heal(float h)
        {
            if (health == MaxHealth)
            {
                MaxHealth += h;
                health = MaxHealth;
            }
            else if (health <= MaxHealth)
            {
                health += h;
            }
            if (health > MaxHealth)
            {
                health = MaxHealth;
            }
        }

        public float GetArmor()
        {
            return armor;
        }

        public void Respawn()
        {
            float lowerBound = DeathPenaltyLower;float upperBound = DeathPenaltyUpper;
            transform.position = new Vector3(0, 25, 0);
            health = MaxHealth;
            if (killHistory.ContainsKey("Dot"))
            {
                var score = killHistory["Dot"];
                var statPenalty = Random.Range(lowerBound, upperBound);

                this.armor -= this.armor * statPenalty;
                this.Speed -= this.Speed * statPenalty * 0.50f;
                this.color -= this.color * statPenalty * 0.25f;
            }
        }

        public void AddKill(string name)
        {
            if (killHistory.ContainsKey(name))
            {
                killHistory[name]++;
            }
        }

        public float GetDamage()
        {
            return Damage;
        }

        public void IncreaseArmor(float a)
        {
            armor += a;
        }
        public void DecreaseArmor(float a)
        {
            armor -= a;
            if (armor <= 0)
            {
                armor = 0;
            }
        }

        public float GetScore()
        {
            float score = 0.0f;
            if (gameObject != null)
            {
                foreach (KeyValuePair<string, int> killType in killHistory)
                {
                    score += killType.Value;
                }
            }
            return score;
        }

        public float GetSpeed()
        {
            return Speed;
        }

        public void SetDamage(float d)
        {
            Damage = d;
            if (weapon.Count > 0)
            {
                weapon[0].SetDamage(Damage, DotPiercing);
            }
        }

        public void SetSpeed(float s)
        {
            Speed = s;
        }

        public float GetShieldChance()
        {
            return ShieldChance;
        }

        public float GetFireChance()
        {
            return FireChance;
        }

        public IWeapon GetWeapon()
        {
            if (weapon.Count > 0)
            {
                return weapon[0];
            }
            return null;
        }

    }

}