// planeCollision.cs
// Brian C.
// Game Physics
// Nick S.
// Game Feel - Animation Manager

using System.Collections;
using UnityEngine;
using Geo.Command;
using Utils.AnimationManager;

namespace Terrain
{

    public class planeCollision : MonoBehaviour
    {
        GameObject player;

        bool animLock = false;
        float animTime = 2.5f;

        [SerializeField] public AnimationManager anim;
        IEnumerator coroutine;

        void Start()
        {
            player = GameObject.FindWithTag("Player");
            animTime = UnityEngine.Random.Range(0.5f, 1f);
            anim = new AnimationManager();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Projectile")
            {
                Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(),
                                        other.gameObject.GetComponent<Collider>());
                return;
            }
            else if (other.gameObject.tag != "Projectile")
            {
                Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(),
                                        other.gameObject.GetComponent<Collider>());

                IGeo[] res = other.gameObject.GetComponents<IGeo>();
                if (res.Length > 0)
                {
                    IGeo geo = res[0];
                    if (geo != null)
                    {
                        // a primitive synch lock
                        if (animLock == false)
                        {
                            float time = UnityEngine.Random.Range(1f, 1.5f) * animTime;
                            coroutine = anim.lerpColor(time, GetComponent<Renderer>().material, geo.GetColor(), animLock, 1f, 0.5f, 2f);
                            StartCoroutine(coroutine);
                            animLock = true;

                            // Set the callback to reset the lock.
                            StartCoroutine(anim.WaitForSecondsThenExecute(() => anim.ResetLock(ref animLock), time));
                        }
                    }
                }
            }
        }

    }
}