using System;
using UnityEngine;
using UnityEngine.AI;
using Mus3d.Anim;
using Animation = Mus3d.Anim.Animation;

namespace Mus3d
{
    public abstract class AiState
    {
        protected Enemy         m_enemy;
        protected EnemyObject   m_enemyObject;
        protected EnemyAnimator m_anim;
        protected NavMeshAgent  m_navAgent;
        protected EnemyAI       m_ai;

        public abstract Type Type_ { get; }

        protected bool m_isActive;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public AiState (EnemyObject enemyObject)
        {
            m_enemyObject = enemyObject;
            m_enemy       = enemyObject.Enemy;
            m_anim        = enemyObject.Anim;
            m_ai          = enemyObject.Ai;
            m_navAgent    = m_ai.NavAgent;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public virtual void Enter () 
        {
            m_isActive = true;
            //Debug.Log ("Entered state: " + Type_);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public virtual void Update () { }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public virtual void Exit ()
        {
            m_isActive = false;
            //Debug.Log ("Exited state: " + Type_);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected virtual bool TryNoticePlayerAndHandleIt ()
        {
            if (m_ai.HasNoticedPlayer ())
            {
                Sounds.PlayRandom (m_enemy.ShoutSounds);

                if (m_ai.CanAttackPlayer ())
                    m_ai.SetState (Type.Attacking);
                else
                    m_ai.SetState (Type.Chasing);

                return true;
            }

            return false;
        }


        public enum Type
        {
            Idle,
            Hit,
            Patroling,
            Chasing,
            Attacking,
            Dying
        }
    }

    /* ================================================================================================================================== */
    // IDLE
    /* ================================================================================================================================== */
    public class State_Idle : AiState
    {
        public State_Idle (EnemyObject enemyObject) : base (enemyObject) { }

        public override Type Type_ { get { return Type.Idle; } }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Enter ()
        {
            base.Enter ();

            m_navAgent.Stop ();
            m_anim.SetState (EnemyAnimator.State.Idle, loop: true);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Update ()
        {
            if (TryNoticePlayerAndHandleIt ())
                return;

            m_anim.SetDirection (m_enemyObject.Forward, true);
        }
    }

    /* ================================================================================================================================== */
    // IDLE - HOUND
    /* ================================================================================================================================== */
    public class State_Idle_Hound : AiState
    {
        public override Type Type_ { get { return Type.Idle; } }

        static readonly float ROAMING_RADIUS = 3f;

        Vector3 m_basePosition;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public State_Idle_Hound (EnemyObject enemyObject) : base (enemyObject)
        {
            m_basePosition = enemyObject.Position;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Enter ()
        {
            base.Enter ();

            m_anim.SetState (EnemyAnimator.State.Walk, loop: true);
            SetNextDestination ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Update ()
        {
            if (TryNoticePlayerAndHandleIt ())
                return;

            if (m_ai.HasFinishedPath ())
                SetNextDestination ();
            else
                m_anim.SetDirection (m_navAgent.desiredVelocity, true);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void SetNextDestination ()
        {
            var angle        = UnityEngine.Random.Range (0f, 360f);
            var offsetVector = Vector3.forward.RotateOnXZPlane (angle) * ROAMING_RADIUS;
            var destination  = m_basePosition + offsetVector;

            NavMeshHit navMeshHit;
            if (m_navAgent.Raycast (destination, out navMeshHit))
                destination = navMeshHit.position;

            m_navAgent.SetDestination (destination);

            m_anim.SetDirection (destination - m_enemyObject.Position, true);
        }
    }

    /* ================================================================================================================================== */
    // CHASING - HOUND
    /* ================================================================================================================================== */
    public class State_Chasing_Hound : State_Chasing
    {
        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public State_Chasing_Hound (EnemyObject enemyObject) : base (enemyObject, cyclesToPathUpdate: 2, flankAngle: 45f) { }
    }

    /* ================================================================================================================================== */
    // CHASING
    /* ================================================================================================================================== */
    public class State_Chasing : AiState
    {
        public override Type Type_ { get { return Type.Chasing; } }

        protected readonly int      CYCLES_TO_PATH_UPDATE;
        protected readonly float    FLANK_ANGLE;

        int m_cyclesCount;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public State_Chasing (EnemyObject enemyObject, int cyclesToPathUpdate, float flankAngle) : base (enemyObject)
        {
            CYCLES_TO_PATH_UPDATE = cyclesToPathUpdate;
            FLANK_ANGLE           = flankAngle;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public State_Chasing (EnemyObject enemyObject) : this (enemyObject, cyclesToPathUpdate: 5, flankAngle: 15f) { }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Enter ()
        {
            base.Enter ();

            m_navAgent.SetDestination (GetRandomDestination ());
            m_navAgent.Resume ();

            var initialChaseVector = m_ai.EnemyToPlayerVector ();

            m_anim.SetState (EnemyAnimator.State.Walk, initialChaseVector, animateNow: true);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Update ()
        {
            if (m_ai.CanAttackPlayer ())
            {
                m_ai.SetState (Type.Attacking);
            }
            else
            {
                m_cyclesCount++;

                if (m_cyclesCount % CYCLES_TO_PATH_UPDATE == 0)
                {
                    var destination = GetRandomDestination ();
                    m_navAgent.SetDestination (destination);
                    m_anim.SetDirection (destination - m_enemyObject.Position, true);
                }
                else if (m_navAgent.desiredVelocity.sqrMagnitude > 0.1f)
                {
                    m_anim.SetDirection (m_navAgent.desiredVelocity, true);
                }
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        Vector3 GetRandomDestination ()
        {
            Side side = (Side)UnityEngine.Random.Range (0, 3);
            return GetDestination (side);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        Vector3 GetDestination (Side side)
        {
            switch (side)
            {
                case Side.Front:
                {
                    return Player.Position;
                }

                case Side.LeftFlank:
                case Side.RightFlank:
                {
                    var enemyToPlayerVector = m_ai.EnemyToPlayerVector ();
                    var flankOffset         = GetFlankDestinationOffset (side, enemyToPlayerVector);
                    var destination         = m_enemyObject.Position + enemyToPlayerVector + flankOffset;

                    NavMeshHit navMeshHit;
                    if (m_navAgent.Raycast (destination, out navMeshHit))
                    {
                        return Player.Position;
                    }

                    return destination;
                }
            }

            return Vector3.zero;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        Vector3 GetFlankDestinationOffset (Side side, Vector3 enemyToPlayerVector)
        {
            if (side == Side.LeftFlank)
                return (-enemyToPlayerVector).WithY (0f).RotateOnXZPlane (FLANK_ANGLE).normalized * m_enemy.AttackRange;
            else if (side == Side.RightFlank)
                return (-enemyToPlayerVector).WithY (0f).RotateOnXZPlane (-FLANK_ANGLE).normalized * m_enemy.AttackRange;

            return Vector3.zero;
        }

        public enum Side
        {
            Front,
            LeftFlank,
            RightFlank
        }
    }

    /* ================================================================================================================================== */
    // ATTACKING - HOUND
    /* ================================================================================================================================== */
    public class State_Attacking_Hound : State_Attacking
    {
        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public State_Attacking_Hound (EnemyObject enemyObject) : base (enemyObject, 45f, 70f) { }
    }

    /* ================================================================================================================================== */
    // ATTACKING
    /* ================================================================================================================================== */
    public class State_Attacking : AiState
    {
        public override Type Type_ { get { return Type.Attacking; } }

        protected readonly float MAX_STRAFE_ANGLE;
        protected readonly float MIN_STRAFE_ANGLE;

        bool m_isStrafing;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public State_Attacking (EnemyObject enemyObject, float minStrafeAngle, float maxStrafeAngle) : base (enemyObject)
        {
            m_anim.E_EndOfStateAnim += HandleEndOfAttackAnimation;
            m_anim.E_EventFired     += HandleAnimAttackEvent;

            MAX_STRAFE_ANGLE = maxStrafeAngle;
            MIN_STRAFE_ANGLE = minStrafeAngle;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public State_Attacking (EnemyObject enemyObject) : this (enemyObject, 20f, 40f) { }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Update ()
        {
            if (m_isStrafing && m_ai.HasFinishedPath ())
            {
                StopStrafing ();

                if (m_ai.CanAttackPlayer ())
                    StartAttacking ();
                else
                    m_ai.SetState (Type.Chasing);
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandleAnimAttackEvent (Animation.Event eventType)
        {
            if (eventType == Animation.Event.AttackStart)
                Sounds.PlayRandom (m_enemy.AttackSounds);
            else if (eventType == Animation.Event.Attack)
                Scanner.ScanEnemyToPlayerShot (m_enemyObject);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandleEndOfAttackAnimation (EnemyAnimator.State state)
        {
            if (state != EnemyAnimator.State.Attack)
                return;

            if (!m_isActive)
            {
                Debug.LogError ("Reached end of attack animation state but we are not in the Attacking state");
                return;
            }

            // TODO: Wait or strafe before another shot
            if (m_ai.CanAttackPlayer ())
                StartStrafing ();
            else
                m_ai.SetState (Type.Chasing);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Enter ()
        {
            base.Enter ();
            m_navAgent.Stop ();

            StartAttacking ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Exit ()
        {
            base.Exit ();
            StopStrafing ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void StartStrafing ()
        {
            var playerToEnemyVector = m_ai.PlayerToEnemyVector ();
            var strafeAngle         = UnityEngine.Random.Range (-MAX_STRAFE_ANGLE, MAX_STRAFE_ANGLE);

            strafeAngle = (strafeAngle < 0) ? Mathf.Min (strafeAngle, -MIN_STRAFE_ANGLE) : Mathf.Max (strafeAngle, MIN_STRAFE_ANGLE);

            var offsetVector = playerToEnemyVector.WithY (0f).RotateOnXZPlane (strafeAngle);

            if (offsetVector.sqrMagnitude > m_enemyObject.AttackRangeOptimalSquare)
                offsetVector *= 0.8f;

            var destination = Player.Position + offsetVector;

            NavMeshHit navMeshHit;
            if (m_navAgent.Raycast (destination, out navMeshHit))
                destination = navMeshHit.position;

            m_navAgent.SetDestination (destination);
            m_navAgent.Resume ();

            m_anim.SetState (EnemyAnimator.State.Walk, destination - m_enemyObject.Position, animateNow: true);

            m_isStrafing = true;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void StopStrafing ()
        {
            m_isStrafing = false;
            m_navAgent.Stop ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void StartAttacking ()
        {
            var aimVector = m_ai.EnemyToPlayerVector ();
            m_anim.SetState (EnemyAnimator.State.Attack, aimVector, false, false);
        }
    }

    /* ================================================================================================================================== */
    // PATROLING
    /* ================================================================================================================================== */
    public class State_Patrolling : AiState, IDisposable
    {
        AiPatrol m_aiPatrol;

        public override Type Type_ { get { return Type.Patroling; } }

        static readonly int MAX_WAIT_CYCLES = 20;

        int     m_cyclesToWait;
        int     m_waitCycleCount;
        bool    m_isWaiting;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public State_Patrolling (EnemyObject enemyObject) : base (enemyObject)
        {
            m_aiPatrol = m_ai.GetComponent<AiPatrol> ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Enter ()
        {
            base.Enter ();

            var nextDestination   = m_aiPatrol.GetNextPatrolDestination ();
            var initialMoveVector = nextDestination - m_enemyObject.Position;

            m_anim.SetState (EnemyAnimator.State.Walk, initialMoveVector);

            m_navAgent.SetDestination (nextDestination);
            m_navAgent.Resume ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Update ()
        {
            if (TryNoticePlayerAndHandleIt ())
                return;

            if (m_isWaiting)
            {
                HandleWaiting ();
                return;
            }

            if (m_ai.HasFinishedPath ())
            {
                HandlePathFinished ();
                return;
            }

            if (m_navAgent.desiredVelocity.sqrMagnitude > 0.1f)
                m_anim.SetDirection (m_navAgent.desiredVelocity, false);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        private void HandlePathFinished ()
        {
            m_isWaiting = true;
            m_navAgent.Stop ();

            m_cyclesToWait = UnityEngine.Random.Range (0, MAX_WAIT_CYCLES);

            m_anim.SetState (EnemyAnimator.State.Idle, m_enemyObject.Forward, false);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        private void HandleWaiting ()
        {
            m_waitCycleCount++;

            if (m_waitCycleCount >= m_cyclesToWait)
            {
                m_isWaiting = false;
                m_waitCycleCount = 0;

                m_navAgent.SetDestination (m_aiPatrol.GetNextPatrolDestination ());
                m_navAgent.Resume ();

                var initialMovementVector = m_navAgent.desiredVelocity - m_enemyObject.Position;
                m_anim.SetState (EnemyAnimator.State.Walk, initialMovementVector);
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Dispose ()
        {
            if (m_aiPatrol != null)
                GameObject.Destroy (m_aiPatrol);
        }
    }

    /* ================================================================================================================================== */
    // GOT HIT
    /* ================================================================================================================================== */
    public class State_Hit : AiState
    {
        public override Type Type_ { get { return Type.Hit; } }

        static readonly int CYCLES_TO_FINISH_STATE = 2
            ;
        bool    m_finishedAnimation;
        int     m_cyclesCount = 0;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public State_Hit (EnemyObject enemyObject) : base (enemyObject)
        {
            m_anim.E_EndOfStateAnim += HandleEndOfHitAnimation;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandleEndOfHitAnimation (EnemyAnimator.State state)
        {
            if (state != EnemyAnimator.State.Hit || !m_isActive)
                return;

            m_finishedAnimation = true;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Enter ()
        {
            base.Enter ();

            Sounds.PlayRandom (m_enemy.GotHitSounds);

            m_anim.SetState (EnemyAnimator.State.Hit, m_ai.EnemyToPlayerVector (), false);

            m_navAgent.Stop ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Update ()
        {
            if (!m_finishedAnimation)
                return;

            m_cyclesCount++;

            if (m_cyclesCount >= CYCLES_TO_FINISH_STATE)
            {
                if (m_ai.CanAttackPlayer ())
                    m_ai.SetState (Type.Attacking);
                else
                    m_ai.SetState (Type.Chasing);
            }
        }


        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Exit ()
        {
            base.Exit ();

            m_finishedAnimation = false;
            m_cyclesCount       = 0;
        }

    }

    /* ================================================================================================================================== */
    // DYING
    /* ================================================================================================================================== */
    public class State_Dying : AiState
    {
        public override Type Type_ { get { return Type.Dying; } }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public State_Dying (EnemyObject enemyObject) : base (enemyObject)
        {
            m_anim.E_EndOfStateAnim += HandleEndOfDeathAnim;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandleEndOfDeathAnim (EnemyAnimator.State animState)
        {
            if (animState != EnemyAnimator.State.Die)
                return;

            m_anim.SetState (EnemyAnimator.State.Body);
            m_enemyObject.HandleDeath ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public override void Enter ()
        {
            base.Enter ();

            Sounds.PlayRandom (m_enemy.DeathSounds);

            m_anim.SetState (EnemyAnimator.State.Die, loop: false);

            m_navAgent.Stop ();
        }
    }
}