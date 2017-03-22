using UnityEngine;

namespace Mus3d
{
    public class ItemObject : MusObject
    {
        [SerializeField] Item m_item;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Pickup ()
        {
            if (!CanPickup (m_item))
                return;

            ItemEffects.Do (m_item);
            Destroy (gameObject);   // TODO: Pool those objects
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected override void Awake ()
        {
            base.Awake ();

            gameObject.layer = LayerMask.NameToLayer (Consts.PICKUP_LAYER);

#if UNITY_EDITOR
            if (m_item == null)
                return;
#endif

            m_spriteRenderer.sprite = m_item.Sprite;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static bool CanPickup (Item item)
        {
            switch (item.Type)
            {
                case ItemType.Ammo:
                    return !Ammunition.HasMax ();
                case ItemType.Healing:
                    return Player.CurrentHealth < Player.MAX_HEALTH;
                default:
                    return true;
            }
        }
    }
}