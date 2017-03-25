using UnityEngine;

namespace Mus3d
{
    public class WeaponView
    {
        static GameObject       s_weaponViewObject;
        static SpriteRenderer   s_spriteRenderer;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static SpriteRenderer Renderer
        {
            get
            {
                if (s_spriteRenderer == null)
                    s_spriteRenderer = s_weaponViewObject.GetComponent<SpriteRenderer> ();
                return s_spriteRenderer;
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Initialize (GameObject weaponViewPrefab, Transform playerHeadTransform)
        {
            s_weaponViewObject = Object.Instantiate (weaponViewPrefab);
            s_weaponViewObject.transform.SetParent (playerHeadTransform);
            s_weaponViewObject.transform.localPosition = new Vector3 (0f, 0.15f, 0.65f);
            s_weaponViewObject.transform.localRotation = Quaternion.identity;

            var weaponBob = s_weaponViewObject.GetComponent<TransformBob> ();
            weaponBob.SetSpeedSource (() => Player.Velocity.sqrMagnitude);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Show ()
        {
            s_weaponViewObject.SetActive (true);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Hide ()
        {
            s_weaponViewObject.SetActive (false);
        }
    }
}