using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Mus3d
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] protected LayerMask      m_aimLayer;
        [SerializeField] protected NavMeshAgent   m_navAgent;
        [Space]
        [SerializeField] protected AiState.Type   m_startingState;
        [Space]
        [SerializeField] protected Collider       m_collider;

        public NavMeshAgent NavAgent { get { return m_navAgent; } }

        protected Enemy       m_enemy;
        protected EnemyObject m_enemyObject;
        protected AiState     m_currentState;

        protected RaycastHit[] m_raycastResults = new RaycastHit [20];    // TODO: This might be too small. Check this

        protected Dictionary<AiState.Type, AiState> m_statesByType;

        protected YieldInstruction m_baseUpdateInterval;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected virtual void OnDrawGizmos ()
        {
            var color = Gizmos.color;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine (transform.position, m_navAgent.destination);
            Gizmos.color = color;

            if (m_currentState != null && m_currentState.Type_ == AiState.Type.Patroling)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine (m_enemyObject.Position, m_navAgent.destination);
                Gizmos.color = color;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawLine (transform.position, transform.position + m_navAgent.desiredVelocity);
            Gizmos.color = color;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected virtual void Awake ()
        {
            if (m_navAgent == null)
                m_navAgent = GetComponent<NavMeshAgent> ();
            if (m_collider == null)
                m_collider = GetComponent<Collider> ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public bool IsAlert
        {
            get
            {
                return m_currentState.Type_ == AiState.Type.Chasing 
                    || m_currentState.Type_ == AiState.Type.Attacking 
                    || m_currentState.Type_ == AiState.Type.Hit;
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public virtual void Initialize (EnemyObject enemyObject, EnemyAnimator anim)
        {
            m_enemyObject    = enemyObject;
            m_enemy          = m_enemyObject.Enemy;
            m_navAgent.speed = m_enemy.Speed;
            m_statesByType   = new Dictionary<AiState.Type, AiState> (8);

            InitializeStates ();
            InitializeBaseInterval ();

            SetState (m_startingState);
            StartCoroutine (AiCoroutine ());
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected virtual void InitializeStates ()
        {
            m_statesByType.Add (AiState.Type.Idle,      new State_Idle (m_enemyObject));
            m_statesByType.Add (AiState.Type.Patroling, new State_Patrolling (m_enemyObject));
            m_statesByType.Add (AiState.Type.Chasing,   new State_Chasing (m_enemyObject));
            m_statesByType.Add (AiState.Type.Attacking, new State_Attacking (m_enemyObject));
            m_statesByType.Add (AiState.Type.Hit,       new State_Hit (m_enemyObject));
            m_statesByType.Add (AiState.Type.Dying,     new State_Dying (m_enemyObject));
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected virtual void InitializeBaseInterval ()
        {
            m_baseUpdateInterval = new WaitForSeconds (0.2f);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected virtual IEnumerator AiCoroutine ()
        {
            yield return m_baseUpdateInterval;

            while (!m_enemyObject.IsDead)
            {
                m_currentState.Update ();
                yield return m_baseUpdateInterval;
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void SetState (AiState.Type stateType)
        {
            if (m_currentState != null)
                m_currentState.Exit ();

            m_currentState = m_statesByType[stateType];
            m_currentState.Enter ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public bool HasFinishedPath ()
        {
            return !m_navAgent.pathPending && m_navAgent.remainingDistance <= m_navAgent.stoppingDistance && (!m_navAgent.hasPath || m_navAgent.velocity.sqrMagnitude == 0f);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public bool HasNoticedPlayer ()
        {
            Vector3 enemyToPlayerVector = EnemyToPlayerVector ();

            bool isPlayerInSightCone = Vector3.Dot (m_enemyObject.Forward.normalized, enemyToPlayerVector.normalized) > 0.4f;

            if (!isPlayerInSightCone)
                return false;

            bool isPlayerInSightRange = enemyToPlayerVector.sqrMagnitude < m_enemyObject.SightRangeSquare;

            return isPlayerInSightRange;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public bool CanAttackPlayer ()
        {
            Vector3 aimVector             = EnemyToPlayerVector ();
            bool    isPlayerInAttackRange = aimVector.sqrMagnitude < m_enemyObject.AttackRangeSquare;

            if (!isPlayerInAttackRange)
                return false;

            int hitCount = Physics.RaycastNonAlloc (m_enemyObject.Position, aimVector, 
                    m_raycastResults, m_enemy.AttackRange, m_aimLayer.value, QueryTriggerInteraction.Ignore);

            return hitCount == 0;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public Vector3 EnemyToPlayerVector ()
        {
            return Player.Position - m_enemyObject.Position;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public Vector3 PlayerToEnemyVector ()
        {
            return m_enemyObject.Position - Player.Position;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void OnDestroy ()
        {
            if (m_navAgent != null)
                Destroy (m_navAgent);
            if (m_collider != null)
                Destroy (m_collider);

            if (m_statesByType != null && m_statesByType.ContainsKey (AiState.Type.Patroling))
                ((IDisposable)m_statesByType[AiState.Type.Patroling]).Dispose ();
        }
    }
}