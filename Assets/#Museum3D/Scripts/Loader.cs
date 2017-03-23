using UnityEngine;

namespace Mus3d
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] GameObject m_pak;

        bool m_isLoading;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Awake ()
        {
            // Setup Mus3d secret combination
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Update ()
        {
            // TODO: Change to the secret key combination
            if (Input.GetKeyDown (KeyCode.P))
            {
                Load ();
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Load ()
        {
            if (m_isLoading)
                return;
            m_isLoading = true;

            // Dim the screen

            // Move the cam to the spot

            // Load up intro screen

            // Undim

            // Let player select the difficulty level / mission

            // Dim

            // Load up stuff

            var postRenderer = Camera.main.gameObject.AddComponent<PostRenderer> ();

            var blackScreen = Camera.main.gameObject.AddComponent<BlackScreen> ();
            blackScreen.Initialize ();

            var pak               = Instantiate (m_pak, transform);
            var musicInitializer  = pak.GetComponent<AudioInitializer> ();
            var playerInitializer = pak.GetComponent<PlayerInitializer> ();
            var enemyInitializer  = pak.GetComponent<EnemyInitializer> ();
            var hudInitializer    = pak.GetComponent<HUDInitializer> ();

            musicInitializer.Run ();
            playerInitializer.Run ();
            enemyInitializer.Run ();
            hudInitializer.Run ();
        }
    }
}