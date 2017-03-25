using UnityEngine;

namespace Mus3d
{
    public class HUDInitializer : MonoBehaviour
    {
        [SerializeField] GameObject m_spriteDigitsPrefab;
        [SerializeField] GameObject m_hudPrefab;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Run ()
        {
            var spriteDigitsObject = Instantiate (m_spriteDigitsPrefab);
            spriteDigitsObject.GetComponent<SpriteDigits> ().Initialize ();

            var hudObject     = Instantiate (m_hudPrefab);
            var hudController = hudObject.GetComponentInChildren<HUDController> ();
            hudController.Initialize ();
        }
    }
}