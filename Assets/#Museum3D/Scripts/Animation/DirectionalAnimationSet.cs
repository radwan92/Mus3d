using System;
using System.Collections.Generic;

namespace Mus3d.Anim
{
    public class DirectionalAnimationSet<TAnimKind, TAnim> : AnimationSet<TAnimKind, TAnim>
        where TAnimKind : struct, IConvertible 
        where TAnim : Animation
    {
        protected Dictionary<Direction, Dictionary<TAnimKind, TAnim>> m_animationSetsByDirection = new Dictionary<Direction, Dictionary<TAnimKind, TAnim>> (8);

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public DirectionalAnimationSet ()
        {
            var directions = Enum.GetValues (typeof (Direction));
            for (int i = 0; i < directions.Length; i++)
            {
                Direction direction = (Direction)directions.GetValue (i);
                m_animationSetsByDirection.Add (direction, new Dictionary<TAnimKind, TAnim> (16));
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void AddAnimation (TAnimKind type, Direction direction, TAnim animation)
        {
            m_animationSetsByDirection[direction].Add (type, animation);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public TAnim this[Direction direction, TAnimKind animationType]
        {
            get
            {
                TAnim anim = null;
                Dictionary<TAnimKind, TAnim> animationSet = null;
                
                if (m_animationSetsByDirection.TryGetValue (direction, out animationSet))
                    animationSet.TryGetValue (animationType, out anim);

                if (anim == null)
                    m_animationsByKind.TryGetValue (animationType, out anim);

                return anim;
            }

            set
            {
                m_animationsByKind.Add (animationType, value);
            }
        }
    }
}