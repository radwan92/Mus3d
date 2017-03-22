using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mus3d
{
    public static class Scanner
    {
        public static readonly float VIABLE_RANGE_SQUARE        = 20f * 20f;
        public static readonly float ALARM_HEARING_RANGE_SQUARE = 5f * 5f;

        static YieldInstruction     m_rangeCheckInterval   = new WaitForSeconds (1f);
        static YieldInstruction     m_sightCheckInterval   = new WaitForSeconds (0.2f);
        static List<EnemyScan>      m_enemyScans           = new List<EnemyScan> (100);

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Initialize ()
        {
            Coroutiner.Start (ScanRange_Corotuine ());
            Coroutiner.Start (ScanSight_Coroutine ());
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void AddEnemy (EnemyScan enemyScan)
        {
            m_enemyScans.Add (enemyScan);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void RemoveEnemy (EnemyScan enemyScan)
        {
            m_enemyScans.Remove (enemyScan);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void ScanPlayerToEnemiesShot (float weaponRangeSquare)
        {
            float       smallestRangeSquare     = float.MaxValue;
            EnemyScan   smallestRangeScan       = null;

            for (int i = 0; i < m_enemyScans.Count; i++)
            {
                var enemyScan = m_enemyScans[i];

                if (!enemyScan.IsInRange || !enemyScan.IsInSight || enemyScan.Enemy.IsDead)
                    continue;

                var enemyToPlayerVect        = EnemyToPlayerVect (enemyScan.Enemy);
                var enemyToPlayerRangeSquare = enemyToPlayerVect.sqrMagnitude;

                if (enemyToPlayerRangeSquare > weaponRangeSquare)
                    continue;

                var enemyHitboxVect    = enemyScan.Enemy.HitboxBoundary;
                var hitboxToPlayerVect = enemyHitboxVect - Player.Position;

                var maxAngle = Vector3.Angle (enemyToPlayerVect, hitboxToPlayerVect);
                var aimAngle = Vector3.Angle (Player.Forward.WithY (0f), enemyToPlayerVect);

                if (aimAngle <= maxAngle
                    && enemyToPlayerRangeSquare < smallestRangeSquare)
                {
                    smallestRangeSquare = enemyToPlayerRangeSquare;
                    smallestRangeScan   = enemyScan;
                }
            }

            bool playerYieldsMeleeWeapon = Player.HeldWeaponClass == Weapon.Class.Melee;

            if (smallestRangeScan != null)
            {
                // Backstab!
                if (playerYieldsMeleeWeapon && !smallestRangeScan.Enemy.IsAlert)
                {
                    var enemyToPlayerVect = EnemyToPlayerVect (smallestRangeScan.Enemy);
                    var isBackstabbing    = Vector3.Dot (enemyToPlayerVect, smallestRangeScan.Enemy.Forward) > 0;

                    if (isBackstabbing)
                    {
                        Debug.Log ("Backstab!");
                        smallestRangeScan.Enemy.HandleGotHit (9999999);
                        return;
                    }
                }

                var damageRand = Random.Range (0, 256);
                int damage = 0;

                int damageRandByTwelve;

                if (smallestRangeSquare < 4)
                    damage = damageRand / 4;
                else if (smallestRangeSquare < 16)
                    damage = damageRand / 6;
                else if (((damageRandByTwelve = (damageRand / 12)) > 0) && (damageRandByTwelve * damageRandByTwelve) > smallestRangeSquare)
                    damage = damageRand / 6;

                if (damage > 0)
                    smallestRangeScan.Enemy.HandleGotHit (damage);
            }

            if (!playerYieldsMeleeWeapon)
                ScanEnemiesShotHearing ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void ScanEnemyToPlayerShot (EnemyObject enemyObject)
        {
            Vector3 playerToEnemyVector = - EnemyToPlayerVect (enemyObject);
            int     distance            = Mathf.CeilToInt (playerToEnemyVector.magnitude);
            bool    doesPlayerSeeEnemy  = Vector3.Angle (Player.Forward, playerToEnemyVector) < 45; // ~FOV 90
            bool    isPlayerMoving      = Player.Velocity.sqrMagnitude > 0.5f;  // TODO: Handle VR mode teleport-type movement

            var hitChance          = isPlayerMoving ? 160 : 256;
            var distanceMultiplier = doesPlayerSeeEnemy ? 16 : 8;
            hitChance             -= distance * distanceMultiplier;

            var hitRand = Random.Range (0, 256); // [0, 255]

            // Miss
            if (hitRand > hitChance)
                return;

            var damageRand = Random.Range (0, 256);
            int damage = 0;

            if (distance < 2)
                damage = damageRand / 4;
            else if (distance < 4)
                damage = damageRand / 8;
            else if (distance < 8)
                damage = damageRand / 16;

            if (damage <= 0)
                return;

            Player.Hit (damage);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void ScanEnemyAlarmHearing (EnemyObject enemyObject)
        {
            for (int i = 0; i < m_enemyScans.Count; i++)
            {
                var otherEnemyObject           = m_enemyScans[i].Enemy;
                var distanceSquareToOtherEnemy = (otherEnemyObject.Position - enemyObject.Position).sqrMagnitude;

                if (distanceSquareToOtherEnemy <= ALARM_HEARING_RANGE_SQUARE)
                    otherEnemyObject.HandleAlarmHeard ();
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        static Vector3 EnemyToPlayerVect (EnemyObject enemy)
        {
            return enemy.Position - Player.Position;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        static IEnumerator ScanRange_Corotuine ()
        {
            while (true)
            {
                for (int i = 0; i < m_enemyScans.Count; i++)
                {
                    var enemyScan         = m_enemyScans[i];
                    var playerToEnemyVect = EnemyToPlayerVect (enemyScan.Enemy);

                    enemyScan.IsInRange            = playerToEnemyVect.sqrMagnitude <= VIABLE_RANGE_SQUARE;
                    enemyScan.IsInShotHearingRange = playerToEnemyVect.sqrMagnitude <= enemyScan.Enemy.HearingRangeSquare;
                }

                yield return m_rangeCheckInterval;
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        static IEnumerator ScanSight_Coroutine ()
        {
            while (true)
            {
                for (int i = 0; i < m_enemyScans.Count; i++)
                {
                    var enemyScan = m_enemyScans[i];

                    if (!enemyScan.IsInRange)
                        continue;

                    var enemyToPlayerVect = EnemyToPlayerVect (enemyScan.Enemy);
                    enemyScan.IsInSight = Vector3.Dot (Player.Forward, enemyToPlayerVect) > 0f;
                }

                yield return m_sightCheckInterval;
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        static void ScanEnemiesShotHearing ()
        {
            for (int i = 0; i < m_enemyScans.Count; i++)
            {
                var enemyScan = m_enemyScans[i];

                if (enemyScan.IsInShotHearingRange)
                    enemyScan.Enemy.HandleShotHeard ();
            }
        }

        /* ================================================================================================================================== */
        // ENEMY SCAN
        /* ================================================================================================================================== */
        public class EnemyScan
        {
            public EnemyObject  Enemy;
            public bool         IsInRange;
            public bool         IsInShotHearingRange;
            public bool         IsInSight;
        }
    }
}