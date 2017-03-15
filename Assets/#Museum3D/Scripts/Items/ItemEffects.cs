using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mus3d
{
    public static class ItemEffects
    {
        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Do (Item item)
        {
            m_effectsByItemType [item.Type].Invoke (item);
        }

        static Dictionary<ItemType, Action<Item>> m_effectsByItemType = new Dictionary<ItemType, Action<Item>> ()
        {
            /* ---------------------------------------------------------------------------------------------------------------------------------- */
            {
                ItemType.Ammo,
                item =>
                {
                    Ammunition.Add (AmmoType.Bullet, item.Value);
                }
            },

            /* ---------------------------------------------------------------------------------------------------------------------------------- */
            {
                ItemType.Weapon,
                item =>
                {
                    Weaponry.PickUp (item);
                }
            },

            /* ---------------------------------------------------------------------------------------------------------------------------------- */
            {
                ItemType.Healing,
                item =>
                {
                    Player.AddHealth (item.Value);
                }
            }
        };
    }
}