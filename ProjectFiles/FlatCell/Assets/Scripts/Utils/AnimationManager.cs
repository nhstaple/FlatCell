// Animation.cs
// Nick S.

using System;
using System.Collections;
using UnityEngine;

namespace Utils.AnimationManager
{
    public class AnimationManager
    {
        // Indicates if the animator is currently busy with a thread.
        bool busy;

        public AnimationManager()
        {
            busy = false;
        }

        public void ResetLock(ref bool Lock)
        {
            Lock = false;
        }

        // Lerps material's color to value without threads.
        // FrameMulti skips frames
        // ie, frameMulti = 4 means color will be set 4 times less than normal lerp. This is to increase performance.
        public void lerpColorWithoutThread(float lerpTime, Material mat, Color targetColor, ref bool locked, float frameMulti = 2f)
        {
            if (locked == false && !busy)
            {
                locked = true;
                busy = true;
                float t = 0.0f;
                while (t <= lerpTime)
                {
                    t += frameMulti * Time.deltaTime;
                    mat.color = Color.Lerp(mat.color,
                                         targetColor,
                                         t / lerpTime);
                }
                locked = false;
                busy = false;
            }
        }

        // Runs a function after waitTime.
        // Used for calls backs 
        // source https://answers.unity.com/questions/516798/executing-an-action-after-a-coroutine-has-finished.html
        public IEnumerator WaitForSecondsThenExecute(Action method, Action callback, float waitTime = 0f, float callBackDelay = 0f)
        {
            // UnityEngine.Debug.Log("waiting " + waitTime + " seconds to run a method...");
            yield return new WaitForSeconds(waitTime);
            // UnityEngine.Debug.Log("waited " + waitTime + " seconds, now running a method...");
            method.Invoke();
            yield return new WaitForSeconds(callBackDelay);
            callback.Invoke();
        }

        // Changing the upper and lower bounds will increase the spread at which this coroutine yields.
        // Ie, makes it look different.
        public IEnumerator lerpColor(float lerpTime, Material mat, Color targetColor, bool locked = false, float frameMulti = 2f, float juiceLowerRange = 1f, float juiceUpperRange = 2f)
        {
            if (locked == false && !busy)
            {
                locked = true;
                busy = true;
                float t = 0.0f;
                while (t <= lerpTime)
                {
                    float tick = UnityEngine.Random.Range(juiceLowerRange, juiceUpperRange) * frameMulti * Time.deltaTime;
                    t += tick;
                    mat.color = Color.Lerp(mat.color,
                                         targetColor,
                                         t / lerpTime);

                    yield return new WaitForSeconds(tick);
                }
                locked = false;
                busy = false;
            }
        }

    }

}
