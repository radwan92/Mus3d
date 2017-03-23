using DG.Tweening;
using UnityEngine;

namespace Mus3d
{
    public class GetPsyched : MonoBehaviour
    {
        [SerializeField] Mesh    m_quadMesh;
        [SerializeField] Texture m_getPsychedTexture;
        [SerializeField] float   m_fadeTime = 0.3f;

        static Material m_material;
        static Tweener  m_fade;

        Transform m_getPsychedTransform;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize ()
        {
            m_material = new Material (Shader.Find ("Unlit/Texture"));
            m_material.mainTexture = m_getPsychedTexture;
            m_material.color = Color.white.WithAlpha (0f);
            m_fade = m_material.DOFade (1f, m_fadeTime).SetAutoKill (false).Pause ();

            InitializeGetPsychedTransform ();

            PostRenderer.AddDraw (Render, 9);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeGetPsychedTransform ()
        {
            var getPsychedGameObject = new GameObject ("GetPsyched");
            m_getPsychedTransform = getPsychedGameObject.transform;
            var mainCameraTransform = Camera.main.transform;
            var cameraForwardNoY = mainCameraTransform.forward.WithY (0f).normalized;
            m_getPsychedTransform.position = mainCameraTransform.position + cameraForwardNoY * 3f;
            m_getPsychedTransform.rotation = Quaternion.LookRotation (-cameraForwardNoY);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Show ()
        {
            m_fade.PlayForward ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Hide ()
        {
            m_fade.PlayBackwards ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Render ()
        {
            Graphics.DrawMesh (m_quadMesh, m_getPsychedTransform.position, m_getPsychedTransform.rotation, m_material, 3000);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void OnDestroy ()
        {
            if (m_getPsychedTransform != null)
                Destroy (m_getPsychedTransform.gameObject);
        }
    }
}