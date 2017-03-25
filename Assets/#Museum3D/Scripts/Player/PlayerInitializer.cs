using System.Diagnostics;
using UnityEngine;

namespace Mus3d
{
    public class PlayerInitializer : MonoBehaviour
    {
        [SerializeField] GameObject m_weaponViewPrefab;

        CharacterController m_characterController;
        WeaponController    m_weaponController;
        MovementController  m_movementController;
        TransformBob        m_weaponBob;

        GameObject m_playerComponentsObject;

        Transform m_playerBodyTransform;
        Transform m_playerHeadTransform;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Run ()
        {
            Weaponry.LoadAndInitialize ();
            Ammunition.Initialize ();

            m_playerComponentsObject = new GameObject ("PlayerComponents");
            m_playerComponentsObject.transform.SetParent (transform);

            m_characterController = FindObjectOfType<CharacterController> ();
            m_playerBodyTransform = m_characterController.transform;
            m_playerHeadTransform = Camera.main.transform;

            DebugRun ();

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
        private void InitializePlayerColliderAndItemPicker ()
        {
            var colliderObject = new GameObject ("Mus_PlayerCollider");
            colliderObject.transform.SetParent (m_playerBodyTransform);
            colliderObject.transform.localPosition -= new Vector3 (0f, m_characterController.height * 0.5f, 0f);
            colliderObject.layer = LayerMask.NameToLayer (Consts.PLAYER_LAYER);

            var playerCollider       = colliderObject.AddComponent<SphereCollider> ();
            playerCollider.radius    = m_characterController.radius;
            playerCollider.center    = m_characterController.center;
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
            // TODO: Disable moving by tapping for gvr
            m_movementController = m_playerComponentsObject.AddComponent<MovementController> ();
            m_movementController.Initialize (FindObjectOfType<CharacterController> (), m_playerHeadTransform, m_playerBodyTransform);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        private void InitializeWeaponController ()
        {
            m_weaponController = m_playerComponentsObject.AddComponent<WeaponController> ();
            m_weaponController.Initialize (WeaponView.Renderer);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        private void InitializeWeaponView ()
        {
            WeaponView.Initialize (m_weaponViewPrefab, m_playerHeadTransform);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        [Conditional ("UNITY_EDITOR")]
        void DebugRun ()
        {
            var debugHeadController = m_playerComponentsObject.AddComponent<DebugHeadController> ();
            debugHeadController.Initialize (m_playerBodyTransform);
        }
    }
}