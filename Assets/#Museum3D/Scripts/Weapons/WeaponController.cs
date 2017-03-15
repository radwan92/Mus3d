﻿using UnityEngine;

namespace Mus3d
{
    public class WeaponController : MonoBehaviour
    {
        Weapon m_weapon;

        SpriteRenderer  m_spriteRenderer;
        WeaponAnim      m_anim;
        bool            m_isOutOfAmmo;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public Weapon.Class WeaponClass
        {
            get { return m_weapon.Class_; }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize (SpriteRenderer weaponRenderer)
        {
            m_spriteRenderer = weaponRenderer;
            m_anim           = new WeaponAnim (m_spriteRenderer);

            m_anim.E_Shot         += HandleWeaponShot;
            m_anim.E_ShotFinished += HandleWeaponShotFinished;

            Weaponry.E_NewAvailable += ChangeWeapon;
            Ammunition.E_OutOfAmmo  += HandleOutOfAmmo;
            Ammunition.E_BackToAmmo += ChangeToPreviousWeapon;

            ChangeWeapon (Weaponry.GetBest ());
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void OnDrawGizmos ()
        {
            Gizmos.DrawLine (Player.Position, Player.Position + Player.Forward * 20f);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandleWeaponShot ()
        {
            Sounds.Play (m_weapon.SoundSource);
            m_weapon.Shot ();
            Scanner.ScanPlayerToEnemiesShot (m_weapon.RangeSquare);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandleWeaponShotFinished ()
        {
            if (!m_isOutOfAmmo)
                return;

            ChangeToNextWeapon ();

            m_isOutOfAmmo = false;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandleOutOfAmmo (Weapon weapon)
        {
            m_isOutOfAmmo = true;
            m_anim.Stop ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void ChangeWeapon (Weapon weapon)
        {
            if (!weapon.IsAvailable || m_anim.InAction)
                return;

            m_weapon = weapon;
            m_anim.ChangeWeapon (m_weapon);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void ChangeToNextWeapon ()
        {
            var weapon = Weaponry.GetNextAvailableAfter (m_weapon.Type_);

            if (m_weapon == weapon)
                return;

            ChangeWeapon (weapon);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void ChangeToPreviousWeapon ()
        {
            var weapon = Weaponry.GetPreviousAvailableAfter (m_weapon.Type_);

            if (m_weapon == weapon)
                return;

            ChangeWeapon (weapon);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Update ()
        {
            if (Inp.GetDown (Inp.Key.A)
                && Ammunition.HasFor (m_weapon))
            {
                m_anim.Start ();
            }

            if (Inp.GetUp (Inp.Key.A))
            {
                m_anim.Stop ();
            }

            if (Inp.GetDown (Inp.Key.LT))
            {
                ChangeToPreviousWeapon ();
            }

            if (Inp.GetDown (Inp.Key.RT))
            {
                ChangeToNextWeapon ();
            }
        }
    }
}