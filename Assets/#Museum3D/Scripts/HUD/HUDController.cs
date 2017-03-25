using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

namespace Mus3d
{
    public class HUDController : MonoBehaviour
    {
        [Range (0f, 2f)] [SerializeField] float m_fadeTime;

        SpriteRenderer[] m_hudRenderers;

        DOGetter<Color>[] m_rendererColorGetters;
        DOSetter<Color>[] m_rendererColorSetter;

        bool m_isShowingHUD;
        bool m_isLocked = true;

        static HUDController s_instance;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize ()
        {
            InitializeHUDRenderers ();

            s_instance = this;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeHUDRenderers ()
        {
            m_hudRenderers         = GetComponentsInChildren<SpriteRenderer> ();
            m_rendererColorGetters = new DOGetter<Color> [m_hudRenderers.Length];
            m_rendererColorSetter  = new DOSetter<Color> [m_hudRenderers.Length];

            for (int i = 0; i < m_hudRenderers.Length; i++)
            {
                var renderer = m_hudRenderers[i];
                m_rendererColorGetters[i] = () => renderer.color;
                m_rendererColorSetter[i]  = color => renderer.color = color;

                renderer.color = renderer.color.WithAlpha (0f);
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Update ()
        {
            if (m_isLocked)
                return;

            if (Inp.GetDown (Inp.Key.Y))
                ToggleHUD ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void ToggleHUD ()
        {
            if (m_isShowingHUD)
                HideHUD ();
            else
                ShowHUD ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void ShowHUD () { ShowHUD (m_fadeTime); }
        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void ShowHUD (float fadeTime )
        {
            if (m_isShowingHUD)
                return;
            m_isShowingHUD = true;

            for (int i = 0; i < m_hudRenderers.Length; i++)
                DOTween.ToAlpha (m_rendererColorGetters[i], m_rendererColorSetter[i], 1f, m_fadeTime);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HideHUD () { HideHUD (m_fadeTime); }
        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HideHUD (float fadeTime)
        {
            if (!m_isShowingHUD)
                return;
            m_isShowingHUD = false;

            for (int i = 0; i < m_hudRenderers.Length; i++)
                DOTween.ToAlpha (m_rendererColorGetters[i], m_rendererColorSetter[i], 0f, m_fadeTime);
        }


        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void LockHUD ()
        {
            if (s_instance == null)
                return;

            s_instance.HideHUD (0f);
            s_instance.m_isLocked = true;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void UnlockHUD ()
        {
            if (s_instance == null)
                return;

            s_instance.m_isLocked = false;
        }
    }
}