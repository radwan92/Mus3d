using System.Collections.Generic;
using UnityEngine;

namespace Mus3d
{
    public class SpriteDigits : ScriptableObject
    {
        [SerializeField] SpriteSheet m_digitsSpriteSheet;

        static Dictionary<int, Sprite> m_spriteByDigits = new Dictionary<int, Sprite> (10);

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize ()
        {
            m_digitsSpriteSheet.Initialize ();

            for (int i = 0; i < 10; i++)
            {
                var digitSprite = m_digitsSpriteSheet.GetSprite (i, 0);
                m_spriteByDigits.Add (i, digitSprite);
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Sprite Get (int digit)
        {
            return m_spriteByDigits[digit];
        }
    }
}