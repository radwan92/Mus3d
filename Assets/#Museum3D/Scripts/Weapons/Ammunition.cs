using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mus3d
{
    public static class Ammunition
    {
        public static readonly int MAX_AMMO = 99;

        public static event Action          E_AmmoChanged;
        public static event Action<Weapon>  E_OutOfAmmo;
        public static event Action          E_BackToAmmo;

        static Dictionary<AmmoType, int> m_ammo = new Dictionary<AmmoType, int> ()
        {
            { AmmoType.Knife, -1 },
            { AmmoType.Bullet, 2 }
        };

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Initialize ()
        {
            foreach (var weapon in Weaponry.Weapons)
            {
                weapon.E_Shot += DecreaseAmmo;
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Add (AmmoType ammoType, int amount)
        {
            bool hadNoAmmo = true;
            foreach (var weapon in Weaponry.Weapons)
            {
                if (weapon.Type_ == Weapon.Type.Knife)
                    continue;

                hadNoAmmo &= !HasFor (weapon);
            }

            m_ammo[ammoType] += amount;
            m_ammo[ammoType] = Mathf.Clamp (m_ammo[ammoType], -1, MAX_AMMO);

            if (hadNoAmmo)
                E_BackToAmmo ();

            E_AmmoChanged ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static int GetCount (Weapon weapon)
        {
            return m_ammo[weapon.AmmoType];
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static int GetCount ()
        {
            return m_ammo[AmmoType.Bullet];
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static bool HasMax ()
        {
            return m_ammo[AmmoType.Bullet] == MAX_AMMO;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static bool HasFor (Weapon weapon)
        {
            return GetCount (weapon) != 0;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        static void DecreaseAmmo (Weapon weapon)
        {
            m_ammo[weapon.AmmoType]--;

            if (GetCount (weapon) == 0)
            {
                E_OutOfAmmo (weapon);
            }

            E_AmmoChanged ();
        }
    }
}