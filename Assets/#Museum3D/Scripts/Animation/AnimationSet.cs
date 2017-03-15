using System;
using System.Collections.Generic;

namespace Mus3d.Anim
{
    public class AnimationSet<TAnimKind, TAnim> 
        where TAnimKind : struct, IConvertible 
        where TAnim : Animation
    {
        protected Dictionary<TAnimKind, TAnim> m_animationsByKind = new Dictionary<TAnimKind, TAnim> (16);

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void AddAnimation (TAnimKind type, TAnim animation)
        {
            m_animationsByKind.Add (type, animation);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public TAnim this[TAnimKind animationType]
        {
            get
            {
                TAnim anim = null;
                m_animationsByKind.TryGetValue (animationType, out anim);
                return anim;
            }
        }
    }
}