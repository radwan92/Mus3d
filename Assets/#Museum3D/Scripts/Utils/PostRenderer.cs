using System;
using UnityEngine;

namespace Mus3d
{
    public class PostRenderer : MonoBehaviour
    {
        static Action[] m_renderActions = new Action [10];

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void AddDraw (Action drawAction, int index)
        {
            m_renderActions[index] += drawAction;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void OnPostRender ()
        {
            for (int i = 0; i < m_renderActions.Length; i++)
                m_renderActions[i].InvokeIfNotNull ();
        }
    }
}