using System;
using UnityEngine;
using Mus3d.Anim;
using AnimationSet = Mus3d.Anim.DirectionalAnimationSet<Mus3d.EnemyAnimator.State, Mus3d.Anim.EnemyAnimation>;

namespace Mus3d
{
    [Serializable]
    public class ItemDrop
    {
        public GameObject ItemObjectPrefab;

        [Range (0f, 100f)]
        public float Chance;
    }

    public class Enemy : ScriptableObject
    {
        public Type Type_;

        public int      Health              = 30;
        public float    Speed               = 2;
        public float    HitboxRadius        = 0.5f;
        public float    AttackRange         = 8f;
        public float    AttackRangeOptimal  = 5f;
        public float    SightRange          = 12f;
        public float    HearingRange        = 6f;
        public int      Score               = 100;

        public SpriteSheet              SpriteSheet;
        public EnemyAnimationDataSet[]  AnimationDataSet;
        public EnemyAnimationData[]     AnimationData;
        public AnimationSet             AnimationSet;

        public SFX.Source[] DeathSounds     = { SFX.Source.GuardDeath1 };
        public SFX.Source[] AttackSounds    = { SFX.Source.GuardAttack1 };
        public SFX.Source[] GotHitSounds    = { SFX.Source.GuardHit1 };
        public SFX.Source[] ShoutSounds     = { SFX.Source.GuardShout1 };

        public ItemDrop[] Drops = { null };

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void InitializeAnimations ()
        {
            AnimationSet = new AnimationSet ();

            for (int i = 0; i < AnimationData.Length; i++)
                LoadAnimationDataIntoAnimationSet (AnimationData[i]);
            for (int i = 0; i < AnimationDataSet.Length; i++)
                for (int dataIndex = 0; dataIndex < AnimationDataSet[i].AnimationData.Length; dataIndex++)
                    LoadAnimationDataIntoAnimationSet (AnimationDataSet[i].AnimationData[dataIndex]);

        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void LoadAnimationDataIntoAnimationSet (EnemyAnimationData animationData)
        {
            var animation = new EnemyAnimation (animationData, SpriteSheet);

            if (animation.IsDirectional)
                AnimationSet.AddAnimation (animation.State, animation.Direction, animation);
            else
                AnimationSet.AddAnimation (animation.State, animation);
        }

        public enum Type
        {
            Guard,
            Hound,
            Officer,
            SS
        }
    }
}