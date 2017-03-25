using UnityEngine;

namespace Mus3d
{
    public class Music : MonoBehaviour
    {
        static AudioSource m_audio;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize ()
        {
            m_audio        = gameObject.AddComponent<AudioSource> ();
            m_audio.volume = 0.7f;
            m_audio.loop   = true;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Play (AudioClip clip)
        {
            m_audio.clip = clip;
            m_audio.Play ();
        }
    }
}