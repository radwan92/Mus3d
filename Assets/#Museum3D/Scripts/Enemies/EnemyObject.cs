using System;
using System.Diagnostics;
using UnityEngine;
using EnemyScan = Mus3d.Scanner.EnemyScan;
using DG.Tweening;

namespace Mus3d
{
    public class EnemyObject : MusObject
    {
        public static readonly float ITEM_DROP_RADIUS = 0.5f;

        [SerializeField] EnemyAI    m_ai;
        [SerializeField] Enemy      m_enemyTemplate;

        public Enemy            Enemy           { get; private set; }
        public Enemy            EnemyTemplate   { get { return m_enemyTemplate; } }
        public EnemyScan        EnemyScan       { get; private set; }
        public EnemyAnimator    Anim            { get; private set; }
        public EnemyAI          Ai              { get { return m_ai; }}

        public Vector3  Position        { get { return m_bodyTransform.position; } }
        public Vector3  Forward         { get { return m_bodyTransform.forward; } }
        public Vector3  Right           { get { return m_bodyTransform.right; } }
        public Vector3  Left            { get { return -Right; } }
        public Vector3  HitboxBoundary  { get { return m_bodyTransform.position + m_transform.right * Enemy.HitboxRadius; } }

        public float AttackRangeSquare          { get; private set; }
        public float AttackRangeOptimalSquare   { get; private set; }
        public float SightRangeSquare           { get; private set; }
        public float HearingRangeSquare         { get; private set; }

        public bool IsDead { get; private set; }

        Transform m_bodyTransform;


        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void HandleGotHit (int damage)
        {
            Enemy.Health -= damage;

            if (Enemy.Health <= 0)
                Die ();

            if (IsDead)
            {
                m_ai.SetState (AiState.Type.Dying);
            }
            else
            {
                if (!m_ai.IsAlert)
                    Scanner.ScanEnemyAlarmHearing (this);

                m_ai.SetState (AiState.Type.Hit);
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void HandleAlarmHeard ()
        {
            if (m_ai.IsAlert)
                return;

            m_ai.SetState (AiState.Type.Chasing);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void HandleShotHeard ()
        {
            if (m_ai.IsAlert)
                return;

            Scanner.ScanEnemyAlarmHearing (this);
            Sounds.PlayRandom (Enemy.ShoutSounds);
            m_ai.SetState (AiState.Type.Chasing);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void HandleDeath ()
        {
            Dropitems ();

            // For body billboarding purposes
            var musObject = gameObject.AddComponent<MusObject> ();
            musObject.Initialize (m_spriteRenderer, m_transform);

            Destroy (this);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Dropitems ()
        {
            RaycastHit groundRayHitInfo;
            bool raycastAgainstTheGround = Physics.Raycast (Position, Vector3.down, out groundRayHitInfo, 5f, ~LayerMask.NameToLayer ("Default"), QueryTriggerInteraction.Ignore);

            float dropYPosition = 0f;

            if (raycastAgainstTheGround)
                dropYPosition = groundRayHitInfo.point.y;
            else
                dropYPosition = Position.y - 1f;

            var drops = Enemy.Drops;
            for (int i = 0; i < drops.Length; i++)
            {
                var dropChance = UnityEngine.Random.Range (0f, 100f);

                if (dropChance > drops[i].Chance)
                    continue;

                var dropAngle    = UnityEngine.Random.Range (0f, Mathf.PI * 2f);
                var dropPosition = new Vector3 (Mathf.Cos (dropAngle), dropYPosition, Mathf.Sin (dropAngle)) * ITEM_DROP_RADIUS + Position;

                var dropObject = Instantiate (drops[i].ItemObjectPrefab, Position.WithY (dropYPosition), Quaternion.identity);
                dropObject.transform.DOJump (dropPosition, 0.3f, 1, 0.5f).SetEase (Ease.Flash);
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Die ()
        {
            if (IsDead)
                return;
            IsDead = true;

            Scanner.RemoveEnemy (EnemyScan);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected override void Awake ()
        {
            base.Awake ();

            if (m_ai == null)
                m_ai = GetComponent<EnemyAI> ();

            EditorAwake ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        [Conditional("UNITY_EDITOR")]
        void EditorAwake ()
        {
            if (Enemy == null)
            {
                Enemies.E_Loaded += Initialize;
                return;
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Initialize ()
        {
            Enemy                    = Enemies.GetInstance (m_enemyTemplate.Type_);
            m_bodyTransform          = m_ai.transform;
            Anim                     = new EnemyAnimator (this, m_spriteRenderer);
            EnemyScan                = new EnemyScan () { Enemy = this };

            AttackRangeSquare        = Enemy.AttackRange * Enemy.AttackRange;
            AttackRangeOptimalSquare = Enemy.AttackRangeOptimal * Enemy.AttackRangeOptimal;
            SightRangeSquare         = Enemy.SightRange * Enemy.SightRange;
            HearingRangeSquare       = Enemy.HearingRange * Enemy.HearingRange;

            m_ai.Initialize (this, Anim);
            Scanner.AddEnemy (EnemyScan);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void OnDestroy ()
        {
            if (Application.isPlaying)
            {
                Destroy (m_ai);
            }
        }
    }
}