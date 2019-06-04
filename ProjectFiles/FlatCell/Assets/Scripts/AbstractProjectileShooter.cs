using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractProjectileShooter : MonoBehaviour
{


    public abstract class AbstractCameraController : MonoBehaviour
    {
        [SerializeField]
        protected int ShootingStyle;
        [SerializeField]
        protected GameObject ProjectileType;

        public abstract void DrawCameraLogic();

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
