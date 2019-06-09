using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pickup.Command;
using Geo.Command;

public class PickupObject : MonoBehaviour, IPickup
{
    public string type;
    public float speed = 0;
    public float hp = 0;
    public float armor = 0;
    public float damage = 0;
    public float piercing = 0;
    public float lifeTime = 3;
    public Vector3 scale = new Vector3(5f, 5f, 5f);
    protected float LowRange = 0.25f;
    protected float HighRange = 0.50f;

    /** Script variables **/
    protected float counter = -1;

    protected Renderer rend;
    protected IGeo owner;

    public void init(IGeo geo, string t)
    {
        type = t;
        owner = geo;
        hp = 1 * Random.Range(LowRange, HighRange);
        armor = Random.Range(LowRange, HighRange)/4;
        damage = 1 * Random.Range(LowRange, HighRange);
        speed = owner.GetSpeed() * Random.Range(LowRange, HighRange);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetType()
    {
        return type;
    }

    public void SetType(string t)
    {
        type = t;
    }

    public GameObject Spawn(Vector3 Location)
    {
        counter++;
        // Create a new projectile object.
        GameObject pickup = new GameObject(" Pickup " + counter);

        // Add mesh components
        rend = pickup.AddComponent<MeshRenderer>();
        MeshFilter filter = pickup.AddComponent<MeshFilter>();
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        filter.mesh = cube.GetComponent<MeshFilter>().mesh;
        GameObject.Destroy(cube);

        // Add collision components and set rotation constraints.
        pickup.AddComponent<BoxCollider>();
        Rigidbody body = pickup.AddComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        pickup.transform.position = Location;

        // Set the size/
        pickup.transform.localScale = scale;

        // Set the material
        rend.material = GameObject.Instantiate(Resources.Load("Geo Mat", typeof(Material)) as Material);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        // Add the script
        PickupObject p = pickup.AddComponent<PickupObject>();
        var res = Random.Range(1, 100);
        if(res <= 33)
        {
            p.init(owner, "Health");
            rend.material.color = Color.red;
        }
        else if(res > 33 && res < 66)
        {
            p.init(owner, "Armor");
            rend.material.color = Color.yellow;
        }
        else
        {
            p.init(owner, "Speed");
            rend.material.color = Color.cyan;
        }

        Destroy(pickup, lifeTime);
        return pickup;
    }

    public void OnCollisionEnter(Collision geo)
    {
        if(!gameObject.ToString().Contains("Geo") &&
            geo.gameObject.ToString().Contains("Player") &&
           !geo.gameObject.ToString().Contains("Projectile"))
        {
            IGeo p = geo.gameObject.GetComponent<PlayerController>();
            if(p != null)
            {
                if(this.type == "Health")
                {
                    p.Heal(this.hp);
                }
                else if (this.type == "Armor")
                {
                    p.ModifyArmor(this.armor);
                }
                else if (this.type == "Speed")
                {
                    p.SetSpeed(p.GetSpeed() + this.speed);
                }
                Destroy(this.gameObject);
            }
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

}
