using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Command;
using Geo.Command;
using Projectile.Command;
public class GeoObject : MonoBehaviour, IGeo
{
    /** Geo Stats **/
    [SerializeField] public float Speed = 100.0f;
    [SerializeField] public float BoostFactor = 4.0f;
    [SerializeField] public float MaxHealth = 10.0f;
    [SerializeField] public float FireRate = 0.25f;

    public float health;
    public float armor = 0.0f;
    public float dhieldMana = 0.0f;
    public List<IWeapon> weapon;

    /** Cosemetics **/
    // The force applied to the projectile. 
    [SerializeField] private float Push = 100.0f;

    /** Script variables **/
    public Vector3 lastMovement;
    public Vector3 movementDirection;
    public float currentSpeed;
    public Vector3 prevPos;


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
    }

    public void FixedUpdate()
    {
        currentSpeed = (transform.position - prevPos).magnitude / Time.deltaTime;
        prevPos = this.transform.position;
    }

    // Update is called once per frame
    public void Update()
    {

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

    public void Shoot(int WeaponIndex = 0, float SpawnOffset = 15)
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

    public void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("collided with ");
        Debug.Log(collision.gameObject.ToString());

        if (collision.gameObject.ToString().Contains("Projectile"))
        {
            Debug.Log("interface match");
            Destroy(collision.gameObject, .1f);
        }
                
        return;
    }
}
