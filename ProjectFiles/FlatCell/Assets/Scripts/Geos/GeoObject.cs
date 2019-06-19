/*
 * 
\* GeoObject.cs
 *
\* Nick S.
\* Game Logic - AI
 *
*/

/*
 * TODO
 * Update documentation 6/16/19
 * Make damage propagate from Weapon -> Projectile spawn.
 * Remove damage from Geo.
*/


using System.Collections.Generic;
using UnityEngine;

using Weapon.Command;
using Projectile.Command;
using Pickup.Command;
using Utils.Debug;
using Utils.ADSR;
using Utils.AnimationManager;
using Spawner.Command;
using Geo.Meter;
using System.Collections;

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
    public class GeoObject : MonoBehaviour, IGeo
    {
    // Geo Stats
        [SerializeField] private float DeathPenaltyLower = 0.5f;
        [SerializeField] private float DeathPenaltyUpper = 0.75f;
        [SerializeField] protected bool DrawDebugLine = false;
        // The speed the geo currently is.
        [SerializeField] protected float Speed = 10;
        [SerializeField] const float MaxSpeed = 100;
        const float BaseSpeed = 1f;
        // Use this to tweak the max speed.
        [SerializeField] float SpeedMultiplier = 4f;
        [SerializeField] protected float FireRate = 0.25f;
        [SerializeField] protected float Damage = 1;
        [SerializeField] protected float health = 0;
        [SerializeField] protected float MaxHealth = 3;
        [SerializeField] protected float armor = 0.0f;
        [SerializeField] protected Color color = Color.clear;
        [SerializeField] protected Shield shield;
        [SerializeField] public const float InitMaxShieldEnergy = 2.0f;
        protected List<IWeapon> weapon;
        [SerializeField] public GameObject InitWeapon;
        [SerializeField] protected GameObject ActiveGun;
        [SerializeField] public IPickup pickup;

    // Dot stats
        [SerializeField] protected float DotDamage = 1;
        [SerializeField] protected float DotPiercing = 0;

    // AI vars
        public Dictionary<string, int> killHistory;
        [SerializeField] protected float FireChance = 35;
        [SerializeField] protected float ShieldChance = 35;
        float adsrCounter = 0;
        ADSR adsr;

    // Cosemetics
        // The force applied to the projectile. 
        [SerializeField] public float ProjectileSpawnOffset = 20f;
        [SerializeField] protected float Push = 10.0f;
        [SerializeField] protected float trailDecay = 2.5f;
        [SerializeField] protected float trailWidth = 5f;
        [SerializeField] protected bool EnableTrail;
        // Used for debugging.
        List<LineDrawer> Lines;
        LineDrawer forwardLine;
        LineDrawer movementLineX;
        LineDrawer movementLineZ;
        LineDrawer velocityLine;
        LineDrawer gunLine;
        Vector3 prevADSR;
        bool freezColor = true;
        IEnumerator coroutine;
        //

        // Animation manager.
        AnimationManager anim;
        private bool locked = false;
        Color initColor;

    // Script variables
        [SerializeField] public TrailRenderer trail;
        protected Vector3 lastMovement;
        protected Vector3 movementDirection;
        protected float currentSpeed;
        protected Vector3 prevPos;
        protected GameObject lastHitBy;
        protected Vector3 gunDirection;

        protected Renderer rend;

        // Audio
        const string Geo_Death_Sound = "Audio/Death Sound";
        protected AudioClip deathSound;
        protected float volLowRange = 0.5F;
        protected float volHighRange = 1.0F;
        protected AudioSource deathSource;

        // Initializers.
        // Start is called before the first frame update
        public void Start()
        {
            addComponents();
            initValues();
        }

        public void Init(float Speed, float MaxHP, float FireRate, float FireChance, float ShieldChance, bool ShowTrail)
        {
            Start();
            this.color = computeRandomColor();
            this.Speed = Speed;
            this.MaxHealth = MaxHP;
            this.FireRate = FireRate;
            this.FireChance = FireChance;
            this.ShieldChance = ShieldChance;
            this.EnableTrail = ShowTrail;
        }

        private void addComponents()
        {
            // Movement
            adsr = new ADSR();

            // Debug lines.
            Lines = new List<LineDrawer>();
            forwardLine.Destroy();
            velocityLine.Destroy();
            movementLineX.Destroy();
            movementLineZ.Destroy();
            gunLine.Destroy();

            forwardLine = new LineDrawer(this, 0.5f);
            velocityLine = new LineDrawer(this, 1f);
            movementLineX = new LineDrawer(this, 0.75f);
            movementLineZ = new LineDrawer(this, 0.75f);
            gunLine = new LineDrawer(this, 0.75f);
            Lines.Add(forwardLine);
            Lines.Add(velocityLine);
            Lines.Add(movementLineX);
            Lines.Add(movementLineZ);
            Lines.Add(gunLine);

            // Add components to game object.
            if (!GetComponent<MeshFilter>())
            {
                gameObject.AddComponent<MeshFilter>();
            }
            if (!GetComponent<MeshRenderer>())
            {
                gameObject.AddComponent<MeshRenderer>();
            }

            // Trail
            if (!trail)
            {
                addTrail();
                trail.enabled = EnableTrail;
            }
            // Shield
            if (!shield)
            {
                shield = gameObject.AddComponent<Shield>();
                shield.SetMaxEnergy(InitMaxShieldEnergy);
            }

            // Weapon
            weapon = new List<IWeapon>();
            if (InitWeapon != null)
            {
                ActiveGun = Instantiate(InitWeapon);
                ActiveGun.gameObject.transform.SetParent(this.transform);
                IWeapon ptr = ActiveGun.GetComponents<IWeapon>()[0];
                ptr.SetOwner(this);
                weapon.Add(ptr);
            }
        }

        // Sets the initial values for the geo.
        private void initValues()
        {
            // Set the stats
            currentSpeed = 0;
            health = MaxHealth;
            transform.forward = new Vector3(1, 0, 0);
            this.gunDirection = transform.forward;
            prevADSR = Vector3.zero;

            // Kill tracking
            lastHitBy = this.gameObject;
            killHistory = new Dictionary<string, int>();
            killHistory.Add("Dot", 0);

            // Material
            if (color == Color.clear)
            {
                color = computeRandomColor();
                initColor = color;
            }
            rend = gameObject.GetComponent<MeshRenderer>();
            rend.material = Instantiate(Resources.Load("Geo Mat", typeof(Material)) as Material);
            rend.material.color = this.color;
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            // Sound.
            deathSource = gameObject.AddComponent<AudioSource>();
            deathSource.enabled = true;
            deathSource.clip = Resources.Load<AudioClip>(Geo_Death_Sound);

            // Cosmetics
            initColor = color;
            anim = new AnimationManager();

            // The pickup
            if (pickup == null)
            {
                pickup = gameObject.AddComponent<PickupObject>();
                pickup.Init(this, EPickup_Type.Default);
                PickupObject cast = (PickupObject)pickup;
                cast.enabled = false;
            }
        }

        // Adds a trail renderer.
        private void addTrail()
        {
            trail = gameObject.AddComponent<TrailRenderer>();
            trail.material = new Material(Resources.Load("TrailShader", typeof(Shader)) as Shader);
            trail.enabled = true;
            trail.time = trailDecay;
            trail.startWidth = trail.startWidth * trailWidth;
            trail.endWidth = 0;
            ResetTrail();
        }

        // Computes a random color for the geo.
        private Color computeRandomColor()
        {
            Color color;
            var res = UnityEngine.Random.Range(1, 100);
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

        // Main Logic
        public void FixedUpdate()
        {
            // Update the speed if the position has changed.
            if (transform.hasChanged)
            {
                currentSpeed = (transform.position - prevPos).magnitude / Time.deltaTime;
                prevPos = this.transform.position;
            }
            else
            {
                currentSpeed = 0;
            }
        }

        public void ResetColorAfterShield(float time = 1f)
        {
            // Lerp the material
            if(!shield.active)
            {
                Color currentColor = GetComponent<Renderer>().material.color;
                var lerpCheck = (currentColor != this.color);
                if (this.gameObject.tag == "Player" && !locked && lerpCheck)
                {
                    coroutine = anim.lerpColor(time, GetComponent<Renderer>().material, this.color, locked);
                    StartCoroutine(coroutine);
                    locked = true;

                    // Set the callback to reset the lock.
                    StartCoroutine(anim.WaitForSecondsThenExecute(() => anim.ResetLock(ref locked), time));
                }
                else if (!locked && lerpCheck)
                {
                    coroutine = anim.lerpColor(time, GetComponent<Renderer>().material, this.color, locked);
                    StartCoroutine(coroutine);
                    locked = true;

                    // Set the callback to reset the lock.
                    StartCoroutine(anim.WaitForSecondsThenExecute(() => anim.ResetLock(ref locked), time));
                }
                else
                {
                    this.color.a = 255;
                    ResetTrail();
                }
            }
        }

        public bool GetAnimLock()
        {
            return locked;
        }

        // Update is called once per frame
        public void Update()
        {
            DrawDebug();
            checkStats();
        }

        // Collision logic.
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.ToString().Contains("Projectile"))
            {
                IProjectile bullet = collision.gameObject.GetComponent<DotProjectile>();
                if (!shield.active &&
                   bullet != null &&
                   bullet.GetOwner() != null &&
                   bullet.GetOwner().GetOwner() != null)
                {
                    if (bullet.ToString().Contains("Player") && this.gameObject.tag == "Player")
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

            }
            else
            {

            }
            return;
        }

        // Checks the max stats.
        private void checkStats()
        {
            if (health <= 0)
            {
                GameObject spawner = GameObject.FindWithTag("Spawner");
                spawner.GetComponent<DotSpawner>().Kill(this.gameObject, lastHitBy);
            }
            if (this.Speed >= MaxSpeed)
            {
                Speed = MaxSpeed;
            }
        }

        // Resets the trail's color.
        public void ResetTrail()
        {
            trail.startColor = 0.25f * this.color + 0.75f * Color.white;
            trail.endColor = this.color;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(trail.startColor, 1f), new GradientColorKey(trail.endColor, 0.25f) },
                new GradientAlphaKey[] { new GradientAlphaKey(0.9f, 1f), new GradientAlphaKey(0.8f, 1f) }
            );
            trail.colorGradient = gradient;
        }

        // Draw's the debug line on the forward vector.
        public void DrawDebug(bool flag = false)
        {
            var length = 15f;
            Color forc = Color.yellow * 10;
            Color gunc = Color.green * 10;
            Color movc = Color.red * 10;
            Color velc = Color.cyan * 10;
            if (DrawDebugLine || flag)
            {
                Vector3 pos = gameObject.transform.position;
                pos.y = 45;
                Vector3 forv = transform.forward * length;

                Vector3 x = new Vector3(movementDirection.x, 0, 0);
                Vector3 z = new Vector3(0, 0, movementDirection.z);
                if (x.x >= 1) { x.x = 1; }
                if (z.z >= 1) { z.z = 1; }
                if (x.x <= -1) { x.x = -1; }
                if (z.z <= -1) { z.z = -1; }

                forwardLine.DrawLineInGameView(pos, pos + forv, forc);
                movementLineX.DrawLineInGameView(pos, pos + x * x.magnitude * length, movc);
                movementLineZ.DrawLineInGameView(pos, pos + z * z.magnitude * length, movc);
                velocityLine.DrawLineInGameView(pos + -1f * prevADSR * length, pos, velc);
                gunLine.DrawLineInGameView(pos, pos + gunDirection * length, gunc);
            }
        }

        public Vector3 GetGunDir()
        {
            return gunDirection;
        }

        // AI Interface
        // Kills the geo.
        public void Kill()
        {
            if (deathSource != null && deathSource.enabled)
            {
                deathSource.PlayOneShot(deathSource.clip, 0.4f);
            }

            if (this.gameObject.tag != "Player")
            {
                foreach (LineDrawer line in Lines)
                {
                    line.Destroy();
                }
                // Turn the pickup script on.
                PickupObject cast = (PickupObject)pickup;
                cast.enabled = true;
            }
        }

        // Respawns the geo. Only to be used on player for now.
        public void Respawn()
        {
            Kill();
            PenalizeOnDeath();
            // Rest the position.
            transform.position = new Vector3(0, 25, 0);
            // Reset the health.
            health = MaxHealth;
        }

        // Applies a penalty to the geo on death.
        void PenalizeOnDeath()
        {
            if (GetScore() > 0)
            {
                float lowerBound = DeathPenaltyLower;
                float upperBound = DeathPenaltyUpper;
                var statPenalty = UnityEngine.Random.Range(lowerBound, upperBound);
                this.armor -= this.armor * statPenalty;
                this.Speed -= this.Speed * statPenalty * 0.25f;
                this.color -= this.color * statPenalty;
            }
        }

        // Fires a projectile.
        public void Shoot()
        {
            Shoot(ProjectileSpawnOffset);
        }

        public void Shoot(float SpawnOffset = 20)
        {
            if (weapon.Count >= 1)
            {
                DotWeapon gun = (DotWeapon)weapon[0];
                gun.Fire(gunDirection, transform.position, Push, SpawnOffset);
            }
            else
            {
                Debug.Log("Error- no weapon assigned to " + gameObject.ToString());
            }
        }

        // Moves to the location.
        public void MoveTo(Vector3 Position, Vector3 MovementVector, float Speed, bool ForwardLock = true)
        {
            // Set the previous movement vector.
            movementDirection = MovementVector;

            if (MovementVector.magnitude > 0)
            {
                adsrCounter += Time.deltaTime;
            }
            else
            {
                adsrCounter = 0;
            }

            // Compute the location to move to.
            Vector3 Location = Position + MovementVector * Speed;

            // Move our position a step closer to the target.
            // transform.Translate(MovementVector, Space.World);
            prevADSR = adsr.ComputeAttack(MovementVector, adsrCounter);
            transform.Translate(prevADSR * (BaseSpeed + SpeedMultiplier * (Speed / MaxSpeed)), Space.World);
            if (!ForwardLock)
            {
                transform.LookAt(Location, new Vector3(0, 1, 0));
            }
        }

        public void LookAt(Vector3 Position, bool VisionLock = false)
        {
            if(!VisionLock)
            {
                gunDirection = Position;
            }
        }

        // Turn the shields on. Sets a flag and toggles emmision field on mat
        public void FlameOn()
        {
            if (!shield.active)
            {
                shield.TurnOn();
            }
        }

        // Turn the shields off. Clears a flag and toggles emmision field on mat
        public void FlameOff()
        {
            if (shield.active)
            {
                shield.TurnOff();
            }
        }

        // Adds health to the geo's hp.
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

        // Removes health from the geo's hp.
        public void Hurt(float d)
        {
            health -= d;
            if (health <= 0)
            {
                GameObject spawner = GameObject.FindWithTag("Spawner");
                spawner.GetComponent<DotSpawner>().Kill(this.gameObject, lastHitBy);
            }
        }

        // Increments the value at key @name in killHistory<string, int>
        public void AddKill(string name)
        {
            if (killHistory.ContainsKey(name))
            {
                killHistory[name]++;
            }
        }

    // Get
        // Returns the current speed of the player. See GeoObject.FixedUpdate()
        public float GetCurrentSpeed()
        {
            return currentSpeed;
        }

        // Returns the direction the last input information the Geo provided.
        public Vector3 GetMovementDirection()
        {
            return movementDirection;
        }

        // Returnd the magnitude of the last movement vector.
        public float GetMovementMagnitude()
        {
            return movementDirection.magnitude;
        }

        // Returns the color.
        public Color GetColor()
        {
            return color;
        }

        // Returns the damage of the geo.
        public float GetDamage()
        {
            return Damage;
        }

        // Returns the aggregated score of this.killHistor<string, int>
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

        // Returns the Speed.
        public float GetSpeed()
        {
            return Speed;
        }

        public float GetMaxSpeed()
        {
            return MaxSpeed;
        }

        public float GetSpeedPercent()
        {
            return Speed / MaxSpeed;
        }

        // Returns the chance of the shield activating.
        public float GetShieldChance()
        {
            return ShieldChance;
        }

        // Retuns the chance of the weapon firing.
        public float GetFireChance()
        {
            return FireChance;
        }

        public bool ColorIsFrozen()
        {
            // Debug.Log("Frozen state: " + freezColor);
            return freezColor;
        }

        public void FreezeColor()
        {
            // Debug.Log("Frozen");
            freezColor = true;
        }

        public void ThawColor()
        {
            // Debug.Log("Thawed");
            freezColor = false;
        }

        // Returns the active weapon.
        public IWeapon GetWeapon()
        {
            if (weapon.Count > 0)
            {
                return weapon[0];
            }
            return null;
        }

        // Returns the current health.
        public float GetHealth()
        {
            return health;
        }

        // Returns the max health.
        public float GetMaxHealth()
        {
            return MaxHealth;
        }

        // Returns the percentage of the health.
        public float GetHealthPercent()
        {
            return health / MaxHealth;
        }

        // Returns the armor.
        public float GetArmor()
        {
            return armor;
        }

        // Returns the direction the object is facing, ie the euler vector.
        public Vector3 GetForward()
        {
            return transform.forward;
        }

        // Returns a copy of the shield object.
        public Shield GetShield()
        {
            return shield;
        }

        // Returns the game object.
        public GameObject GetGameObject()
        {
            if (this != null && gameObject != null)
            {
                return gameObject;
            }
            return null;
        }

    // Set & Mutators
        // Changes the color of the object.
        public void AddColor(Color c)
        {
            color += c;
        }

        // Sets the damage. TODO- setup to perculate through to IProjectile
        public void SetDamage(float d)
        {
            Damage = d;
            if (weapon.Count > 0)
            {
                weapon[0].SetDamage(Damage, DotPiercing);
            }
        }

        // Modifes the armor. Calls Increase/Decrease armor respectively.
        public void ModifyArmor(float a)
        {
            if (a < 0)
            {
                decreaseArmor(a);
            }
            else
            {
                increaseArmor(a);
            }
        }

        // Sets the movement speed of the geo
        public void SetSpeed(float s)
        {
            if(s <= MaxSpeed)
            {
                Speed = s;
            }
            else
            {
                Speed = MaxSpeed;
            }
        }

        // Sets the max health to the new value iff health >= MaxHealth
        public bool SetMaxHealth(float h)
        {
            if (health >= MaxHealth)
            {
                MaxHealth = h;
                health = MaxHealth;
                return true;
            }
            return false;
        }

        // Increases the armor
        private void increaseArmor(float a)
        {
            armor += a;
        }

        // Decreases the armor.
        private void decreaseArmor(float a)
        {
            armor -= a;
            if (armor <= 0)
            {
                armor = 0;
            }
        }

        public void SetColor(Color c)
        {
            color = c;
        }

    }

}