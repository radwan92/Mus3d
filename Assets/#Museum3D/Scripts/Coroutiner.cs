using System.Collections;
using UnityEngine;

namespace Mus3d
{
    public class Coroutiner : MonoBehaviour
    {
        static Coroutiner _instance;
        static Coroutiner instance
        {
            get
            {
                if (_instance == null)
                {
                    var coroutinerObject = new GameObject ("Coroutiner");
                    _instance = coroutinerObject.AddComponent<Coroutiner> ();
                }

                return _instance;
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Start (IEnumerator coroutine)
        {
            instance.StartCoroutine (coroutine);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Stop (IEnumerator coroutine)
        {
            if (coroutine != null)
                instance.StopCoroutine (coroutine);
        }
    }
}