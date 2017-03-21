using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mus3d
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] SpriteSheet    m_faceSpriteSheet;
        [SerializeField] SpriteRenderer m_faceSpriteRenderer;
        [SerializeField] SpriteSheet    m_weaponSpriteSheet;
        [SerializeField] SpriteRenderer m_weaponRenderer;
        [SerializeField] HUDNumber[]    m_hudNumbers;

        Dictionary<HUDNumber.Type, HUDNumber> m_hudNumbersByType;

        Dictionary<Weapon.Type, int> m_weaponSpriteIndexByType = new Dictionary<Weapon.Type, int> ()
        {
            { Weapon.Type.Knife,        0 },
            { Weapon.Type.Pistol,       1 },
            { Weapon.Type.MachineGun,   2 },
            { Weapon.Type.ChainGun,     3 },
        };

        int[] m_faceSpriteRowByMinHealth = new int[] { 87, 74, 61, 48, 35, 22, 0 };
        int m_currentFaceRow;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Start ()
        {
            InitializeHUDNumbers ();
            InitializeWeaponDisplay ();
            InitializeFaceDisplay ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Update ()
        {
            // TODO: Face animation
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeFaceDisplay ()
        {
            m_faceSpriteSheet.Initialize ();
            Player.E_HealthChanged += SetFaceAnimationRow;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeWeaponDisplay ()
        {
            m_weaponSpriteSheet.Initialize ();
            Player.E_WeaponChanged += HandleWeaponChanged;
            HandleWeaponChanged ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandleWeaponChanged ()
        {
            int spriteIndex = m_weaponSpriteIndexByType[Player.HeldWeaponType];
            m_weaponRenderer.sprite = m_weaponSpriteSheet.GetSprite (spriteIndex, 0);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void SetFaceAnimationRow ()
        {
            for (int i = 0; i < m_faceSpriteRowByMinHealth.Length; i++)
            {
                int minHealthForRow = m_faceSpriteRowByMinHealth[i];
                if (Player.CurrentHealth > minHealthForRow)
                {
                    m_currentFaceRow = i;
                    break;
                }
            }

            m_faceSpriteRenderer.sprite = m_faceSpriteSheet.GetSprite (1, m_currentFaceRow);
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