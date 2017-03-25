using UnityEngine;

namespace Mus3d
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] float m_speedMultiplier = 3.5f;
        [SerializeField] float m_rotationAngle = 45f;

        CharacterController m_controller;
        Transform           m_head;
        Transform           m_body;

        bool m_hasRotatedLeft;
        bool m_hasRotatedRight;

        static bool s_isEnabled;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize (CharacterController characterController, Transform head, Transform body)
        {
            m_controller = characterController;
            m_head       = head;
            m_body       = body;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Enable ()
        {
            s_isEnabled = true;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Disable ()
        {
            s_isEnabled = false;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Update ()
        {
            if (!s_isEnabled)
                return;

            HandleMovementInput ();
            HandleRotationInput ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandleRotationInput ()
        {
            float rtValue = Inp.Get (Inp.Axis.RT);
            float ltValue = Inp.Get (Inp.Axis.LT);

            if (ltValue > 0.5f)
            {
                if (!m_hasRotatedLeft)
                {
                    m_hasRotatedLeft = true;
                    m_body.Rotate (Vector3.up, -m_rotationAngle, Space.World);
                }
            }
            else if (ltValue < 0.3f)
            {
                m_hasRotatedLeft = false;
            }

            if (rtValue > 0.5f)
            {
                if (!m_hasRotatedRight)
                {
                    m_hasRotatedRight = true;
                    m_body.Rotate (Vector3.up, m_rotationAngle, Space.World);
                }
            }
            else if (rtValue < 0.3f)
            {
                m_hasRotatedRight = false;
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandleMovementInput ()
        {
            float movX = 0f;
            float movY = 0f;

            if (Inp.GetDown (Inp.Key.Select))
                Player.Recenter ();

            if (Inp.Get (Inp.Axis.L_Vertical) > 0.1f)
                movY = 1f;
            else if (Inp.Get (Inp.Axis.L_Vertical) < -0.1f)
                movY = -1f;

            if (Inp.Get (Inp.Axis.L_Horizontal) < -0.1f)
                movX = -1f;
            else if (Inp.Get (Inp.Axis.L_Horizontal) > 0.1f)
                movX = 1f;

            var forward = m_head.forward.WithY (0f);
            var right   = m_head.right.WithY (0f);
            var movementVector = (forward * movY + right * movX).normalized;
            m_controller.SimpleMove (movementVector * m_speedMultiplier);
        }
    }
}