using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.ADSR
{
    public struct ADSRfunctions
    {
        public class Function
        {
            public float duration = 0.0f;
            public Func<Vector3, float, float, Vector3> func;
            public Function(Func<Vector3, float, float, Vector3> f, float range)
            {
                func = f;
                duration = range;
            }
        }

        private static void checkMax(ref Vector3 v)
        {
            float MAX = 5;
            if (v.x >= MAX) { v.x = MAX; }
            if (v.z >= MAX) { v.z = MAX; }
        }

        private static Vector3 linearFunc(Vector3 Offset, float t, float duration)
        {
            // Debug.Log("called linear");

            bool parityFlag = false;
            Vector3 prev = Offset;
            if (prev.x < 0) { parityFlag = true; }
            prev.x = (float)Math.Pow(prev.x, 2);
            if (parityFlag) { prev.x *= -1; parityFlag = false; }

            if (prev.z < 0) { parityFlag = true; }
            prev.z = (float)Math.Pow(prev.z, 2);
            if (parityFlag) { prev.z *= -1; }

            Vector3 newPos = Vector3.Lerp(prev, Offset, t / duration);
            // checkMax(ref newPos);
            return newPos;
        }

        private static Vector3 squaredFunc(Vector3 Offset, float t, float duration)
        {
            // Debug.Log("called squared");

            bool parityFlag = false;
            if(Offset.x < 0) { parityFlag = true; }
            Offset.x = (float) Math.Pow(Offset.x, 2);
            if(parityFlag) { Offset.x *= -1; parityFlag = false; }

            if (Offset.z < 0) { parityFlag = true; }
            Offset.z = (float)Math.Pow(Offset.z, 2);
            if(parityFlag) { Offset.z *= -1; }

            Vector3 newPos = Vector3.Lerp(Offset * (float)Math.Pow(8, t), Offset, t / duration);
            // checkMax(ref newPos);
            return newPos;
        }

        private static Vector3 exponentialFunc(Vector3 Offset, float t, float duration)
        {
            // Debug.Log("called exponential");
            Offset *= (float)Math.Pow(8, t);
            Vector3 newPos = Vector3.Lerp(Vector3.zero, Offset, t / duration);
            // checkMax(ref newPos);
            return newPos;
        }

        public static Function exponential = new Function(exponentialFunc, 0.25f);
        public static Function squared = new Function(squaredFunc, .75f);
        public static Function linear = new Function(linearFunc, 1f);
    }

    public class ADSRenvelope
    {
        public List<Tuple<float, ADSRfunctions.Function>> funcs;
        public float maxTime;
        protected ADSRenvelope()
        {
            funcs = new List<Tuple<float, ADSRfunctions.Function>>();
            // Debug.Log("made an asdr envelope");
            maxTime = 2f;
        }

        protected ADSRenvelope(List<Tuple<float, ADSRfunctions.Function>> f)
        {
            funcs = f;
            // Debug.Log("made an asdr envelope");
        }
    }

    public class ADSRattack : ADSRenvelope
    {
        const float endEpoch1 = 0.25f;
        const float endEpoch2 = 1f;
        const float endEpoch3 = 2f;
        public ADSRattack()
        {
            // vDebug.Log("made an asdr attack");
            funcs.Add(new Tuple<float, ADSRfunctions.Function>(endEpoch1, ADSRfunctions.exponential));
            funcs.Add(new Tuple<float, ADSRfunctions.Function>(endEpoch2, ADSRfunctions.squared));
            funcs.Add(new Tuple<float, ADSRfunctions.Function>(endEpoch3, ADSRfunctions.linear));
        }
    }

    public class ADSRrelease : ADSRenvelope
    {
        public ADSRrelease()
        {
            // Debug.Log("made an asdr release");
        }
    }

    public class ADSR
    {
        public ADSRattack attack;
        public ADSRrelease release;

        public ADSR(ADSRattack a, ADSRrelease r)
        {
            attack = a;
            release = r;
        }

        public ADSR()
        {
            attack = new ADSRattack();
            release = new ADSRrelease();
        }

        public Vector3 ComputeAttack(Vector3 Offset, float t)
        {
            if (t >= attack.maxTime)
            {
                t = attack.maxTime;
            }

            foreach (Tuple<float, ADSRfunctions.Function> func in attack.funcs)
            {
                // If the current time is less than the duration.
                if (t < func.Item1)
                {
                    // Return it's computation.
                    return func.Item2.func(Offset, t, func.Item2.duration);
                }
                // Else keep checking the thresholds.
            }
            // Debug.Log("returned default value");
            return Offset;
        }

        public Vector3 ComputeSustain(Vector3 Offset, float t)
        {
            return Offset;
        }

        public Vector3 ComputeRelease(Vector3 Offset, float t)
        {
            return Offset;
        }
    }
}
