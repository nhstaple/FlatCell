// GeoObject.cs
// Nick S.
// Game Logic - AI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Command;
using Geo.Command;
using Projectile.Command;

/*
 * Geo Object
 * 
 * This is the base abastract class that implements the IGeo interface.
 * You should *NOT* make Geo Objects directly.
*/

public class GeoObject : MonoBehaviour, IGeo
{
    /** Geo Stats **/
    [SerializeField] protected float Speed;
    [SerializeField] protected float BoostFactor;
    [SerializeField] protected float MaxHealth;
    [SerializeField] protected float FireRate;
    [SerializeField] protected float Damage;
    protected float health;
    protected float armor = 0.0f;
    protected Shield shield;
    [SerializeField] protected float MaxShieldEnergy = 2.0f;
    protected List<IWeapon> weapon;

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

    /** Script variables **/
    protected TrailRenderer trail;
    protected Vector3 lastMovement;
    protected Vector3 movementDirection;
    protected float currentSpeed;
    protected Vector3 prevPos;
    protected GameObject lastHitBy;

    protected Color color;
    protected Renderer rend;
    const float colorRefreshPoll = 0.5f;
    protected float refreshCounter = 0;

    protected AudioSource source;

    public void init(float Speed, float MaxHP, float FireRate, float FireChance, float ShieldChance, bool ShowTrail)
    {
        this.Speed = Speed;
        this.MaxHealth = MaxHP;
        this.FireRate = FireRate;
        this.FireChance = FireChance;
        this.ShieldChance = ShieldChance;
        AddTrail();
        trail.enabled = ShowTrail;
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

        // Add components to game object.
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        // Init values. 
        currentSpeed = 0;
        health = MaxHealth;
        transform.forward = new Vector3(1, 0, 0);

        // Set material
        color = ComputeRandomColor();

        rend = gameObject.GetComponent<MeshRenderer>();
        rend.material = Instantiate(Resources.Load("Geo Mat", typeof(Material)) as Material);
        rend.material.color = this.color;
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        if(!trail)
        {
            AddTrail();
            trail.enabled = EnableTrail;
        }
        shield = ScriptableObject.CreateInstance<Shield>();
        shield.SetMaxEnergy(MaxShieldEnergy);
        lastHitBy = this.gameObject;
        killHistory = new Dictionary<string, int>();
        killHistory.Add("Dot", 0);
        source = gameObject.AddComponent<AudioSource>();
        source.enabled = true;
    }

    public void FixedUpdate()
    {
        currentSpeed = (transform.position - prevPos).magnitude / Time.deltaTime;
        prevPos = this.transform.position;
    }

    // Update is called once per frame
    public void Update()
    {
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

    public Vector3 GetForward()
    {
        return transform.forward;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.ToString().Contains("Projectile"))
        {
            DotProjectile bullet = collision.gameObject.GetComponent<DotProjectile>();
            if(bullet != null &&
               bullet.GetOwner() != null &&
               bullet.GetOwner().GetOwner() != null)
            {
                lastHitBy = bullet.GetOwner().GetOwner().GetGameObject();
            }
            if (!shield.IsActve())
            {
                Hurt(bullet.GetDamage());
            }
            Destroy(collision.gameObject, .1f);
            source = GetComponent<AudioSource>();
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
            }
            source.clip = Resources.Load<AudioClip>("Audio/Death Sound");
            source.PlayOneShot(source.clip, 0.4F);
        }            
        return;
    }

    public Shield GetShield()
    {
        return shield;
    }

    public GameObject GetGameObject()
    {
        if(this != null && gameObject != null)
        {
            return gameObject;
        }
        return null;
    }

    // Turn the shields on. Sets a flag and toggles emmision field on mat
    public void FlameOn()
    {
        shield.TurnOn();
        rend.material.EnableKeyword("_EMISSION");
        shield.Drain(Time.deltaTime);
    }

    // Turn the shields off. Clears a flag and toggles emmision field on mat
    public void FlameOff()
    {
        shield.TurnOff();
        shield.Charge(Time.deltaTime);
        rend.material.DisableKeyword("_EMISSION");
    }

    public bool SetMaxHealth(float h)
    {
        if(health == MaxHealth)
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
        if(health <= 0)
        {
            GameObject spawner = GameObject.FindWithTag("Spawner");
            spawner.GetComponent<DotSpawner>().Kill(this.gameObject, lastHitBy);
        }
    }

    public void Respawn()
    {
        const float lowerBound = 0.50f;
        const float upperBound = 0.75f;
        transform.position = new Vector3(0, 25, 0);
        health = MaxHealth;
        if (killHistory.ContainsKey("Dot"))
        {
            var score = killHistory["Dot"];
            killHistory["Dot"] = (int)UnityEngine.Random.Range(score * lowerBound, score * upperBound);
        }
    }

    public void AddKill(string name)
    {
        if(killHistory.ContainsKey(name))
        {
            killHistory[name]++;
        }
    }

    public float GetScore()
    {
        float score = 0.0f;
        foreach(KeyValuePair<string, int> killType in killHistory)
        {
            score += killType.Value;
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
        if(weapon.Count > 0)
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
        if(weapon.Count > 0)
        {
            return weapon[0];
        }
        return null;
    }

}

