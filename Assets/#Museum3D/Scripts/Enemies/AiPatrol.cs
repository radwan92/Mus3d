using UnityEngine;

namespace Mus3d
{
    public class AiPatrol : MonoBehaviour
    {
        public enum Type
        {
            RoundRobin,
            Random
        }

        public enum State
        {
            Waiting,
            Patrolling
        }

        [SerializeField] Transform[]    m_route;
        [SerializeField] Type           m_type;

        int m_waypointIndex;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public Vector3 GetCurrentPatrolDestination ()
        {
            return m_route [m_waypointIndex].position;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public Vector3 GetNextPatrolDestination ()
        {
            if (m_type == Type.Random && m_route.Length > 2)
            {
                m_waypointIndex = Random.Range (0, m_route.Length);
            }
            else
            {
                m_waypointIndex = (m_waypointIndex + 1) % m_route.Length;
            }

            return m_route[m_waypointIndex].position;
        }
    }
}