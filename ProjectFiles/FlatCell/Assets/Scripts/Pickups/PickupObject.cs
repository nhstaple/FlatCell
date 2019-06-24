﻿/*
 * 
\* Pickup.cs
 *
\* Nick S.
\* Game Logic - AI
 *
*/

using UnityEngine;
using Geo.Command;
using Utils.Vectors;

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


/*
 * TODO
 * Update documentation 6/16/19
*/

namespace Pickup.Stats
{
    public struct Pickup_Colors
    {
        public static Color Color_c = Color.grey * 0.5f;
        public static Color Speed_c = Color.cyan;
    }
}
namespace Pickup.Command
{
    public enum EPickup_Type
    {
        Default = 0,
        Health,
        Armor,
        Color,
        Speed
    }

    public class PickupObject : MonoBehaviour
    {
        protected bool DEBUG_TEXT = false;

        public EPickup_Type type { get; protected set; }

        /*
        public float speed = 0;
        public float hp = 0;
        public float armor = 0;
        public float damage = 0;
        public float piercing = 0;
        public Color color;
        */

        public float lifeTime { get; protected set; } = 4;
        public Vector3 scale { get; protected set; } = Scales.PickupScale;

        // Script variables
        protected Renderer rend;
        protected IGeo owner;

        private bool playerBoxHit = false;
        private bool playerMeshHit = false;
        protected bool playerHit = false;

        protected void Init(IGeo geo)
        {
            owner = geo;
            /*
            hp = 1 * Random.Range(LowRange, HighRange);
            armor = Random.Range(LowRange, HighRange) / 4;
            damage = 1 * Random.Range(LowRange, HighRange);
            speed = owner.GetSpeed() * Random.Range(LowRange, HighRange);
            color = owner.GetColor() * Random.Range(LowRange*1.5f, HighRange);
            */
        }
        
        protected void UpdateValues()
        {
        }

        protected float GetStatFromGeo(float geoStat, float lowRange, float highRange)
        {
            return geoStat * Random.Range(lowRange, highRange);
        }

        protected Color GetStatFromGeo(Color geoStat, float lowRange, float highRange)
        {
            return geoStat * Random.Range(lowRange, highRange);
        }

        public EPickup_Type GetType()
        {
            return type;
        }

        protected GameObject Spawn(Vector3 Location)
        {
            // Create a new projectile object.
            GameObject pickup = new GameObject("Pickup ");
            pickup.tag = "Pickup";

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

            return pickup;

            /*
            // Add the script
            PickupObject p = pickup.AddComponent<PickupObject>();
            var res = Random.Range(1, 100);
            const int ArmorChance = 10;
            const int SpeedChance = 15 + ArmorChance;
            const int HealthChance = 25 + SpeedChance;
            if (res <= ArmorChance)
            {
                if(DEBUG_TEXT) { Debug.Log("Made a armor drop!"); }
                p.Init(owner, EPickup_Type.Armor);
                rend.material.color = Color.yellow;
            }
            else if (ArmorChance < res && res <= SpeedChance)
            {
                if(DEBUG_TEXT) { Debug.Log("Made a speed drop!"); }
                p.Init(owner, EPickup_Type.Speed);
                rend.material.color = Color.cyan;
            }
            else if (SpeedChance < res && res <= HealthChance)
            {
                if(DEBUG_TEXT) { Debug.Log("Made a health drop!"); }
                p.Init(owner, EPickup_Type.Health);
                rend.material.color = Color.red;
            }
            else
            {
                if(DEBUG_TEXT) { Debug.Log("Made a color drop!"); Debug.Log(this.color); }
                p.Init(owner, EPickup_Type.Color);
                rend.material.color = Color.grey;
                rend.material.SetColor("_EmissionColor", this.color * ColorIntensity);
                rend.material.EnableKeyword("_EMISSION");
            }

            Destroy(pickup, lifeTime);
            return pickup;
            */
        }
        

        public void OnCollisionEnter(Collision geo)
        {
            playerBoxHit = false;
            playerMeshHit = false;
            playerHit = false;

            if (gameObject.GetComponent<PickupObject>().enabled)
            {
                BoxCollider box = geo.gameObject.GetComponent<BoxCollider>();
                MeshCollider mesh = geo.gameObject.GetComponent<MeshCollider>();

                if (box != null && geo.gameObject.tag == "Player")
                {
                    if(box == geo.collider)
                    {
                        playerBoxHit = true;
                    }
                }

                if (mesh != null && geo.gameObject.tag == "Player")
                {
                    if (mesh == geo.collider)
                    {
                        playerMeshHit = true;
                    }
                }

                Physics.IgnoreCollision(geo.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());

                if (geo.gameObject.tag == "Projectile")
                {
                    if(DEBUG_TEXT) { Debug.Log("passed through pickup!"); }
                    return;
                }
                else if (geo.gameObject.tag == "Player")
                {
                    if (playerMeshHit)
                    {
                        playerHit = true;
                        /*
                        IGeo p = geo.gameObject.GetComponent<PlayerController>().geo;
                        if (this.type == EPickup_Type.Health)
                        {
                            p.Heal(this.hp);
                        }
                        else if (this.type == EPickup_Type.Armor)
                        {
                            p.ModifyArmor(this.armor);
                        }
                        else if (this.type == EPickup_Type.Speed)
                        {
                            p.SetSpeed(p.GetSpeed() + this.speed);
                        }
                        else if (this.type == EPickup_Type.Color)
                        {
                            p.AddColor(this.color);
                        }
                        */
                    }
                }
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

    }

}
