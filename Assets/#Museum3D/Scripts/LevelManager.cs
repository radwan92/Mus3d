﻿using System.Collections.Generic;
using UnityEngine;

namespace Mus3d
{
    public static class LevelManager
    {
        static bool             m_isLevelLoaded;
        static GameObject       m_level;
        static List<Object>     m_dynamicItems = new List<Object> ();

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void LoadLevel (GameObject levelPrefab)
        {
            if (m_isLevelLoaded)
            {
                Debug.LogError ("Trying to load level on top of already loaded level", levelPrefab);
                return;
            }
            m_isLevelLoaded = true;

            GameObject.Instantiate (levelPrefab);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void UnloadLevel ()
        {
            Destroy (m_level);

            for (int i = 0; i < m_dynamicItems.Count; i++)
            {
                var dynamicItem = m_dynamicItems[i];

                if (dynamicItem != null)
                    Destroy (m_dynamicItems[i]);
            }

            m_dynamicItems.Clear ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void RegisterDynamicItem (Object item)
        {
            m_dynamicItems.Add (item);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        static void Destroy (Object obj)
        {
            Object.Destroy (obj);
        }

        /* ================================================================================================================================== */
        // INSTANTIATIONS
        /* ================================================================================================================================== */
        public static T Instantiate<T> (T original) where T : Object
        {
            var obj = Object.Instantiate (original);
            RegisterDynamicItem (obj);
            return obj;
        }

        public static T Instantiate<T> (T original, Transform parent) where T : Object
        {
            var obj = Object.Instantiate (original, parent);
            RegisterDynamicItem (obj);
            return obj;
        }

        public static T Instantiate<T> (T original, Vector3 position, Quaternion rotation) where T : Object
        {
            var obj = Object.Instantiate (original, position, rotation);
            RegisterDynamicItem (obj);
            return obj;
        }

        public static T Instantiate<T> (T original, Transform parent, bool worldPositionStays) where T : Object
        {
            var obj = Object.Instantiate (original, parent, worldPositionStays);
            RegisterDynamicItem (obj);
            return obj;
        }

        public static T Instantiate<T> (T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            var obj = Object.Instantiate (original, position, rotation, parent);
            RegisterDynamicItem (obj);
            return obj;
        }
    }
}