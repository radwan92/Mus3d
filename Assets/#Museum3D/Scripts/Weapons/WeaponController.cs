using System;
using UnityEngine;

namespace Mus3d
{
    public class WeaponController : MonoBehaviour
    {
        public event Action<Weapon> E_WeaponChanged;

        Weapon m_weapon;

        SpriteRenderer  m_spriteRenderer;
        WeaponAnim      m_anim;
        bool            m_isOutOfAmmo;

        static bool s_isEnabled;
        static WeaponController s_instance;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public Weapon.Class WeaponClass
        {
            get { return m_weapon.Class_; }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public Weapon.Type WeaponType
        {
            get { return m_weapon.Type_; }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize (SpriteRenderer weaponRenderer)
        {
            s_instance = this;

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
            Gizmos.DrawLine (Player.Position, Player.Position + Player.HeadForward * 20f);
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

            if (E_WeaponChanged != null)
                E_WeaponChanged (m_weapon);
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
            if (!s_isEnabled)
                return;

            if (Inp.GetDown (Inp.Key.A)
                && Ammunition.HasFor (m_weapon))
            {
                m_anim.Start ();
            }

            if (Inp.GetUp (Inp.Key.A))
            {
                m_anim.Stop ();
            }

            if (Inp.GetDown (Inp.Key.LS))
            {
                ChangeToPreviousWeapon ();
            }

            if (Inp.GetDown (Inp.Key.RS))
            {
                ChangeToNextWeapon ();
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Enable ()
        {
            s_isEnabled = true;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Disable ()
        {
            s_isEnabled = false;
            s_instance.m_anim.Stop ();
        }
    }
}