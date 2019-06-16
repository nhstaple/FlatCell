// Shield.cs
// Nick S. & Kyle C.
// Game Logic - Combat

using UnityEngine;

/*
 * Shield
 * 
 * This is an abstraction for the geometry's shield.
 * 
 Public
   // Sets the max energy
   void SetMaxEnergy(float f)

   // Returns ernergy/max
   float GetPercent()

   // Adds the energy to this.energy. +/- is handle internally.
   void AddEnergy(float e)
  
   // Turns the shields on or off. ie, sets this.active
   void TurnOn()
   void TurnOff()
   
 Private
 
   // Drains energy from the shield.
   // If the energy is empty, then the shield needs to cooldown.
   void Drain(float e)

   // Adds energy back to the shield.
   // If the energy exceeds max then cap it and clear the cooldown flag
   void Charge(float e)
*/

namespace Geo.Command
{
    public class Shield : MonoBehaviour // ScriptableObject
    {
        // The shield's energy in seconds.
        [SerializeField] public float energy { get; protected set; } = 2;

        // Indicates if the shield is charging.
        [SerializeField] public bool charging { get; protected set; } = false;

        // Indicates if the shields are on.
        [SerializeField] public bool active { get; protected set; } = false;

        // The max value of the energy in seconds.
        [SerializeField] public float MaxEnergy { get; protected set; } = 2;

        // The gameobject that this script is attached to.
        protected GameObject Owner;

        // The geo object that this shield logically belongs to.
        protected IGeo owner;

        // The owner's mesh renderer.
        MeshRenderer rend;

        // The owner's trail.
        TrailRenderer trail;

        // The color of the shield.
        protected Color shieldColor;

        // The color the shield should lerp to.
        Color newShieldColor;

        // Counter used to determine when to change shield colors.
        float shieldLerpCounter = 0;

        // The time required before changing shields colors.
        [SerializeField] private float InitShieldLerpTime = 0.5f;
        float shieldLerpTime = 0f;

        protected void grabComponents()
        {
            Owner = gameObject;
            IGeo[] boop = Owner.GetComponents<IGeo>();
            if (boop.Length > 0)
            {
                owner = boop[0];
                rend = Owner.GetComponent<MeshRenderer>();
                trail = Owner.GetComponent<TrailRenderer>();
            }
            shieldLerpTime = InitShieldLerpTime * Random.Range(1, 2f);
        }

        public void Awake()
        {
            grabComponents();
        }

        public void Start()
        {
            grabComponents();
            energy = MaxEnergy;
        }

        public void Update()
        {
            // If the components are null, grab them.
            if(rend == null || trail == null || Owner == null || owner == null)
            {
                grabComponents();
            }
            // If the shield is active and not busy charging.
            if(active && !charging)
            {
                shieldLerpCounter += Time.deltaTime;
                Drain(Time.deltaTime);
                changeColor();
            }
            else
            {
                Charge(Time.deltaTime);
            }
        }

        
        public void TurnOn()
        {
            active = true;
            turnOnShieldEffect();
        }

        public void TurnOff()
        {
            active = false;
            turnOffShieldEffect();
            newShieldColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            shieldLerpCounter = 0;
        }

        public bool CheckEnergy()
        {
            if (energy < MaxEnergy)
            {
                energy = MaxEnergy;
                charging = true;
                return false;
            }
            return true;
        }

        public void ForceRecharge()
        {
            energy = MaxEnergy;
            charging = false;
        }

        public bool IsCharging()
        {
            if (energy <= 0)
            {
                charging = true;
                energy = 0;
                return charging;
            }
            else
            {
                return charging;
            }
        }

        // Drains energy from the shield.
        // If the energy is empty, then the shield needs to cooldown.
        protected void Drain(float e)
        {
            if(e > 0) { e *= -1;  }
            energy += e;
            if (energy < 0)
            {
                energy = 0;
                charging = true;
            }
        }

        // Adds energy back to the shield.
        // If the energy exceeds max then cap it and clear the cooldown flag
        protected void Charge(float e)
        {
            energy += e;
            if (energy >= MaxEnergy)
            {
                energy = MaxEnergy;
                charging = false;
            }
        }

    // Private
        private void turnOnShieldEffect()
        {
            rend.material.EnableKeyword("_EMISSION");
            trail.enabled = false;
        }

        private void turnOffShieldEffect()
        {
            rend.material.DisableKeyword("_EMISSION");
            trail.enabled = true;
        }

        private void changeColor()
        {
            if (active)
            {
                // The old color.
                Color oldColor = rend.material.GetColor("_EmissionColor");

                // The color to lerp to.
                Color c = Color.Lerp(oldColor,
                                     newShieldColor,
                                     shieldLerpCounter / shieldLerpTime);

                rend.material.SetColor("_EmissionColor", c);

                if (shieldLerpCounter >= shieldLerpTime)
                {
                    shieldLerpCounter = 0;
                    newShieldColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f) * Random.Range(1f, 2f);
                }
            }
        }
    
    // Get and Set
        public float GetMaxEnergy()
        {
            return MaxEnergy;
        }

        public float GetPercent()
        {
            return energy / MaxEnergy;
        }

        public void SetMaxEnergy(float f)
        {
            energy = MaxEnergy = f;
        }

    }
}
