using DG.Tweening;
using System;
using UnityEngine;

namespace Mus3d
{
    public class GetPsyched : MonoBehaviour
    {
        public static event Action E_Finished;

        [SerializeField] Mesh       m_quadMesh;
        [SerializeField] Texture    m_getPsychedTexture;
        [SerializeField] Texture    m_loadingBarTexture;
        [SerializeField] Shader     m_loadingBarShader;

        static float        m_fadeTime = 0.8f;
        static Material     m_getPsychedMaterial;
        static Tweener      m_getPsychedFade;
        static Material     m_loadingBarMaterial;
        static Tweener      m_loadingBarFade;
        static bool         m_shouldDraw;
        static Transform    m_getPsychedTransform;

        Vector3   m_getPsychedScale;
        Vector3   m_loadingBarScale;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize ()
        {
            InitializeDrawingMaterials ();
            InitializeGetPsychedTransform ();
            InitializeLoadingBar ();

            PostRenderer.AddDraw (Render, 9);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeDrawingMaterials ()
        {
            var getPsychedShader = Shader.Find ("Unlit/Transparent Colored");

            m_getPsychedMaterial             = new Material (getPsychedShader);
            m_getPsychedMaterial.mainTexture = m_getPsychedTexture;
            m_getPsychedMaterial.color       = Color.white.WithAlpha (0f);
            m_getPsychedFade                 = m_getPsychedMaterial.DOFade (1f, m_fadeTime).SetAutoKill (false).Pause ().OnRewind (HandleScreenFinished);

            m_loadingBarMaterial             = new Material (m_loadingBarShader);
            m_loadingBarMaterial.color       = Color.white;
            m_loadingBarMaterial.mainTexture = m_loadingBarTexture;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeGetPsychedTransform ()
        {
            var getPsychedGameObject       = new GameObject (GetType ().ToString ());
            m_getPsychedTransform          = getPsychedGameObject.transform;

            var textureRatio  = m_getPsychedTexture.width / m_getPsychedTexture.height;
            m_getPsychedScale = new Vector3 (textureRatio, 1f);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        static void UpdateGetPsychedTransform ()
        {
            var cameraForwardNoY           = Player.BodyForward.WithY (0f).normalized;
            m_getPsychedTransform.position = Player.Position + cameraForwardNoY * 4f;
            m_getPsychedTransform.rotation = Quaternion.LookRotation (cameraForwardNoY);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeLoadingBar ()
        {
            var textureRatio  = m_loadingBarTexture.width / m_loadingBarTexture.height;
            m_loadingBarScale = new Vector3 (m_getPsychedScale.x * 0.9f, m_getPsychedScale.x / textureRatio);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Show ()
        {
            UpdateGetPsychedTransform ();

            m_shouldDraw = true;
            m_getPsychedFade.PlayForward ();
            m_loadingBarMaterial.SetFloat ("_Progress", 0f);
            m_loadingBarMaterial.SetColor ("_Color", m_loadingBarMaterial.color.WithAlpha (1f));
            m_loadingBarMaterial.DOFloat (1f, "_Progress", 3f).OnComplete (Hide);
        }
        
        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Hide ()
        {
            m_loadingBarMaterial.DOFade (0f, m_fadeTime);
            m_getPsychedFade.PlayBackwards ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void HandleScreenFinished ()
        {
            m_shouldDraw = false;
            E_Finished.InvokeIfNotNull ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Render ()
        {
            if (!m_shouldDraw)
                return;

            m_getPsychedMaterial.SetPass (0);
            Graphics.DrawMeshNow (m_quadMesh, Matrix4x4.TRS (m_getPsychedTransform.position, m_getPsychedTransform.rotation, m_getPsychedScale));
            m_loadingBarMaterial.SetPass (0);
            Graphics.DrawMeshNow (m_quadMesh, Matrix4x4.TRS (m_getPsychedTransform.position.WithYOffset (-0.45f), m_getPsychedTransform.rotation, m_loadingBarScale));
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void OnDestroy ()
        {
            if (m_getPsychedTransform != null)
                Destroy (m_getPsychedTransform.gameObject);
        }
    }
}