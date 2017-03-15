using UnityEngine;

namespace Mus3d
{
    [ExecuteInEditMode]
    public class Billboard_Editor : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer m_spriteRenderer;
        [SerializeField] protected Transform      m_spriteTransform;

        protected Transform m_sceneCameraTransform;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        [ContextMenu ("Setup")]
        protected virtual void Awake ()
        {
            if (Application.isPlaying)
                return;

            TrySetComponents ();

            gameObject.tag = "EditorOnly";
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected virtual bool ShouldRenderObject
        {
            get { return !Application.isPlaying; }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected virtual bool AreComponentsSet
        {
            get { return m_spriteTransform != null && m_spriteRenderer != null && m_sceneCameraTransform != null; }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected virtual bool TrySetComponents ()
        {
            if (m_spriteTransform == null)
                m_spriteTransform = transform.parent;
            if (m_spriteRenderer == null && m_spriteTransform != null)
                m_spriteRenderer = m_spriteTransform.GetComponent<SpriteRenderer> ();
            if (m_sceneCameraTransform == null)
            {
                if (Camera.current != null && Camera.current.name == "SceneCamera")
                    m_sceneCameraTransform = Camera.current.transform;
            }

            return AreComponentsSet;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected virtual void OnRenderObject ()
        {
            if (!ShouldRenderObject)
                return;

            if (!AreComponentsSet && !TrySetComponents ())
                return;

            Vector3 facingVector = (Camera.current.transform.position - m_spriteTransform.position).WithY (0f);
            m_spriteTransform.rotation = Quaternion.LookRotation (facingVector);
        }
    }
}