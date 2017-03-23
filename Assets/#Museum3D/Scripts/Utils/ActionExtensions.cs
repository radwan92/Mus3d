using System;

namespace Mus3d
{
    public static class ActionExtensions
    {
        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static bool InvokeIfNotNull (this Action action)
        {
            if (action != null)
            {
                action ();
                return true;
            }

            return false;
        }
    }
}