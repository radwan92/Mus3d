using UnityEngine;
using DG.Tweening;

namespace Mus3d
{
    public class FaceFlash : MonoBehaviour
    {
        static Material m_faceFlashMat;
        static Sequence m_flashAnimation;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize (Material sourceMaterial)
        {
            m_faceFlashMat        = new Material (sourceMaterial);
            m_faceFlashMat.color  = m_faceFlashMat.color.WithAlpha (0f);
            m_flashAnimation      = DOTween.Sequence ();

            m_flashAnimation.Append (m_faceFlashMat.DOFade (1f, 0.1f).SetAutoKill (false))
                .Append (m_faceFlashMat.DOFade (0f, 0.35f).SetEase (Ease.OutCubic).SetAutoKill (false))
                .SetAutoKill (false)
                .Pause ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void FlashColor (Color color)
        {
            m_faceFlashMat.color = color;
            m_faceFlashMat.mainTextureOffset = new Vector2 (Random.Range (0f, 1f), Random.Range (0f, 1f));
            m_flashAnimation.Restart ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void OnPostRender ()
        {
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
    }
}