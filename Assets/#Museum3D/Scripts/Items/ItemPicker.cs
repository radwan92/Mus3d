using UnityEngine;

namespace Mus3d
{
    public class ItemPicker : MonoBehaviour
    {
        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void OnTriggerStay (Collider collider)
        {
            ItemObject foundObject = collider.GetComponent<ItemObject> ();

            if (foundObject != null)
                foundObject.Pickup ();
        }
    }
}