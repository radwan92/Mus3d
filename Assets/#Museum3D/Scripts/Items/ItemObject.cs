using UnityEngine;

namespace Mus3d
{
    public class ItemObject : MusObject
    {
        [SerializeField] Item m_item;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Pickup ()
        {
            Sounds.Play (m_item.PickupSound);
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
    }
}