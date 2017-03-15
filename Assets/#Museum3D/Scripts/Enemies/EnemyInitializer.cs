using UnityEngine;

namespace Mus3d
{
    public class EnemyInitializer : MonoBehaviour
    {
        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Run ()
        {
            Enemies.Load ();
            Scanner.Initialize ();
        }
    }
}