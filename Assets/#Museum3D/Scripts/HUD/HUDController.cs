using System.Collections.Generic;
using UnityEngine;

namespace Mus3d
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] SpriteRenderer m_weaponSprite;
        [SerializeField] HUDNumber[]    m_hudNumbers;

        Dictionary<HUDNumber.Type, HUDNumber> m_hudNumbersByType;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Start ()
        {
            InitializeHUDNumbers ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeHUDNumbers ()
        {
            m_hudNumbersByType = new Dictionary<HUDNumber.Type, HUDNumber> (10);
            for (int i = 0; i < m_hudNumbers.Length; i++)
            {
                var hudNumber = m_hudNumbers[i];
                m_hudNumbersByType.Add (hudNumber._Type, hudNumber);
                SetupHUDNumber (hudNumber);
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void SetupHUDNumber (HUDNumber hudNumber)
        {
            switch (hudNumber._Type)
            {
                case HUDNumber.Type.Score:
                    Player.E_ScoreChanged += () => hudNumber.Set (Player.Score);
                    hudNumber.Set (Player.Score);
                    break;
                case HUDNumber.Type.Lives:
                    Player.E_LivesChanged += () => hudNumber.Set (Player.Lives);
                    hudNumber.Set (Player.Lives);
                    break;
                case HUDNumber.Type.Health:
                    Player.E_HealthChanged += () => hudNumber.Set (Player.CurrentHealth);
                    hudNumber.Set (Player.CurrentHealth);
                    break;
                case HUDNumber.Type.Ammo:
                    Ammunition.E_AmmoChanged += () => hudNumber.Set (Ammunition.GetCount ());
                    hudNumber.Set (Ammunition.GetCount ());
                    break;
                case HUDNumber.Type.Level:
                    Player.E_LevelChanged += () => hudNumber.Set (Player.Level);
                    hudNumber.Set (Player.Level);
                    break;
                default:
                    break;
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void ShowHUD ()
        {
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void HideHUD ()
        {
        }
    }
}