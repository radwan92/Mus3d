using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mus3d
{
    public static class Weaponry
    {
        public static event Action<Weapon> E_NewAvailable;

        public static int Count { get; private set; }
        public static IEnumerable<Weapon> Weapons { get { return m_weaponsByType.Values; } }

        static Dictionary<Weapon.Type, Weapon> m_weaponsByType = new Dictionary<Weapon.Type, Weapon> ();

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Weapon Get (Weapon.Type weaponType)
        {
            return m_weaponsByType[weaponType];
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Weapon GetBest ()
        {
            var weaponTypes = Enum.GetValues (typeof (Weapon.Type));
            for (int i = weaponTypes.Length - 1; i >= 0; i--)
            {
                var type   = (Weapon.Type)weaponTypes.GetValue (i);
                var weapon = m_weaponsByType[type]; 

                if (CanUse (weapon))
                    return weapon;
            }

            return m_weaponsByType[Weapon.Type.Knife]; // Fallback
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void PickUp (Item weaponAsItem)
        {
            var weaponType   = (Weapon.Type)weaponAsItem.Value;
            var pickedWeapon = Get (weaponType);

            Ammunition.Add (pickedWeapon.AmmoType, pickedWeapon.StartingAmmo);
            bool wasAvailable = pickedWeapon.IsAvailable;
            pickedWeapon.IsAvailable = true;

            if (!wasAvailable)
                E_NewAvailable (pickedWeapon);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static bool CanUse (Weapon weapon)
        {
            return weapon.IsAvailable && Ammunition.HasFor (weapon);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Weapon GetNextAvailableAfter (Weapon.Type weaponType)
        {
            Weapon nextWeapon;
            while (true)
            {
                weaponType = (Weapon.Type)(((int)weaponType + 1) % Count);

                nextWeapon = m_weaponsByType[weaponType];

                if (CanUse (nextWeapon))
                    return nextWeapon;
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Weapon GetPreviousAvailableAfter (Weapon.Type weaponType)
        {
            Weapon previousWeapon;
            while (true)
            {
                int weaponTypeIndex = ((int)weaponType - 1);
                if (weaponTypeIndex < 0)
                    weaponTypeIndex = Count - 1;

                weaponType = (Weapon.Type)weaponTypeIndex;

                previousWeapon = m_weaponsByType[weaponType];

                if (CanUse (previousWeapon))
                    return previousWeapon;
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void LoadAndInitialize ()
        {
            var loadedWeapons = Resources.LoadAll <Weapon> ("Weapons");
            for (int i = 0; i < loadedWeapons.Length; i++)
            {
                var weapon = GameObject.Instantiate<Weapon> (loadedWeapons[i]);
                m_weaponsByType.Add (weapon.Type_, weapon);
            }

            Count = m_weaponsByType.Count;
        }
    }
}