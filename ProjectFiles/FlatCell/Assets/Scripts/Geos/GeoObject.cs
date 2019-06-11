﻿// GeoObject.cs
// Nick S.
// Game Logic - AI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Command;
using Projectile.Command;
using Pickup.Command;

/*
 * Geo Object
 * 
 * This is the base abastract class that implements the IGeo interface.
 * You should *NOT* make Geo Objects directly.
 * 
 
 Public
    // Init's the AI.
    void init(float Speed, float MaxHP, float FireRate, float FireChance, float ShieldChance, bool ShowTrail)
    
    // Respawns the player.
    void Respawn()

 AI

    // Moves to the position. Also updates the forward vector.
    void MoveTo(Vector3 Location, Vector3 Forward, float Step)
    
    // Shoots a bullet away from the transform.position @ Offset in transform.forward direction.
    void Shoot(float Offset)

    // Adds a kill to the goe's kill record.
    void AddKill(string name)

    // Returns the AI's fire chance
    float GetFireChance()

    // Return's the AI's shield chance
    float GetShieldChance()

    // Kills the AI, also applies a penalty to the goe's stats.
    void Kill()

    // Turns the shields on.
    void FlameOn()
    
    // Turns the shields off.
    void FlameOff()

    // Deals d damage.
    void Hurt(float d)

    // Heals h damage.
    void Heal(float h)

 Scripting
    float GetCurrentSpeed()
    Vector3 GetMovementDirection()
    float GetMovementMagnitude()
    IWeapon GetWeapon()
    Vector3 GetForward()
    void OnCollisionEnter(Collision collision)
    Shield GetShield()
    GameObject GetGameObject()

 Stats
    // Get
    float GetDamage()
    float GetArmor()
    float GetHealth()
    float GetMaxHealth()
    Color GetColor()
    float GetScore()
    float GetSpeed()
    // Set
    void AddColor(Color c)
    bool SetMaxHealth(float h)
    void ModifyArmor(float a)
    void SetDamage(float d)
    void SetSpeed(float s)
    void SetDamage(float d)

 Private
    // Adds a trail to the geo.
    void AddTrail()

    // Used for init AI colors.
    private Color ComputeRandomColor()

    void IncreaseArmor(float a)
    void DecreaseArmor(float a)
*/

namespace Geo.Command
{
    // Used to draw debug line.
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
        [SerializeField] private float DeathPenaltyLower = 0.5f;
        [SerializeField] private float DeathPenaltyUpper = 0.75f;
        [SerializeField] protected bool DrawDebugLine = false;
        [SerializeField] protected float Speed = 75;
        [SerializeField] protected float MaxSpeed = 200;
        [SerializeField] protected float BoostFactor;
        [SerializeField] protected float MaxHealth = 3;
        [SerializeField] protected float FireRate = 0.25f;
        [SerializeField] protected float Damage = 1;
        [SerializeField] protected float health = 0;
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
        [SerializeField] protected float trailDecay = 5f;
        [SerializeField] protected bool EnableTrail;
        // Used for debugging.
        LineDrawer forwardLine;
        LineDrawer movementLine;
        LineDrawer velocityLine;
        //
        // Animation manager.
        Animation anim = new Animation();
        private bool locked = false;
        [SerializeField] protected float colorRefreshPoll = 0.5f;
        [SerializeField] protected float shieldLerpTime = 1f;
        private float shieldLerpCounter = 0;
        [SerializeField] protected float geoColorLerpTime = 1f;
        private float geoColorLerpCounter = 0f;
        protected float refreshCounter = 0;
        private Color newShieldColor;
        Color initColor;

        /** Script variables **/
        [SerializeField] public TrailRenderer trail;
        protected Vector3 lastMovement;
        protected Vector3 movementDirection;
        protected float currentSpeed;
        protected Vector3 prevPos;
        protected GameObject lastHitBy;

        protected Renderer rend;

