using UnityEngine;

namespace Mus3d
{
    public enum ItemType
    {
        Ammo,
        Healing,
        Weapon,
        Treasure
    }

    public class Item : ScriptableObject
    {
        public ItemType     Type;
        public int          Value;
        public Sprite       Sprite;
        public SFX.Source   PickupSound;
    }
}