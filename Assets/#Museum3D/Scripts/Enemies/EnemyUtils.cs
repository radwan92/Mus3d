using UnityEngine;

namespace Mus3d
{
    public static class EnemyUtils
    {
        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Direction GetDirectionForVector (Vector3 observerToEnemyVector, Vector3 viewDirection)
        {
            float angle = Vector3.Angle (observerToEnemyVector, viewDirection);

            if (Vector3.Cross (observerToEnemyVector, viewDirection).y > 0)
                angle = (360f - angle);

            angle = (angle + Consts.HALF_DIRECTION_ANGLE) % 360;
            int directionIndex = ((int)(angle / Consts.DIRECTION_ANGLE) + 4) % 8;

            return (Direction)directionIndex;
        }
    }
}