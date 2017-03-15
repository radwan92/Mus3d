using UnityEngine;

namespace Mus3d
{
    public class HudController : MonoBehaviour
    {
        //[SerializeField] float m_hudRadius;
        //[SerializeField] float m_hudYOffset;

        //SpriteRenderer  m_hudRenderer;
        //Transform       m_headTransform;

        ///* ---------------------------------------------------------------------------------------------------------------------------------- */
        //void Awake ()
        //{
        //    m_hudRenderer   = GetComponent<SpriteRenderer> ();
        //    m_headTransform = Camera.main.transform;
        //}

        ///* ---------------------------------------------------------------------------------------------------------------------------------- */
        //void Update ()
        //{
        //    UpdatePositionAndRotation ();
        //}

        ///* ---------------------------------------------------------------------------------------------------------------------------------- */
        //void UpdatePositionAndRotation ()
        //{
        //    var forwardNoY = m_headTransform.forward;
        //    forwardNoY.y = 0f;

        //    var hudToFaceOffset = forwardNoY.normalized * m_hudRadius;
        //    hudToFaceOffset.y = m_hudYOffset;

        //    transform.position = m_headTransform.position + hudToFaceOffset;
        //    transform.rotation = Quaternion.LookRotation (-hudToFaceOffset);
        //}

        ///* ---------------------------------------------------------------------------------------------------------------------------------- */
        //public void Show ()
        //{
        //    m_hudRenderer.enabled = true;
        //}

        ///* ---------------------------------------------------------------------------------------------------------------------------------- */
        //public void Hide ()
        //{
        //    m_hudRenderer.enabled = false;
        //}
    }
}