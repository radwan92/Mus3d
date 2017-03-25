using System;
using System.Collections;
using UnityEngine;
using Mus3d.Anim;
using Animation = Mus3d.Anim.Animation;

namespace Mus3d
{
    public class EnemyAnimator : IDisposable
    {
        public enum State
        {
            Idle,
            Walk,
            Die,
            Hit,
            Attack,
            Body
        }

        public event Action<State>              E_EndOfStateAnim;
        public event Action<Animation.Event>    E_EventFired;

        DirectionalAnimationSet<State, EnemyAnimation> m_animationSet;

        SpriteRenderer  m_renderer;
        State           m_state;
        Direction       m_direction;
        EnemyObject     m_enemyObject;

        EnemyAnimation  m_animation;
        Sprite[]        m_stateSprites;

        int  m_frame;
        bool m_loop = true;
        bool m_endOfAnimReached;

        bool m_isDisposing;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public EnemyAnimator (EnemyObject enemyObject, SpriteRenderer enemySpriteRenderer)
        {
            m_enemyObject  = enemyObject;
            m_renderer     = enemySpriteRenderer;
            m_animationSet = enemyObject.Enemy.AnimationSet;

            SetState (State.Idle, enemyObject.Forward);

            Coroutiner.Start (AnimationCoroutine ());
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void SetState (State state, Vector3? directionVector = null, bool loop = true, bool animateNow = true)
        {
            m_state            = state;
            m_loop             = loop;
            m_endOfAnimReached = false;
            m_frame            = 0;

            if (directionVector != null)
                m_direction = EnemyUtils.GetDirectionForVector (PlayerToEnemyViewVector (), directionVector.Value);

            SetAnimation ();

            if (animateNow)
                Animate ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void SetAnimation ()
        {
            m_animation     = m_animationSet[m_direction, m_state];
            m_stateSprites  = m_animation.SpriteSequence;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void SetDirection (Vector3 directionVector, bool animateNow)
        {
            var direction = EnemyUtils.GetDirectionForVector (PlayerToEnemyViewVector (), directionVector);
            SetDirection (direction, animateNow);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void SetDirection (Direction newDirection, bool animateNow)
        {
            m_direction = newDirection;

            SetAnimation ();

            if (animateNow)
                Animate ();
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void NextSpriteFrame ()
        {
            if (m_endOfAnimReached)
                return;

            if (!m_loop && m_frame >= m_stateSprites.Length)
            {
                m_endOfAnimReached = true;
                E_EndOfStateAnim (m_state);
            }
            else
            {
                Animate ();
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void Animate ()
        {
            m_renderer.sprite = m_stateSprites[m_frame % m_stateSprites.Length];
            m_frame++;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        IEnumerator AnimationCoroutine ()
        {
            while (!m_isDisposing)
            {
                NextSpriteFrame ();

                yield return m_animation.Interval;
                if (m_isDisposing)
                    break;

                FireAnimationEvents ();
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        void FireAnimationEvents ()
        {
            if (m_endOfAnimReached)
                return;

            EventData[] eventData = m_animation.EventData;
            for (int i = 0; i < eventData.Length; i++)
            {
                var data = eventData[i];

                if (data.FrameIndex == m_frame % m_stateSprites.Length)
                    E_EventFired (data.Event);
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        Vector3 PlayerToEnemyViewVector ()
        {
            return (m_enemyObject.Position - Player.Position).WithY (0f);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Dispose ()
        {
            m_isDisposing = true;
        }
    }
}