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

        static Dictionary<Weapon.Type, SFX.Source> m_soundByWeaponType = new Dictionary<Weapon.Type, SFX.Source> ()
        {
            { Weapon.Type.ChainGun, SFX.Source.MachineGunPickup },
            { Weapon.Type.MachineGun, SFX.Source.ChainGunPickup }
        };

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Reset ()
        {
            foreach (var weapon in m_weaponsByType.Values)
                weapon.IsAvailable = false;

            m_weaponsByType[Weapon.Type.Knife].IsAvailable  = true;
            m_weaponsByType[Weapon.Type.Pistol].IsAvailable = true;

            E_NewAvailable (GetBest ());
        }

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

            bool wasAvailable = pickedWeapon.IsAvailable;
            pickedWeapon.IsAvailable = true;


            if (!wasAvailable)
            {
                Ammunition.Add (pickedWeapon.AmmoType, pickedWeapon.StartingAmmo);
                Sounds.Play (weaponAsItem.PickupSound);
                E_NewAvailable (pickedWeapon);
            }
            else
            {
                Ammunition.Add (pickedWeapon.AmmoType, pickedWeapon.AdditionalAmmo);
                Sounds.Play (SFX.Source.AmmoPickup);
            }
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