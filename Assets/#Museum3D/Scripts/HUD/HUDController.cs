using System;
using UnityEngine;

namespace Mus3d
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] HUDNumber[] m_hudNubmers;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeHUDNumbers ()
        {

        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void ShowHUD ()
        {
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void HideHUD ()
        {
        }

        /* ================================================================================================================================== */
        // HUD NUMBER
        /* ================================================================================================================================== */

        // TODO: Maybe instead of this struct make a child of SpriteNumber with Type?
        [Serializable] 
        public struct HUDNumber
        {
            public Type         _Type;
            public SpriteNumber SpriteNumber;

            public enum Type
            {
                Level,
                Score,
                Lives,
                Health,
                Ammo
            }
        }
    }
}