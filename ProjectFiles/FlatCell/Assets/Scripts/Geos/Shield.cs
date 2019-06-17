/*
 * 
\* Shield.cs
 *
\* Nick S.
\* Game Logic - Game Mechanics
 *
\* Kyle C.
\* Input - Shields
 *
*/

/*
 * TODO
 * Abstract Boost and Shield to derive from a Meter class.
*/

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
    public class Shield : Meter
    {
        [SerializeField] public float InitShieldEnergy = 2f;
        [SerializeField] public float InitMaxShieldEnergy = 2f;

        // The color of the shield.
        Color shieldColor;

        // The color the shield should lerp to.
        Color newShieldColor;

        // Counter used to determine when to change shield colors.
        float shieldLerpCounter = 0;

        // The time required before changing shields colors.
        [SerializeField] float InitShieldLerpTime = 1f;
        float shieldLerpTime = 0f;

        new public void Start()
        {
            grabComponents();
            this.setVals();
        }

        new protected void setVals()
        {
            MaxEnergy = InitMaxShieldEnergy;
            base.setVals();
            shieldLerpTime = Random.Range(1f, 2f) * InitShieldLerpTime;
        }

        new public void Update()
        {
            base.Update();
            // If the shield is active and not busy charging.
            if (active && !charging)
            {
                shieldLerpCounter += Time.deltaTime;
                changeColor();
            }
        }

        new public void TurnOn()
        {
            base.TurnOn();
            turnOnShieldEffect();
        }

        new public void TurnOff()
        {
            base.TurnOff();
            turnOffShieldEffect();
            newShieldColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            shieldLerpCounter = 0;
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
                    Debug.Log("New color!");
                    shieldLerpCounter = 0;
                    newShieldColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f) * Random.Range(1f, 2f);
                }
            }
        }
    }
}
