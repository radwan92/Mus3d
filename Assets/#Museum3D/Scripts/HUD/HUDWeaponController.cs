using System.Collections.Generic;
using UnityEngine;

namespace Mus3d
{
    public class HUDWeaponController : MonoBehaviour
    {
        [SerializeField] SpriteSheet    m_weaponSpriteSheet;
        [SerializeField] SpriteRenderer m_weaponRenderer;

        Dictionary<Weapon.Type, int> m_weaponSpriteIndexByType = new Dictionary<Weapon.Type, int> ()
        {
            { Weapon.Type.Knife,        0 },
            { Weapon.Type.Pistol,       1 },
            { Weapon.Type.MachineGun,   2 },
            { Weapon.Type.ChainGun,     3 },
        };

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Start ()
        {
            InitializeWeaponDisplay ();
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
    }
}