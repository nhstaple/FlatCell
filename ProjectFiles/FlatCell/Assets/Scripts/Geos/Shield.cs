
namespace Geo.Command
{
    public class Shield
    {
        private float energy;
        private bool ready;
        private bool active;
        private float max;

        public Shield(float m)
        {
            energy = max = m;
            ready = true;
            active = false;
        }

        public void SetMaxEnergy(float f)
        {
            energy = max = f;
        }

        public float GetPercent()
        {
            return energy / max;
        }

        // Drains energy from the shield.
        // If the energy is empty, then the shield needs to cooldown.
        public void Drain(float e)
        {
            energy -= e;
            if (energy <= 0)
            {
                energy = 0;
                ready = false;
            }
        }

        // Adds energy back to the shield.
        // If the energy exceeds max then cap it and clear the cooldown flag
        public void Charge(float e)
        {
            energy += e;
            if (energy >= max)
            {
                energy = max;
                ready = true;
            }
        }

        public void TurnOn()
        {
            active = true;
        }

        public void TurnOff()
        {
            active = false;
        }

        public float GetEnergy()
        {
            return energy;
        }

        public float GetMaxEnergy()
        {
            return max;
        }

        public bool IsReady()
        {
            return ready;
        }

        public bool IsActve()
        {
            return active;
        }
    }
}
