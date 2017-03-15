using System;
using UnityEngine;

namespace Mus3d
{
    public static class Player
    {
        public static event Action E_Died;

        public static Vector3 Velocity  { get { return m_characterController.velocity; } }
        public static Vector3 Position  { get { return m_bodyTransform.position; } }
        public static Vector3 Forward   { get { return m_headTransform.forward; } }

        public static Weapon.Class HeldWeaponClass { get { return m_weaponController.WeaponClass; } }

        public static bool    IsDead    { get; private set; }

        static CharacterController  m_characterController;
        static WeaponController     m_weaponController;
        static Transform            m_headTransform;
        static Transform            m_bodyTransform;
        static int                  m_currentHealth = 100;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Hit (int damage)
        {
            // TODO: Handle this
            //if (IsDead)
            {
                // TODO: Handle enemies state on player death

                //Debug.LogError ("Player got hit even though he's dead");
                //return;
            }

            m_currentHealth -= damage;

            BloodFlash.Show ();
            // TODO: Handle death

            if (m_currentHealth > 0)
                return;

            Debug.Log ("PLAYER DIED");

            // Death
            IsDead = true;
            //E_Died ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Initialize (CharacterController characterController, WeaponController weaponController, Transform playerBodyTransfom, Transform playerHeadTransform)
        {
            m_characterController = characterController;
            m_weaponController    = weaponController;
            m_headTransform       = playerHeadTransform;
            m_bodyTransform       = playerBodyTransfom;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void AddHealth (int amount)
        {
            m_currentHealth += amount;
        }
    }
}