using UnityEngine;

namespace Mus3d
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] float m_speedMultiplier = 360f;

        CharacterController m_controller;
        Transform           m_head;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize (CharacterController characterController, Transform head)
        {
            m_controller = characterController;
            m_head       = head;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Update ()
        {
            float movX = 0f;
            float movY = 0f;

            if (Inp.Get (Inp.Key.Up))
                movY = 1f;
            else if (Inp.Get (Inp.Key.Down))
                movY = -1f;

            if (Inp.Get (Inp.Key.Left))
                movX = -1f;
            else if (Inp.Get (Inp.Key.Right))
                movX = 1f;

            var movementVector = (m_head.forward * movY + m_head.right * movX).normalized;
            m_controller.SimpleMove (movementVector * Time.deltaTime * m_speedMultiplier);
        }
    }
}