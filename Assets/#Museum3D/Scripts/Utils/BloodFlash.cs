using UnityEngine;
using DG.Tweening;

namespace Mus3d
{
    public class BloodFlash : MonoBehaviour
    {
        static Material m_bloodFlashMat;
        static Sequence m_flashAnimation;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize (Material sourceMaterial)
        {
            m_bloodFlashMat       = new Material (sourceMaterial);
            m_bloodFlashMat.color = m_bloodFlashMat.color.WithAlpha (0f);
            m_flashAnimation      = DOTween.Sequence ();

            m_flashAnimation.Append (m_bloodFlashMat.DOFade (1f, 0.1f).SetAutoKill (false))
                .Append (m_bloodFlashMat.DOFade (0f, 0.5f).SetEase (Ease.OutCubic).SetAutoKill (false))
                .SetAutoKill (false)
                .Pause ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Show ()
        {
            m_bloodFlashMat.mainTextureOffset = new Vector2 (Random.Range (0f, 1f), Random.Range (0f, 1f));
            m_flashAnimation.Restart ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void OnPostRender ()
        {
            m_bloodFlashMat.SetPass (0);

            GL.PushMatrix ();
            {
                GL.LoadOrtho ();
                GL.Color (m_bloodFlashMat.color);

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