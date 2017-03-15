using UnityEngine;
using Mus3d.Anim;
using AnimationSet = Mus3d.Anim.DirectionalAnimationSet<Mus3d.EnemyAnimator.State, Mus3d.Anim.EnemyAnimation>;

namespace Mus3d
{
    public class EnemyBillboard_Editor : Billboard_Editor
    {
        [SerializeField] protected EnemyObject m_enemyObject;

        protected AnimationSet  m_animationSet;
        protected Transform     m_enemyBodyTransform;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected override bool TrySetComponents ()
        {
            base.TrySetComponents ();

            if (m_enemyObject == null)
            {
                m_enemyObject = m_spriteTransform.GetComponent<EnemyObject> ();
            }

            if (m_enemyObject != null)
            {
                LoadBaseSprites ();    
                m_enemyBodyTransform = m_enemyObject.Ai != null ? m_enemyObject.Ai.transform : null;
            }

            return AreComponentsSet;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void LoadBaseSprites ()
        {
            m_animationSet = new AnimationSet ();
            m_enemyObject.EnemyTemplate.SpriteSheet.Initialize ();

            var animationDataSet = m_enemyObject.EnemyTemplate.AnimationDataSet[0];
            for (int i = 0; i < animationDataSet.AnimationData.Length; i++)
            {
                var animation = new EnemyAnimation (animationDataSet.AnimationData[i], m_enemyObject.EnemyTemplate.SpriteSheet);
                m_animationSet.AddAnimation (animation.State, animation.Direction, animation);
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected override bool AreComponentsSet
        {
            get { return base.AreComponentsSet && m_enemyObject != null && m_animationSet != null && m_enemyBodyTransform != null; }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected override void OnRenderObject ()
        {
            base.OnRenderObject ();

            if (!ShouldRenderObject)
                return;

            if (!AreComponentsSet && !TrySetComponents ())
                return;

            var camToEnemyVector = (m_enemyBodyTransform.position - m_sceneCameraTransform.position).WithY (0f);
            var spriteDirection  = EnemyUtils.GetDirectionForVector (camToEnemyVector, m_enemyBodyTransform.forward);
            var sprite           = m_animationSet[spriteDirection, EnemyAnimator.State.Idle].SpriteSequence[0];

            m_spriteRenderer.sprite = sprite;
        }
    }
}