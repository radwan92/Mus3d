using System.Collections.Generic;
using UnityEngine;

namespace Mus3d
{
    public class Sounds : MonoBehaviour
    {
        static List<AudioSource> m_audioPool = new List<AudioSource> (5);
        static GameObject m_go;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize ()
        {
            m_go = gameObject;

            for (int i = 0; i < 5; i++)
            {
                AddAudioSourceToPool ();
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Play (SFX.Source source)
        {
            var clip = SFX.Get (source);
            Play (clip);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void PlayRandom (SFX.Source[] sources)
        {
            if (sources == null || sources.Length == 0)
                return;

            int clipIndex = Random.Range (0, sources.Length);
            var clip = SFX.Get (sources[clipIndex]);
            Play (clip);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Play (AudioClip clip)
        {
            AudioSource audioSource = null;

            for (int i = 0; i < m_audioPool.Count; i++)
            {
                audioSource = m_audioPool[i];

                if (!audioSource.isPlaying)
                    break;
                audioSource = null;
            }

            audioSource = audioSource ?? AddAudioSourceToPool ();
            audioSource.clip = clip;
            audioSource.Play ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        static AudioSource AddAudioSourceToPool ()
        {
            var audioSource = m_go.AddComponent<AudioSource> ();
            audioSource.loop = false;
            m_audioPool.Add (audioSource);
            return audioSource;
        }
    }
}