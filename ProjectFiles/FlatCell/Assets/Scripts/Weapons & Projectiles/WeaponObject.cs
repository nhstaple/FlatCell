// WeaponObject.cs
// Nick S.
// Game Logic - Combat

using UnityEngine;
using Weapon.Command;
using Projectile.Command;
using Geo.Command;
using Controller.Player;

public class WeaponObject : MonoBehaviour, IWeapon
{
/** Weapon Stats **/
    [SerializeField] protected float FireRate;
    [SerializeField] protected float Damage;
    [SerializeField] protected float Piercing;
    [SerializeField] protected float ProjectileLifetime;
    // The owner of the weapon
    protected IGeo Owner;

    // The bullet to fire
    [SerializeField] protected IProjectile Projectile;

/** Script variables **/
    // Keeps track of the player's last input values.
    protected Vector3 lastMove;
    // Keeps track of the last time the weapon was shot.
    protected float shootCounter;

/** Audio **/
    [SerializeField] public AudioClip shootSound;
    protected float volLowRange = 0.5F;
    protected float volHighRange = 1.0F;
    protected AudioSource weaponSource;
    [SerializeField] protected float PlayerSoundPlayChance = 25;

    public void Init(IGeo GeoOwner, AudioClip Sound,
                     float Damage = 1, float Pierce = 0, float Rate = 0.125f, float lifeTime = 2.5f)
    {
        this.Owner = GeoOwner;
        this.FireRate = Rate;
        this.Damage = Damage;
        this.Piercing = Pierce;
        this.shootSound = Sound;
        if (Owner == null)
        {
            this.Owner = this.gameObject.GetComponents<IGeo>()[0];
        }
        if (weaponSource == null)
        {
            weaponSource = gameObject.AddComponent<AudioSource>();
            weaponSource.enabled = true;
        }
        this.ProjectileLifetime = lifeTime;
    }

    public void SetOwner(IGeo geo)
    {
        Owner = geo;
    }

    public void Start()
    {
        if (Owner == null)
        {
            this.Owner = this.gameObject.GetComponents<IGeo>()[0];
        }
        if (weaponSource == null)
        {
            weaponSource = gameObject.AddComponent<AudioSource>();
            weaponSource.enabled = true;
        }
        if(shootSound == null)
        {
            shootSound = Resources.Load("Audio/Shoot Sound") as AudioClip;
        }
    }

    public void Update()
    {
    }

    public void SetDamage(float Damage, float Piercing)
    {
        this.Damage = Damage;
        this.Piercing = Piercing;
    }

    protected void PlaySound()
    {
        if (weaponSource == null)
        {
            weaponSource = gameObject.AddComponent<AudioSource>();
            weaponSource.enabled = true;
        }
        if (Owner == null)
        {
            this.Owner = this.gameObject.GetComponent<PlayerController>().geo;
        }
        if (Owner.ToString().Contains("Player"))
        {
            // 25% to play sound
            if (Random.Range(1, 100) <= PlayerSoundPlayChance &&
                shootSound != null)
            {
                weaponSource.PlayOneShot(shootSound,
                         Random.Range(volLowRange, volHighRange));
            }
        }
        else
        {
            weaponSource.PlayOneShot(shootSound,
                         Random.Range(0.1f, 0.35f));
        }
    }

    public void Fire(Vector3 movementDir, Vector3 pos, float push, float SpawnOffset)
    {
        shootCounter += Time.deltaTime;
        lastMove = movementDir;
        return;
    }

    public IGeo GetOwner()
    {
        return Owner;
    }
}