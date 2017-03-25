using UnityEngine;

namespace Mus3d
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] GameObject m_pak;
        [SerializeField] GameObject m_menu;
        [SerializeField] GameObject m_getPsychedController;

        bool m_isLoading;
        bool m_areGameComponentsInitialized;

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

            var postRenderer = Camera.main.gameObject.AddComponent<PostRenderer> ();
            var blackScreen  = Camera.main.gameObject.AddComponent<BlackScreen> ();
            blackScreen.Initialize ();

            BlackScreen.E_FullBlack_OneShot += () =>
            {
                InitializeMenu ();
                InitializeGetPsyched ();
                BlackScreen.Hide ();
            };
            BlackScreen.Show ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeMenu ()
        {
            var difficultyMenuGameObject = Instantiate (m_menu);
            difficultyMenuGameObject.transform.position = Camera.main.transform.forward * 3f;
            DifficultyMenu.E_DifficultySelected += HandleDifficultySet;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeGetPsyched ()
        {
            var getPsychedGameObject = Instantiate (m_getPsychedController);
            var getPsyched           = getPsychedGameObject.GetComponent<GetPsyched> ();
            getPsyched.Initialize ();
            GetPsyched.E_Finished += HandleGetPsychedFinished;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandleDifficultySet (Difficulty difficulty)
        {
            BlackScreen.E_FullBlack += () =>
            {
                DifficultyMenu.Hide ();
                GetPsyched.Show ();
            };
            BlackScreen.Show ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandleGetPsychedFinished ()
        {
            LoadGame ();
            BlackScreen.Hide ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void LoadGame ()
        {
            if (!m_areGameComponentsInitialized)
                LoadGameComponents ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void LoadGameComponents ()
        {
            var pak               = Instantiate (m_pak, transform);
            var musicInitializer  = pak.GetComponent<AudioInitializer> ();
            var playerInitializer = pak.GetComponent<PlayerInitializer> ();
            var enemyInitializer  = pak.GetComponent<EnemyInitializer> ();
            var hudInitializer    = pak.GetComponent<HUDInitializer> ();

            musicInitializer.Run ();
            playerInitializer.Run ();
            enemyInitializer.Run ();
            hudInitializer.Run ();

            m_areGameComponentsInitialized = true;
        }
    }
}