using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Command;
using Geo.Command;
using Projectile.Command;
using Spawner.Command;

public class Shield
{
    private float energy;
    private bool ready;
    private bool active;
    private float max;

    public Shield(float m)
    {
        energy = max = m;
        ready = true;
        active = false;
    }

    public void SetMaxEnergy(float f)
    {
        energy = max = f;
    }

    public void Drain(float e)
    {
        energy -= e;
        if (energy <= 0)
        {
            energy = 0;
            ready = false;
        }
    }

    public void Charge(float e)
    {
        energy += e;
        if(energy >= max)
        {
            energy = max;
            ready = true;
        }
    }

    public void TurnOn()
    {
        active = true;
    }

    public void TurnOff()
    {
        active = false;
    }

    public float GetEnergy()
    {
        return energy;
    }

    public float GetMaxEnergy()
    {
        return max;
    }

    public bool IsReady()
    {
        return ready;
    }

    public bool IsActve()
    {
        return active;
    }
}

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

    /** AI vars **/
    public Dictionary<string, int> killHistory;
    [SerializeField] protected float FireChance = 35;
    [SerializeField] protected float ShieldChance = 35;

    /** Cosemetics **/
    // The force applied to the projectile. 
    [SerializeField] protected float Push = 100.0f;
    [SerializeField] protected float trailDecay = 1f;

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

    public void init(float Speed, float MaxHP, float FireRate, float FireChance, float ShieldChance)
    {
        this.Speed = Speed;
        this.MaxHealth = MaxHP;
        this.FireRate = FireRate;
        this.FireChance = FireChance;
        this.ShieldChance = ShieldChance;
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
        color = Color.clear;

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

        rend = gameObject.GetComponent<MeshRenderer>();
        rend.material = Instantiate(Resources.Load("Geo Mat", typeof(Material)) as Material);
        rend.material.color = this.color;
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        trail = gameObject.AddComponent<TrailRenderer>();
        trail.startColor = this.color;
        trail.endColor = Color.white;
        trail.material = new Material(Resources.Load("TrailShader", typeof(Shader)) as Shader);
        trail.enabled = true;
        trail.time = 1.0f;
        shield = new Shield(MaxShieldEnergy);
        lastHitBy = this.gameObject;
        killHistory = new Dictionary<string, int>();
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

    public void Shoot(int WeaponIndex = 0, float SpawnOffset = 20)
    {
        if(weapon.Count > 1 && WeaponIndex < weapon.Count)
        {
            weapon[WeaponIndex].Fire(transform.forward, transform.position, Push, SpawnOffset);
        }
        else if(weapon.Count == 1)
        {
            weapon[0].Fire(transform.forward, transform.position, Push, SpawnOffset);
        }
    }

    public Vector3 GetForward()
    {
        return transform.forward;
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("collided with ");
        Debug.Log(collision.gameObject.ToString());

        if (collision.gameObject.ToString().Contains("Projectile"))
        {
            Debug.Log("interface match");
            Destroy(collision.gameObject, .1f);
            ProjectileObject bullet = collision.gameObject.GetComponent<ProjectileObject>();
            if (!shield.IsActve())
            {
                health -= bullet.GetDamage();
            }
            lastHitBy = bullet.GetOwner().GetOwner().GetGameObject();
        }            
        return;
    }

    public Shield GetShield()
    {
        return shield;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
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
        if(killHistory.ContainsKey("Dot"))
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
}
