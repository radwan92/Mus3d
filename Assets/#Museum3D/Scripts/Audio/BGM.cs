using System.Collections.Generic;
using UnityEngine;

namespace Mus3d
{
    public static class BGM
    {
        static List<AudioClip> m_clips = new List<AudioClip> ();

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static AudioClip GetRandom ()
        {
            return m_clips[Random.Range (0, m_clips.Count)];
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Load ()
        {
            var bgms = Resources.LoadAll <AudioClip> ("BGM");
            for (int i = 0; i < bgms.Length; i++)
            {
                var clip = bgms[i];
                m_clips.Add (clip);
            }
        }
    }
}