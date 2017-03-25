using System;
using UnityEngine;

namespace Mus3d
{
    public class Loader : MonoBehaviour
    {
        public static event Action E_ShowingDifficultyMenu;

        [SerializeField] Transform  m_levelPreviewPoint;
        [SerializeField] GameObject m_pak;
        [SerializeField] GameObject m_menu;
        [SerializeField] GameObject m_faceFlash;
        [SerializeField] GameObject m_getPsychedController;
        [SerializeField] GameObject m_level;

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
            if (Inp.GetDown (Inp.Key.A))
            {
                Debug.Log ("Loading...");
                Load ();
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Load ()
        {
            if (m_isLoading)
                return;
            m_isLoading = true;

            InitializePostRenderer ();
            InitializeBlackScreen ();

            ShowDifficultyMenu ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void ShowDifficultyMenu ()
        {
            BlackScreen.E_FullBlack_OneShot += () =>
            {
                EnsureComponentsAreLoaded ();
                Player.Recenter ();
                Player.Spawn (m_levelPreviewPoint);
                DifficultyMenu.Show ();

                BlackScreen.E_FullTransparent_OneShot += TitleText.Show;
                BlackScreen.Hide ();
            };
            BlackScreen.Show ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void ShowGetPsychedScreen (Difficulty.Level difficulty)
        {
            BlackScreen.E_FullBlack_OneShot += () =>
            {
                Player.Recenter ();
                TitleText.Hide ();
                DifficultyMenu.Hide ();
                GetPsyched.Show ();
            };
            BlackScreen.Show ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void LoadLevel ()
        {
            Player.Recenter ();
            LevelManager.LoadLevel (m_level);
            Player.Spawn (LevelManager.GetSpawnPoint ());
            MovementController.Enable ();
            WeaponController.Enable ();
            HUDController.UnlockHUD ();
            WeaponView.Show ();
            BlackScreen.Hide ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandlePlayerDeath ()
        {
            MovementController.Disable ();
            WeaponController.Disable ();

            BlackScreen.E_FullBlack_OneShot += () =>
            {
                LevelManager.UnloadLevel ();
                WeaponView.Hide ();
                HUDController.LockHUD ();
                Player.Reset ();
                Ammunition.Reset ();
                Weaponry.Reset ();
            };
            ShowDifficultyMenu ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void EnsureComponentsAreLoaded ()
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

            MovementController.Disable ();
            WeaponController.Disable ();
            WeaponView.Hide ();
            HUDController.LockHUD ();

            InitializeMainMenu ();
            InitializeGetPsyched ();
            InitializeFaceFlash ();

            Difficulty.Initialize ();
            Player.E_Died += HandlePlayerDeath;

            m_areGameComponentsInitialized = true;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializePostRenderer ()
        {
            var postRenderer = Camera.main.gameObject.AddComponent<PostRenderer> ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeBlackScreen ()
        {
            var blackScreen  = Camera.main.gameObject.AddComponent<BlackScreen> ();
            blackScreen.Initialize ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeMainMenu ()
        {
            var difficultyMenuGameObject = Instantiate (m_menu);
            difficultyMenuGameObject.transform.position = Camera.main.transform.forward * 3f;
            DifficultyMenu.Hide ();
            DifficultyMenu.E_DifficultySelected += ShowGetPsychedScreen;

            var titleText = difficultyMenuGameObject.GetComponentInChildren<TitleText> ();
            TitleText.Initialize (titleText);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeGetPsyched ()
        {
            var getPsychedGameObject = Instantiate (m_getPsychedController);
            var getPsyched           = getPsychedGameObject.GetComponent<GetPsyched> ();
            getPsyched.Initialize ();
            GetPsyched.E_Finished += LoadLevel;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeFaceFlash ()
        {
            var faceFlashObject = Instantiate (m_faceFlash);
            var faceFlash       = faceFlashObject.GetComponent<FaceFlash> ();

            faceFlash.Initialize ();
        }
    }
}