﻿using DG.Tweening;
using System;
using UnityEngine;

namespace Mus3d
{
    public class BlackScreen : MonoBehaviour
    {
        public static event Action E_FullTransparent;
        public static event Action E_FullBlack;
        public static event Action E_FullTransparent_OneShot;
        public static event Action E_FullBlack_OneShot;

        [SerializeField] float m_fadeTime = 0.6f;

        static Material m_material;
        static Tweener  m_fade;
        static State    m_state;

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
            var state = m_state;
            m_state = State.None;

            switch (state)
            {
                case State.None:
                    Debug.LogError ("Invalid state on fade finished", this);
                    break;
                case State.Showing:
                    E_FullBlack.InvokeIfNotNull ();
                    E_FullBlack_OneShot.InvokeIfNotNull ();
                    E_FullBlack_OneShot = null;
                    break;
                case State.Hiding:
                    E_FullTransparent.InvokeIfNotNull ();
                    E_FullTransparent_OneShot.InvokeIfNotNull ();
                    E_FullTransparent_OneShot = null;
                    break;
            }
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
                    GL.Vertex3(0f, 0f, -1f);
                    GL.TexCoord(new Vector3 (0f, 0f, 0f));
			        GL.Vertex3(0f, 1f, -1f);
                    GL.TexCoord(new Vector3 (0f, 1f, 0f));
			        GL.Vertex3(1f, 1f, -1f);
                    GL.TexCoord(new Vector3 (1f, 1f, 0f));
			        GL.Vertex3(1f, 0f, -1f);
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