using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float Speed = 200.0f;
    [SerializeField] private float BoostFactor = 4.0f;
    [SerializeField] private float Power = 2.0f;
    //Serialized private fields flag a warning as of v2018.3. 
    //This pragma disables the warning in this one case.
#pragma warning disable 0649
    [SerializeField] private TerrainGenerator GeneratedTerrain;

    private float TrailDecay = 5.0f;
    private float ModifiedSpeed;
    private Vector3 MovementDirection;
    private TrailRenderer trail;

    void Awake()
    {
        //       this.transform.position = new Vector3(this.GeneratedTerrain.Width/2, this.GeneratedTerrain.Height/2, this.transform.position.z);
        //      this.trail = this.GetComponent<TrailRenderer>();
        //     if(this.GeneratedTerrain == null)
        //    {
        //       Debug.Log("You need pass a TrarrainGenerator component to the player.");
        //      throw new MissingComponentException();
        //  }
    }

    public float GetCurrentSpeed()
    {
        return ModifiedSpeed;
    }

    public Vector3 GetMovementDirection()
    {
        return MovementDirection;
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            //        GeneratedTerrain.ChangeTerrainHeight(gameObject.transform.position, Power);
        }
        if (Input.GetButton("Fire2"))
        {
            //          GeneratedTerrain.ChangeTerrainHeight(gameObject.transform.position, -Power);
        }

        ModifiedSpeed = Speed;
        if (Input.GetButton("Jump"))
        {
            ModifiedSpeed *= BoostFactor;
            //         trail.widthMultiplier = BoostFactor;
        }
        else
        {
            //          if (trail.widthMultiplier >= 1.0f)
            {
                //              trail.widthMultiplier -= Time.deltaTime * TrailDecay;
            }
        }
        MovementDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        gameObject.transform.Translate(MovementDirection * Time.deltaTime * ModifiedSpeed);
    }
}
