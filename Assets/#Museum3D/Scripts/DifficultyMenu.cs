using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Mus3d
{
    // TODO: Move to the appropriate place
    
    

    public class DifficultyMenu : MonoBehaviour
    {
        public static event Action<Difficulty.Level> E_DifficultySelected;

        [SerializeField] Text[]     m_difficultyLabels;
        [SerializeField] Image      m_menuPistolImage;
        [SerializeField] Image      m_faceImage;
        [SerializeField] Sprite[]   m_difficultyFaces;
        [Space]
        [SerializeField] Color m_baseTextColor;
        [SerializeField] Color m_highlightedTextColor;

        static GameObject s_gameObject;

        Difficulty.Level    m_difficulty;
        int                 m_difficultyIndex;
        bool                m_isLocked;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Show ()
        {
            UpdatePosition ();
            s_gameObject.SetActive (true);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Hide ()
        {
            s_gameObject.SetActive (false);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        static void UpdatePosition ()
        {
            var tform               = s_gameObject.transform;
            var cameraForwardNoY    = Player.BodyForward.WithY (0f).normalized;
            tform.position          = Player.Position + cameraForwardNoY * 6.5f;
            tform.rotation          = Quaternion.LookRotation (cameraForwardNoY);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Awake ()
        {
            s_gameObject = gameObject;
            SetDifficulty (Difficulty.Level.BringEmOn);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void OnEnable ()
        {
            m_isLocked = false;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Update ()
        {
            if (m_isLocked)
                return;

            if (Inp.GetDown (Inp.Key.Down))
            {
                SetNextDifficulty ();
            }

            if (Inp.GetDown (Inp.Key.Up))
            {
                SetPreviousDifficulty ();
            }

            if (Inp.GetDown (Inp.Key.A))
            {
                E_DifficultySelected (m_difficulty);
                m_isLocked = true;
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void SetDifficulty (Difficulty.Level difficulty)
        {
            m_difficultyLabels[m_difficultyIndex].color = m_baseTextColor;

            m_difficulty      = difficulty;
            m_difficultyIndex = (int)m_difficulty;

            m_faceImage.sprite = m_difficultyFaces[m_difficultyIndex];
            var difficultyLabel = m_difficultyLabels[m_difficultyIndex];
            difficultyLabel.color = m_highlightedTextColor;

            float pistolY = difficultyLabel.rectTransform.anchoredPosition.y + difficultyLabel.rectTransform.rect.height * 2;
            m_menuPistolImage.rectTransform.DOLocalMoveY (pistolY, 0.2f).SetEase (Ease.Linear);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void SetNextDifficulty ()
        {
            var difficultyIndex = (m_difficultyIndex + 1) % m_difficultyLabels.Length;
            SetDifficulty ((Difficulty.Level)difficultyIndex);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void SetPreviousDifficulty ()
        {
            var difficultyIndex = m_difficultyIndex - 1;

            if (difficultyIndex < 0)
                difficultyIndex = m_difficultyLabels.Length - 1;

            SetDifficulty ((Difficulty.Level)difficultyIndex);
        }
    }
}