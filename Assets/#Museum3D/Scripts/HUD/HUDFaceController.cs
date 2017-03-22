using UnityEngine;

namespace Mus3d
{
    public class HUDFaceController : MonoBehaviour
    {
        [SerializeField] SpriteSheet    m_faceSpriteSheet;
        [SerializeField] SpriteRenderer m_faceSpriteRenderer;

        int[]   m_faceSpriteRowByMinHealth = new int[] { 87, 74, 61, 48, 35, 22, 0 };
        int     m_currentFaceRow;
        float   m_nextFaceUpdateTime;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Start ()
        {
            InitializeFaceDisplay ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void InitializeFaceDisplay ()
        {
            m_faceSpriteSheet.Initialize ();
            Player.E_HealthChanged += SetFaceAnimationRow;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Update ()
        {
            UpdateFaceAnimation ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void UpdateFaceAnimation ()
        {
            if (Time.time < m_nextFaceUpdateTime)
                return;

            SetRandomFaceSprite ();

            m_nextFaceUpdateTime = Time.time + Random.Range (0.8f, 2f);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void SetRandomFaceSprite ()
        {
            var faceIndex  = Random.Range (0, 3);
            var faceSprite = m_faceSpriteSheet.GetSprite (faceIndex, m_currentFaceRow);

            m_faceSpriteRenderer.sprite = faceSprite;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void SetFaceAnimationRow ()
        {
            for (int i = 0; i < m_faceSpriteRowByMinHealth.Length; i++)
            {
                int minHealthForRow = m_faceSpriteRowByMinHealth[i];
                if (Player.CurrentHealth > minHealthForRow)
                {
                    m_currentFaceRow = i;
                    break;
                }
            }

            SetRandomFaceSprite ();
        }
    }
}