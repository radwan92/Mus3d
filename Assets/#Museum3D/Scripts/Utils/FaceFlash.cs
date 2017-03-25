using UnityEngine;
using DG.Tweening;

namespace Mus3d
{
    public class FaceFlash : MonoBehaviour
    {
        [SerializeField] Material   m_material;
        [SerializeField] Texture    m_noiseTexture;

        static Material m_faceFlashMat;
        static Sequence m_flashAnimation;

        static FaceFlash    s_instance;
        static bool         s_isDrawing;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize ()
        {
            s_instance = this;

            m_faceFlashMat        = new Material (m_material);
            m_faceFlashMat.color  = m_faceFlashMat.color.WithAlpha (0f);
            m_flashAnimation      = DOTween.Sequence ();

            m_flashAnimation.Append (m_faceFlashMat.DOFade (1f, 0.1f).SetAutoKill (false))
                .Append (m_faceFlashMat.DOFade (0f, 0.35f).SetEase (Ease.OutCubic).SetAutoKill (false))
                .SetAutoKill (false)
                .Pause ()
                .OnComplete (() => s_isDrawing = false);

            PostRenderer.AddDraw (Render, 5);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void FlashColor (Color color, Visual visual)
        {
            s_isDrawing = true;
            m_faceFlashMat.color = color;

            if (visual == Visual.Full)
            {
                m_faceFlashMat.mainTexture = null;
            }
            else if (visual == Visual.Noise)
            {
                m_faceFlashMat.mainTexture = s_instance.m_noiseTexture;
                m_faceFlashMat.mainTextureOffset = new Vector2 (Random.Range (0f, 1f), Random.Range (0f, 1f));
            }

            m_flashAnimation.Restart ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Render ()
        {
            if (!s_isDrawing)
                return;

            m_faceFlashMat.SetPass (0);

            GL.PushMatrix ();
            {
                GL.LoadOrtho ();
                GL.Color (m_faceFlashMat.color);

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


        public enum Visual
        {
            Full,
            Noise
        }
    }
}