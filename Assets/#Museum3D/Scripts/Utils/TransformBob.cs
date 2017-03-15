using System;
using UnityEngine;

namespace Mus3d
{
    public class TransformBob : MonoBehaviour
    {
        [SerializeField] Transform m_bobedTransform;
        [Space]
        [SerializeField] float m_horizontalBobRange        = 0.33f;
        [SerializeField] float m_verticalBobRange          = 0.33f;
        [SerializeField] float m_verticaltoHorizontalRatio = 1f;
        [SerializeField] float m_bobBaseInterval           = 0.5f;
        [Space]
        [SerializeField] AnimationCurve m_bobCurve = new AnimationCurve(new Keyframe(0f, 0f), 
            new Keyframe(0.5f, 1f),
            new Keyframe(1f, 0f), 
            new Keyframe(1.5f, -1f),
            new Keyframe(2f, 0f));

        private float   m_cyclePositionX;
        private float   m_cyclePositionZ;
        private Vector3 m_originalPosition;
        private float   m_time;

        Func<float> e_getBobSpeed;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Start ()
        {
            m_originalPosition = m_bobedTransform.localPosition;
            m_time             = m_bobCurve[m_bobCurve.length - 1].time; // Get the length of the curve in time
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void SetSpeedSource (Func<float> e_getSpeed)
        {
            e_getBobSpeed = e_getSpeed;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Update ()
        {
            DoBob ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void DoBob ()
        {
            if (e_getBobSpeed == null)
                return;

            float speed = e_getBobSpeed ();

            float xPos = m_originalPosition.x + (m_bobCurve.Evaluate(m_cyclePositionX) * m_horizontalBobRange);
            float zPos = m_originalPosition.z + (m_bobCurve.Evaluate(m_cyclePositionZ) * m_verticalBobRange);

            m_cyclePositionX += (speed * Time.deltaTime) / m_bobBaseInterval;
            m_cyclePositionZ += ((speed * Time.deltaTime) / m_bobBaseInterval) * m_verticaltoHorizontalRatio;

            if (m_cyclePositionX > m_time)
            {
                m_cyclePositionX = m_cyclePositionX - m_time;
            }
            if (m_cyclePositionZ > m_time)
            {
                m_cyclePositionZ = m_cyclePositionZ - m_time;
            }

            m_bobedTransform.localPosition = new Vector3(xPos, 0f, zPos);
        }
    }
}