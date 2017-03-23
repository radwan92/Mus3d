using DG.Tweening;
using System;
using UnityEngine;

namespace Mus3d
{
    public class BlackScreen : MonoBehaviour
    {
        public static event Action E_FullTransparent;
        public static event Action E_FullBlack;

        [SerializeField] float m_fadeTime = 0.6f;

        static Material m_material;
        static Tweener  m_fade;
        static State    m_state;

        void Update ()
        {
            if (Input.GetKeyDown (KeyCode.K))
                Show ();
            if (Input.GetKeyDown (KeyCode.J))
                Hide ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize ()
        {
            m_material = new Material (Shader.Find ("Unlit/Transparent Colored"));
            m_material.color = Color.black.WithAlpha (0f);
            m_fade     = m_material.DOFade (1f, m_fadeTime).SetAutoKill (false).OnComplete (HandleFadeFinished).OnRewind (HandleFadeFinished).Pause ();

            PostRenderer.AddDraw (Render, 8);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Show ()
        {
            m_state = State.Showing;
            m_fade.PlayForward ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Hide ()
        {
            m_state = State.Hiding;
            m_fade.PlayBackwards ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandleFadeFinished ()
        {
            switch (m_state)
            {
                case State.None:
                    Debug.LogError ("Invalid state on fade finished");
                    break;
                case State.Showing:
                    E_FullBlack.InvokeIfNotNull ();
                    break;
                case State.Hiding:
                    E_FullTransparent.InvokeIfNotNull ();
                    break;
            }

            m_state = State.None;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Render ()
        {
            m_material.SetPass (0);

            GL.PushMatrix ();
            {
                GL.LoadOrtho ();
                GL.Color (m_material.color);

                GL.Begin (GL.QUADS);
                {
                    GL.Vertex3(0f, 0f, -0.5f);
                    GL.TexCoord(new Vector3 (0f, 0f, 0f));
			        GL.Vertex3(0f, 1f, -0.5f);
                    GL.TexCoord(new Vector3 (0f, 1f, 0f));
			        GL.Vertex3(1f, 1f, -0.5f);
                    GL.TexCoord(new Vector3 (1f, 1f, 0f));
			        GL.Vertex3(1f, 0f, -0.5f);
                    GL.TexCoord(new Vector3 (1f, 0f, 0f));

                }
                GL.End ();
            }
            GL.PopMatrix ();
        }

        public enum State
        {
            None,
            Showing,
            Hiding
        }
    }
}