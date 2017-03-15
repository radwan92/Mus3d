using System.Diagnostics;
using UnityEngine;

namespace Mus3d
{
    public class MusObject : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer m_spriteRenderer;
        [SerializeField] protected Transform      m_transform;

        protected Transform m_mainCameraTransform;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected virtual void Awake ()
        {
            if (m_spriteRenderer != null || m_transform == null)
                Initialize (GetComponent<SpriteRenderer> (), transform);

            m_mainCameraTransform = Camera.main.transform;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize (SpriteRenderer spriteRenderer, Transform transform)
        {
            m_spriteRenderer = spriteRenderer;
            m_transform      = transform;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected virtual void Update ()
        {
            UpdateBillBoard ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected virtual void UpdateBillBoard ()
        {
            var lookVector = -m_mainCameraTransform.forward;
            lookVector.y = 0f;

            m_transform.rotation = Quaternion.LookRotation (lookVector);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        [ContextMenu ("Setup")]
        [Conditional ("UNITY_EDITOR")]
        protected virtual void Setup ()
        {
            Awake ();
        }
    }
}