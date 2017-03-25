using DG.Tweening;
using UnityEngine;

namespace Mus3d
{
    public class TitleText : MonoBehaviour
    {
        [SerializeField] RectTransform m_super;
        [SerializeField] RectTransform m_museum;
        [SerializeField] RectTransform m_3d;

        [SerializeField] CanvasGroup m_superCanvasGroup;
        [SerializeField] CanvasGroup m_museumCanvasGroup;
        [SerializeField] CanvasGroup m_3dCanvasGroup;

        [SerializeField] Vector3 m_superFinalPosition;
        [SerializeField] Vector3 m_museumFinalPosition;
        [SerializeField] Vector3 m_3dFinalPosition;

        static TitleText    s_instance;
        static Sequence     s_anim;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Initialize (TitleText instance)
        {
            s_instance = instance;

            s_anim = DOTween.Sequence ()
                .SetAutoKill (false)
                .Pause ()
                .SetDelay (1.5f)
                .AppendCallback (() =>
                {
                    DOTween.To (() => s_instance.m_superCanvasGroup.alpha, alpha => s_instance.m_superCanvasGroup.alpha = alpha, 1f, 0.8f);
                    FaceFlash.FlashColor (Color.white, FaceFlash.Visual.Full);
                    Sounds.Play (SFX.Source.Explosion1);
                })
                .Append (s_instance.m_super.DOLocalMove (s_instance.m_superFinalPosition, 1.5f).SetEase (Ease.OutBounce))
                .AppendCallback (() =>
                {
                    DOTween.To (() => s_instance.m_museumCanvasGroup.alpha, alpha => s_instance.m_museumCanvasGroup.alpha = alpha, 1f, 0.8f);
                    FaceFlash.FlashColor (Color.white, FaceFlash.Visual.Full);
                    Sounds.Play (SFX.Source.Explosion1);
                })
                .Append (s_instance.m_museum.DOLocalMove (s_instance.m_museumFinalPosition, 1.5f).SetEase (Ease.OutElastic))
                .AppendCallback (() =>
                {
                    DOTween.To (() => s_instance.m_3dCanvasGroup.alpha, alpha => s_instance.m_3dCanvasGroup.alpha = alpha, 1f, 0.8f);
                    FaceFlash.FlashColor (Color.white, FaceFlash.Visual.Full);
                    Sounds.Play (SFX.Source.Explosion1);
                })
                .Append (s_instance.m_3d.DOLocalMove (s_instance.m_3dFinalPosition, 1.5f).SetEase (Ease.OutBack));
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Show ()
        {
            s_anim.Play ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Hide ()
        {
            s_anim.Rewind ();
            s_instance.m_superCanvasGroup.alpha  = 0f;
            s_instance.m_museumCanvasGroup.alpha = 0f;
            s_instance.m_3dCanvasGroup.alpha     = 0f;
        }
    }
}