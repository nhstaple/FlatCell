using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Command;
using Geo.Command;
public class GeoObject : MonoBehaviour, IGeo
{
    /** Geo Stats **/
    [SerializeField] public float Speed = 100.0f;
    [SerializeField] public float BoostFactor = 4.0f;
    [SerializeField] public float MaxHealth = 10.0f;
    [SerializeField] public float FireRate = 0.25f;

    public float Health;
    public float Armor = 0.0f;
    public float ShieldMana = 0.0f;
    public IWeapon Weapon;

    /** Script variables **/
    public Vector3 LastMovement;
    public Vector3 MovementDirection;
    public float CurrentSpeed;
    public Vector3 PrevPos;


    // Start is called before the first frame update
    public void Start()
    {
        // Add components to game object.
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        // Init values. 
        CurrentSpeed = 0;
        Health = MaxHealth;
    }

    public void FixedUpdate()
    {
        CurrentSpeed = (transform.position - PrevPos).magnitude / Time.deltaTime;
        PrevPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /** IGeo methods **/
    public void Shoot(Vector3 Direction, Vector3 Location, float Force)
    {
        this.Weapon.Fire(Direction, Location, Force);
        return;
    }

    public float GetCurrentSpeed()
    {
        return CurrentSpeed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        /*
        Debug.Log("collided with ");
        Debug.Log(collision.gameObject.transform.parent.gameObject.ToString());
        
        bool IsProjectile = typeof(IProjectile).IsAssignableFrom(collision.gameObject.transform.parent.gameObject.GetType());
        if(IsProjectile)
        {
            Debug.Log("interface match");
            Destroy(collision.collider);
        }
        */
        return;
    }
}
