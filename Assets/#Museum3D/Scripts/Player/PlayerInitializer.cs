using System.Diagnostics;
using UnityEngine;

namespace Mus3d
{
    public class PlayerInitializer : MonoBehaviour
    {
        [SerializeField] GameObject m_weaponViewPrefab;
        [SerializeField] Material   m_bloodFlashMaterial;

        CharacterController m_characterController;
        WeaponController    m_weaponController;
        MovementController  m_movementController;
        TransformBob        m_weaponBob;

        GameObject m_playerComponentsObject;
        GameObject m_weaponViewObject;

        Transform m_playerBodyTransform;
        Transform m_playerHeadTransform;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Run ()
        {
            Weaponry.LoadAndInitialize ();
            Ammunition.Initialize ();

            m_playerComponentsObject = new GameObject ("PlayerComponents");
            m_playerComponentsObject.transform.SetParent (transform);

            DebugRun ();

            m_characterController = FindObjectOfType<CharacterController> ();
            m_playerBodyTransform = m_characterController.transform;
            m_playerHeadTransform = Camera.main.transform;

            InitializeBloodFlash ();
            InitializeWeaponView ();
            InitializeWeaponController ();
            InitializeMovementController ();
            InitializePlayerColliderAndItemPicker ();
            InitializePlayer ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializePlayer ()
        {
            Player.Initialize (m_characterController, m_weaponController, m_playerBodyTransform, m_playerHeadTransform);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeBloodFlash ()
        {
            var bloodFlash = Camera.main.gameObject.AddComponent<FaceFlash> ();
            bloodFlash.Initialize (m_bloodFlashMaterial);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        private void InitializePlayerColliderAndItemPicker ()
        {
            var colliderObject = new GameObject ("Mus_PlayerCollider");
            colliderObject.transform.SetParent (m_playerBodyTransform);
            colliderObject.layer = LayerMask.NameToLayer (Consts.PLAYER_LAYER);

            var playerCollider       = colliderObject.AddComponent<CapsuleCollider> ();
            playerCollider.radius    = m_characterController.radius;
            playerCollider.center    = m_characterController.center;
            playerCollider.height    = m_characterController.height;
            playerCollider.isTrigger = true;

            var playerRigidBody = colliderObject.AddComponent<Rigidbody> ();
            playerRigidBody.isKinematic = true;
            playerRigidBody.useGravity  = false;

            colliderObject.AddComponent<ItemPicker> ();

            m_playerBodyTransform.gameObject.layer = LayerMask.NameToLayer (Consts.PLAYER_LAYER);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        private void InitializeMovementController ()
        {
            m_movementController = m_playerComponentsObject.AddComponent<MovementController> ();
            m_movementController.Initialize (FindObjectOfType<CharacterController> (), m_playerHeadTransform);    // Disable moving by tapping for gvr
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        private void InitializeWeaponController ()
        {
            var weaponSpriteRenderer = m_weaponViewObject.GetComponent<SpriteRenderer> ();

            m_weaponController = m_playerComponentsObject.AddComponent<WeaponController> ();
            m_weaponController.Initialize (weaponSpriteRenderer);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        private void InitializeWeaponView ()
        {
            m_weaponViewObject = Instantiate (m_weaponViewPrefab);
            m_weaponViewObject.transform.SetParent (m_playerHeadTransform);
            m_weaponViewObject.transform.localPosition = new Vector3 (0f, 0f, 0.5f);
            m_weaponViewObject.transform.localRotation = Quaternion.identity;

            var weaponBob = m_weaponViewObject.GetComponent<TransformBob> ();
            weaponBob.SetSpeedSource (() => Player.Velocity.sqrMagnitude);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        [Conditional ("UNITY_EDITOR")]
        void DebugRun ()
        {
            var debugHeadController = m_playerComponentsObject.AddComponent<DebugHeadController> ();
            debugHeadController.Initialize (Camera.main.transform.parent);
        }
    }
}