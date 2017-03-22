using UnityEngine;

namespace Mus3d
{
    public class SpriteNumber : MonoBehaviour
    {
        static readonly string DIGIT_GAMEOBJECT_PREFIX = "L_";

        SpriteRenderer[]    m_digitRenderers;
        Transform           m_tform;
        int                 m_digitCount;

        int[] m_digitStorage;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Set (int number)
        {
            if (number / (Mathf.Pow (10, m_digitCount)) >= 10)
            {
                Debug.LogErrorFormat (this, "Trying to set a number: {0} exceeding allowed number of digits: {1}", number, m_digitCount);
                return;
            }

            int digitIndex = 0;
            while (number > 0)
            {
                int digit = number % 10;
                number /= 10;
                m_digitStorage[digitIndex] = digit;
                ++digitIndex;
            }

            for (int i = 0; i < m_digitCount; i++)
            {
                if (i < digitIndex)
                {
                    int digit = m_digitStorage[i];
                    m_digitRenderers[i].enabled = true;
                    m_digitRenderers[i].sprite = SpriteDigits.Get (digit);
                }
                else
                {
                    m_digitRenderers[i].enabled = false;
                }
            }

            if (digitIndex == 0)
            {
                m_digitRenderers[0].enabled = true;
                m_digitRenderers[0].sprite = SpriteDigits.Get (0);
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Awake ()
        {
            m_tform          = transform;
            m_digitCount     = m_tform.childCount;
            m_digitStorage   = new int [m_digitCount];

            GetDigitObjects ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void GetDigitObjects ()
        {
            m_digitRenderers = new SpriteRenderer[m_digitCount];

            for (int i = 0; i < m_tform.childCount; i++)
            {
                var child = m_tform.GetChild (i);

                if (!child.name.StartsWith (DIGIT_GAMEOBJECT_PREFIX))
                {
                    Debug.LogErrorFormat (this, "Invalid {0} digit child", typeof (SpriteNumber));
                    continue;
                }

                var digitIndexString = child.name.Remove (0, DIGIT_GAMEOBJECT_PREFIX.Length);

                int digitIndex;
                if (!int.TryParse (digitIndexString, out digitIndex))
                    continue;

                m_digitRenderers[digitIndex - 1] = child.GetComponent<SpriteRenderer> ();
            }
        }
    }
}