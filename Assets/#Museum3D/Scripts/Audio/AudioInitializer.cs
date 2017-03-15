using UnityEngine;

namespace Mus3d
{
    public class AudioInitializer : MonoBehaviour
    {
        GameObject  m_musicComponentsObject;
        Music       m_music;
        Sounds      m_sounds;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Run ()
        {
            BGM.Load ();
            SFX.Load ();

            m_musicComponentsObject = new GameObject("Music");
            m_musicComponentsObject.transform.SetParent (transform);

            m_music = m_musicComponentsObject.AddComponent <Music> ();
            m_music.Initialize ();
            Music.Play (BGM.GetRandom ());

            m_sounds = m_musicComponentsObject.AddComponent<Sounds> ();
            m_sounds.Initialize ();
        }
    }
}