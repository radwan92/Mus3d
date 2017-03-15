using UnityEngine;

public class HUDPositioner : MonoBehaviour
{
    [SerializeField] float m_interpolationFactor = 1f;

    Transform m_transform;
    Transform m_headTransform;

    /* ---------------------------------------------------------------------------------------------------------------------------------- */
    void Awake ()
    {
        m_transform     = GetComponent<Transform> ();
        m_headTransform = Camera.main.transform;
    }

    /* ---------------------------------------------------------------------------------------------------------------------------------- */
    void Update ()
    {
        m_transform.position = m_headTransform.position;
        m_transform.rotation = Quaternion.Slerp (m_transform.rotation, m_headTransform.rotation, Time.deltaTime * m_interpolationFactor);
    }
}