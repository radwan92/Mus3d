using System.Diagnostics;
using UnityEngine;

namespace Mus3d
{
    public class DebugHeadController : MonoBehaviour
    {
        [SerializeField][Range (0.1f, 1f)] float m_sensitivity = 0.4f;

        static Transform m_headAnchor;

        bool m_isOn;
        Vector2 m_lastMousePosition;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize (Transform headAnchor)
        {
            m_headAnchor = headAnchor;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        [Conditional ("UNITY_EDITOR")]
        public static void Recenter ()
        {
            m_headAnchor.rotation = Quaternion.identity;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Update ()
        {
            if (Input.GetKeyDown (KeyCode.LeftAlt))
            {
                m_isOn = !m_isOn;
            }

            if (m_isOn)
                UpdateHead ();

            m_lastMousePosition = Input.mousePosition;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void UpdateHead ()
        {
            var mouseDelta = (m_lastMousePosition - (Vector2)Input.mousePosition) * m_sensitivity;

            var rot = Quaternion.Euler (mouseDelta.y, -mouseDelta.x, 0f);
            m_headAnchor.localRotation *= rot;

            var xAngle = m_headAnchor.localEulerAngles.x;
            if (xAngle < -87f)
                xAngle = -87f;
            if (xAngle > 180 && xAngle < 273f)
                xAngle = 273f;
            m_headAnchor.localEulerAngles = new Vector3 (xAngle, m_headAnchor.localEulerAngles.y, 0f);
        }
    }
}