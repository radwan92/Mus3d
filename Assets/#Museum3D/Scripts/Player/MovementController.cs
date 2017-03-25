using UnityEngine;

namespace Mus3d
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] float m_speedMultiplier = 360f;

        CharacterController m_controller;
        Transform           m_head;

        static bool s_isEnabled;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize (CharacterController characterController, Transform head)
        {
            m_controller = characterController;
            m_head       = head;
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
            m_controller.SimpleMove (movementVector * Time.deltaTime * m_speedMultiplier);
        }
    }
}