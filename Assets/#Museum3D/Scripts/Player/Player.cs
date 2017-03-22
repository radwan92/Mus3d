using System;
using UnityEngine;

namespace Mus3d
{
    public static class Player
    {
        public static readonly int MAX_HEALTH = 100;

        public static event Action E_WeaponChanged;
        public static event Action E_ScoreChanged;
        public static event Action E_HealthChanged;
        public static event Action E_LivesChanged;
        public static event Action E_LevelChanged;
        public static event Action E_Died;

        public static Vector3 Velocity  { get { return m_characterController.velocity; } }
        public static Vector3 Position  { get { return m_bodyTransform.position; } }
        public static Vector3 Forward   { get { return m_headTransform.forward; } }

        public static Weapon.Class HeldWeaponClass  { get { return m_weaponController.WeaponClass; } }
        public static Weapon.Type  HeldWeaponType   { get { return m_weaponController.WeaponType; } }

        public static bool IsDead           { get; private set; }
        public static int  CurrentHealth    { get; private set; }
        public static int  Lives            { get; private set; }
        public static int  Score            { get; private set; }   // TODO: Handle score
        public static int  Level            { get; private set; }

        static CharacterController  m_characterController;
        static WeaponController     m_weaponController;
        static Transform            m_headTransform;
        static Transform            m_bodyTransform;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Hit (int damage)
        {
            if (IsDead)
                return;

            CurrentHealth -= damage;
            E_HealthChanged ();

            FaceFlash.FlashColor (Color.red);
            // TODO: Handle death

            if (CurrentHealth > 0)
                return;

            Debug.Log ("PLAYER DIED");

            // Death
            IsDead = true;
            //E_Died ();

            Lives--;
            //E_LivesChanged ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Initialize (CharacterController characterController, WeaponController weaponController, Transform playerBodyTransfom, Transform playerHeadTransform)
        {
            m_characterController = characterController;
            m_weaponController    = weaponController;
            m_headTransform       = playerHeadTransform;
            m_bodyTransform       = playerBodyTransfom;

            m_weaponController.E_WeaponChanged += HandleWeaponChanged;

            CurrentHealth = MAX_HEALTH;
            Lives         = 1;
            Level         = 1;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        static void HandleWeaponChanged (Weapon weapon)
        {
            E_WeaponChanged ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Dispose ()
        {
            if (m_weaponController != null)
                m_weaponController.E_WeaponChanged -= HandleWeaponChanged;

            m_characterController = null;
            m_weaponController    = null;
            m_headTransform       = null;
            m_bodyTransform       = null;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void AddHealth (int amount)
        {
            CurrentHealth += amount;
            E_HealthChanged ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void AddScore (int score)
        {
            Score += score;
            E_ScoreChanged ();
        }
    }
}