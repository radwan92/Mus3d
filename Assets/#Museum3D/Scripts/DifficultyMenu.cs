using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Mus3d
{
    // TODO: Move to the appropriate place
    public enum Difficulty
    {
        CanIDaddy      = 0,
        DontHurtMe     = 1,
        BringEmOn      = 2,
        DeathIncarnate = 3
    }

    public class DifficultyMenu : MonoBehaviour
    {
        public static event Action<Difficulty> E_DifficultySelected;

        [SerializeField] Text[]     m_difficultyLabels;
        [SerializeField] Image      m_menuPistolImage;
        [SerializeField] Image      m_faceImage;
        [SerializeField] Sprite[]   m_difficultyFaces;
        [Space]
        [SerializeField] Color m_baseTextColor;
        [SerializeField] Color m_highlightedTextColor;

        static GameObject s_gameObject;

        Difficulty  m_difficulty;
        int         m_difficultyIndex;
        bool        m_isLocked;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Show ()
        {
            s_gameObject.SetActive (true);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Hide ()
        {
            s_gameObject.SetActive (false);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Awake ()
        {
            s_gameObject = gameObject;
            SetDifficulty (Difficulty.BringEmOn);
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
        void SetDifficulty (Difficulty difficulty)
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
            SetDifficulty ((Difficulty)difficultyIndex);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void SetPreviousDifficulty ()
        {
            var difficultyIndex = m_difficultyIndex - 1;

            if (difficultyIndex < 0)
                difficultyIndex = m_difficultyLabels.Length - 1;

            SetDifficulty ((Difficulty)difficultyIndex);
        }
    }
}