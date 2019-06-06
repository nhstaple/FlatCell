using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotController : DotObject
{
    private GameObject player; //Player object must have the tag 'Player';

    private float xMovementDir;
    private float zMovementDir;
    private Vector3 MovementDirection;

    private float timer = 0.0f;

    [SerializeField] public float DotProjectilePush = 100;
    [SerializeField] public float FireChance = 35;

    new private void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        xMovementDir = Random.Range(-1f, 1f);
        zMovementDir = Random.Range(-0.5f, 0.5f);
        movementDirection = new Vector3(xMovementDir, 0.0f, zMovementDir);
    }

    void Awake()
    {

    }

    new void Update()
    {
        base.Update();
        /*if (player.transform.position.x > transform.position.x)
        {
            //Go right 
            transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
        }
        else
        {
            //Go left 
            transform.position -= new Vector3(Speed * Time.deltaTime, 0, 0);
        }
        //Go towards Player's y 
        if (player.transform.position.y > transform.position.y)
        {
            //Go up 
            transform.position += new Vector3(0, Speed * Time.deltaTime, 0);
        }
        else
        {
            //Go down 
            transform.position -= new Vector3(0, Speed * Time.deltaTime, 0);
        }*/

        // gameObject.transform.Translate(movementDirection * Time.deltaTime * Speed);

        float step = Speed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(movementDirection.x, 0, movementDirection.z), step, 0.0f);
        // Move our position a step closer to the target.
        transform.position = Vector3.MoveTowards(transform.position, transform.position + movementDirection * step, step);
        transform.forward = newDir;

        timer += Time.deltaTime;
        if (timer >= 5f) //If 2.5 seconds reached - reset timer, change direction
        {
            xMovementDir = Random.Range(-SpawnOffset, SpawnOffset);//Code pulled from DotSpawner
            zMovementDir = Random.Range(-SpawnOffset, SpawnOffset);//change movement direction angle coords
            movementDirection = new Vector3(xMovementDir, 0.0f, zMovementDir);
            timer = 0.0f;
        }

        if(Random.Range(0, 100) <= FireChance)
        {
            if(Random.Range(0, 100) <= FireChance*2)
            {
                weapon[0].Fire(transform.forward, transform.position, DotProjectilePush, SpawnOffset);
            }
        }
    }

    new public void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        Debug.Log("collided with ");
        Debug.Log(collision.gameObject.ToString());

        if (collision.gameObject.ToString().Contains("Dot") || collision.gameObject.ToString().Contains("Player"))
        {
            Debug.Log("interface match");
            movementDirection = new Vector3(0, 0.0f, 0);
        }

        return;
    }
}