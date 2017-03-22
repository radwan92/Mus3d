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

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Start ()
        {
            InitializeHUDRenderers ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeHUDRenderers ()
        {
            m_hudRenderers         = GetComponentsInChildren<SpriteRenderer> ();
            m_rendererColorGetters = new DOGetter<Color> [m_hudRenderers.Length];
            m_rendererColorSetter  = new DOSetter<Color> [m_hudRenderers.Length];

            for (int i = 0; i < m_hudRenderers.Length; i++)
            {
                int index = i;
                m_rendererColorGetters[i] = () => m_hudRenderers[index].color;
                m_rendererColorSetter[i]  = color => m_hudRenderers[index].color = color;
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Update ()
        {
            if (Inp.GetDown (Inp.Key.Start))
            {
                ToggleHUD ();
            }
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
        void ShowHUD ()
        {
            if (m_isShowingHUD)
                return;
            m_isShowingHUD = true;

            for (int i = 0; i < m_hudRenderers.Length; i++)
                DOTween.ToAlpha (m_rendererColorGetters[i], m_rendererColorSetter[i], 1f, m_fadeTime);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HideHUD ()
        {
            if (!m_isShowingHUD)
                return;
            m_isShowingHUD = false;

            for (int i = 0; i < m_hudRenderers.Length; i++)
                DOTween.ToAlpha (m_rendererColorGetters[i], m_rendererColorSetter[i], 0f, m_fadeTime);
        }
    }
}