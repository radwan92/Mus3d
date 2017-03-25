using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mus3d
{
    public static class Enemies
    {
        public static event Action E_Loaded;

        static Dictionary<Enemy.Type, Enemy> m_enemiesByType = new Dictionary<Enemy.Type, Enemy> ();

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Enemy GetInstance (Enemy template)
        {
            var instance = GameObject.Instantiate<Enemy> (template);
            instance.AnimationSet = template.AnimationSet;
            instance.SpriteSheet  = template.SpriteSheet;
            return instance;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static Enemy GetInstance (Enemy.Type enemyType)
        {
            var template = m_enemiesByType[enemyType];
            var instance = GameObject.Instantiate<Enemy> (template);
            instance.AnimationSet = template.AnimationSet;
            instance.SpriteSheet  = template.SpriteSheet;
            return instance;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Load ()
        {
            var enemies = Resources.LoadAll<Enemy> ("Enemies");

            for (int i = 0; i < enemies.Length; i++)
            {
                var enemy = enemies[i];
                enemy.SpriteSheet.Initialize ();
                enemy.InitializeAnimations ();
                m_enemiesByType.Add (enemy.Type_, enemy);
            }

            E_Loaded.InvokeIfNotNull ();
        }
    }
}