    /** Audio **/
        const string Geo_Death_Sound = "Audio/Death Sound";
        protected AudioClip deathSound;
        protected float volLowRange = 0.5F;
        protected float volHighRange = 1.0F;
        protected AudioSource deathSource;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.ToString().Contains("Projectile"))
            {
                IProjectile bullet = collision.gameObject.GetComponent<DotProjectile>();
                if(!shield.active && 
                   bullet != null &&
                   bullet.GetOwner() != null &&
                   bullet.GetOwner().GetOwner() != null)
                {
                    if(bullet.ToString().Contains("Player") && this.gameObject.tag == "Player")
                    {
                        return;
                    }
                    lastHitBy = bullet.GetOwner().GetOwner().GetGameObject();
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
            else if (collision.gameObject.tag == "Boundary")
            {
                Debug.Log("Hit the wall!");
                movementDirection.x *= -1;
                movementDirection.y *= -1;
                Vector3 pos = -1 * transform.position;
                pos.y = 0;
                pos.Normalize();

                Rigidbody body = GetComponent<Rigidbody>();
                body.AddForce(25f * pos, ForceMode.Impulse);
                // MoveTo(pos, movementDirection, Speed*Time.deltaTime);
            }
            else
            {
                Debug.Log("Hit a: " + collision.gameObject.tag);
            }
            return;
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
                initColor = color;
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
            color.a = 255;
            newShieldColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            if (pickup == null)
            {
                pickup = gameObject.AddComponent<PickupObject>();
                pickup.init(this, "Pickup");
            }
            refreshCounter = colorRefreshPoll;
            initColor = color;
        }

        public void FixedUpdate()
        {
            currentSpeed = (transform.position - prevPos).magnitude / Time.deltaTime;
            prevPos = this.transform.position;
        }

        // Update is called once per frame
        public void Update()
        {
            if (DrawDebugLine)
            {
                Vector3 pos = gameObject.transform.position;
                pos.y = 25;
                Vector3 forw = transform.forward * 25;
                forw.y = 0;
                forwardLine.DrawLineInGameView(pos + transform.forward, pos + forw, Color.yellow * 10);
            }
            refreshCounter += Time.deltaTime;
            if (health <= 0)
            {
                GameObject spawner = GameObject.FindWithTag("Spawner");
                spawner.GetComponent<DotSpawner>().Kill(this.gameObject, lastHitBy);
            }
            if (refreshCounter >= colorRefreshPoll && !shield.active)
            {
                refreshCounter = 0;
                if(this.Speed >= MaxSpeed)
                {
                    Speed = MaxSpeed;
                }
                if (!locked)
                {
                    geoColorLerpCounter = 0f;
                    float time = 1f;
                    // Lerp the material
                    // StartCoroutine(anim.lerpColor(time, rend.material, this.color, locked));
                    if (this.gameObject.tag == "Player")
                    {
                        anim.lerpColorWithoutThread(time, rend.material, this.color, ref locked);
                    }
                    else
                    {
                        anim.lerpColorWithoutThread(time, rend.material, this.initColor, ref locked);
                    }

                    this.color.a = 255;
                    refreshCounter = 0;
                    trail.startColor = Color.white;
                    trail.endColor = this.color;
                    Gradient gradient = new Gradient();
                    gradient.SetKeys(
                        new GradientColorKey[] { new GradientColorKey(trail.startColor, 1f), new GradientColorKey(trail.endColor, 0.25f) },
                        new GradientAlphaKey[] { new GradientAlphaKey(0.9f, 1f), new GradientAlphaKey(0.8f, 1f) }
                    );
                    trail.colorGradient = gradient;

                    // Set the callback
                    /*
                    StartCoroutine(anim.WaitForSecondsThenExecute(() => {

                        this.color.a = 255;
                        refreshCounter = 0;
                        trail.startColor = Color.white;
                        trail.endColor = this.color;
                        Gradient gradient = new Gradient();
                        gradient.SetKeys(
                            new GradientColorKey[] { new GradientColorKey(trail.startColor, 1f), new GradientColorKey(trail.endColor, 0.25f) },
                            new GradientAlphaKey[] { new GradientAlphaKey(0.9f, 1f), new GradientAlphaKey(0.8f, 1f) }
                        );
                        trail.colorGradient = gradient;
                                              
                    }, time*1.1f));
                    */
                }
            }
        }


        public void init(float Speed, float MaxHP, float FireRate, float FireChance, float ShieldChance, bool ShowTrail)
        {
            this.color = ComputeRandomColor();
            this.Speed = Speed;
            this.MaxHealth = MaxHP;
            this.FireRate = FireRate;
            this.FireChance = FireChance;
            this.ShieldChance = ShieldChance;
            this.EnableTrail = ShowTrail;
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
            if (killHistory.ContainsKey("Dot"))
            {
                var score = killHistory["Dot"];
                float lowerBound = DeathPenaltyLower;
                float upperBound = DeathPenaltyUpper;
                var statPenalty = Random.Range(lowerBound, upperBound);
                this.armor -= this.armor * statPenalty;
                this.Speed -= this.Speed * statPenalty * 0.50f;
                this.color -= this.color * statPenalty * 0.25f;
            }
            forwardLine.Destroy();
        }

        public void AddColor(Color c)
        {
            color = new Color(color.r + c.r, color.g + c.g, color.b + c.b, color.a);
        }

        private void AddTrail()
        {
            trail = gameObject.AddComponent<TrailRenderer>();
            trail.startColor = Color.white;
            trail.endColor = this.color;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(trail.startColor, 0.75f), new GradientColorKey(trail.endColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(0.75f, 0.0f), new GradientAlphaKey(1, 1.0f) }
            );
            trail.colorGradient = gradient;
            trail.material = new Material(Resources.Load("TrailShader", typeof(Shader)) as Shader);
            trail.enabled = true;
            trail.time = 1.0f;
            trail.startWidth = trail.startWidth * 10;
            trail.endWidth = 0;
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

        private Color ComputeRandomColor()
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

        public void MoveTo(Vector3 Location, Vector3 Forward, float Step)
        {
            // movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            Vector3 newDir = Vector3.RotateTowards(transform.forward, Forward, 360 * Mathf.Deg2Rad, 0.0f);
            // Move our position a step closer to the target.
            transform.position = Vector3.MoveTowards(transform.position, Location, Step);
            transform.rotation = Quaternion.LookRotation(newDir); ;
        }

        public Vector3 GetForward()
        {
            return transform.forward;
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
        // Called every frame.
        public void FlameOn()
        {
            if (!shield.active)
            {
                shield.TurnOn();
                rend.material.EnableKeyword("_EMISSION");
                geoColorLerpCounter = 0f;
                trail.enabled = false;
            }

            if (shield.active)
            {
                Color oldColor = rend.material.GetColor("_EmissionColor");
                // Shield color
                Color c = Color.Lerp(oldColor,
                                     newShieldColor,
                                     shieldLerpCounter / shieldLerpTime);

                // If the geo hasn't updated its color
                if(rend.material.color != Color.gray && !locked)
                {
                    geoColorLerpCounter += Time.deltaTime * 0.25f;
                    shieldLerpCounter += Time.deltaTime;
                    locked = true;
                    // Geo color.
                    if (this.gameObject.tag == "Player")
                    {
                        rend.material.color = Color.Lerp(this.color,
                                 Color.grey,
                                 geoColorLerpCounter / geoColorLerpTime);
                    }
                    else
                    {
                        rend.material.color = Color.Lerp(this.color * 0.25f + Color.grey * 0.25f,
                                 Color.grey,
                                 geoColorLerpCounter / geoColorLerpTime);
                    }
                    locked = false;
                }

                rend.material.SetColor("_EmissionColor", c);

                if (shieldLerpCounter >= shieldLerpTime)
                {
                    shieldLerpCounter = 0;
                    newShieldColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f) * Random.Range(1f, 2f);
                }

                shield.Drain(Time.deltaTime);
            }
        }

        // Turn the shields off. Clears a flag and toggles emmision field on mat
        // Called every frame.
        public void FlameOff()
        {
            if (shield.active)
            {
                shield.TurnOff();
                rend.material.DisableKeyword("_EMISSION");
                trail.enabled = true;
                shieldLerpCounter = 0;
                newShieldColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            }

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
            Kill();
            transform.position = new Vector3(0, 25, 0);
            health = MaxHealth;
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