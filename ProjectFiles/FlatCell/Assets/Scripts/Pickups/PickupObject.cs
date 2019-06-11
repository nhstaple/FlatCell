// Pickup.cs
// Nick S.
// Game Logic - AI

using UnityEngine;
using Pickup.Command;
using Geo.Command;

/*
 * There's one pickup object for all types of stat drops.
 * The 
 * 
 void init(IGeo geo, string t)
 string GetType()
 void SetType(string t)
 GameObject Spawn(Vector3 Location)
 void OnCollisionEnter(Collision geo)
 void Destroy()
 * 
 * Usage
 //* From GeoObject.Start()
 if (pickup == null)
 {
     pickup = gameObject.AddComponent<PickupObject>();
     pickup.init(this, "Pickup");
 }

 //* From Spawner.Kill()
 // Spawn the stat drop.
 IPickup pickup = dead.GetComponent<PickupObject>();
 if(pickup != null)
 {
     pickup.Spawn(dead.transform.position + dead.transform.forward*10);
 }

 *
 * 
*/
public class PickupObject : MonoBehaviour, IPickup
{
    public string type;
    public float speed = 0;
    public float hp = 0;
    public float armor = 0;
    public float damage = 0;
    public float piercing = 0;
    public float lifeTime = 3;
    public Color color;
    public Vector3 scale = new Vector3(10f, 10f, 10f);
    protected float LowRange = 0.25f;
    protected float HighRange = 0.50f;
    protected float ColorIntensity = 100f;

    /** Script variables **/
    protected float counter = -1;

    protected Renderer rend;
    protected IGeo owner;

    private float colorRefresh = 0.25f;
    private float refreshCounter = 0f;

    public void init(IGeo geo, string t)
    {
        type = t;
        owner = geo;
        hp = 1 * Random.Range(LowRange, HighRange);
        armor = Random.Range(LowRange, HighRange)/4;
        damage = 1 * Random.Range(LowRange, HighRange);
        speed = owner.GetSpeed() * Random.Range(LowRange, HighRange);
        color = owner.GetColor() * Random.Range(0.01f, 0.10f);
    }
    // Start is called before the first frame update
    void Start()
    {
        if(owner != null)
        {
            color = owner.GetColor() * Random.Range(0.01f, 0.10f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        refreshCounter += Time.deltaTime;
        if (owner != null && refreshCounter >= colorRefresh)
        {
            refreshCounter = 0f;
            color = owner.GetColor() * Random.Range(0.01f, 0.10f);
        }
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
        const int ArmorChance = 10;
        const int SpeedChance = 15 + ArmorChance ;
        const int HealthChance = 25 + SpeedChance;
        if(res <= 10)
        {
            Debug.Log("Made a armor drop!");
            p.init(owner, "Armor");
            rend.material.color = Color.yellow;
        }
        else if (res <= 25)
        {
            Debug.Log("Made a speed drop!");
            p.init(owner, "Speed");
            rend.material.color = Color.cyan;
        }
        else if (res <= 50)
        {
            Debug.Log("Made a health drop!");
            p.init(owner, "Health");
            rend.material.color = Color.red;
        }
        else
        {
            Debug.Log("Made a color drop!");
            Debug.Log(this.color);
            p.init(owner, "Color");
            rend.material.color = Color.grey;
            rend.material.SetColor("_EmissionColor", this.color * ColorIntensity);
            rend.material.EnableKeyword("_EMISSION");
        }

        Destroy(pickup, lifeTime);
        return pickup;
    }

    public void OnCollisionEnter(Collision geo)
    {
        if(geo.gameObject.ToString().Contains("Projectile"))
        {
            Debug.Log("passed through pickup!");
            Physics.IgnoreCollision(geo.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        }
        else if(!gameObject.ToString().Contains("Geo") &&
                 geo.gameObject.ToString().Contains("Player"))
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
                else if (this.type == "Color")
                {
                    p.AddColor(this.color);
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
