using UnityEngine;

namespace Mus3d.Anim
{
    public class Animation
    {
        public Sprite[]         SpriteSequence  { get; private set; }
        public YieldInstruction Interval        { get; private set; }
        public EventData[]      EventData       { get; private set; }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public Animation (AnimationData animationData, SpriteSheet spriteSheet)
        {
            SpriteSequence = spriteSheet.GetSpriteSequence (animationData.SpritePositions);
            Interval       = new WaitForSeconds (animationData.FrameInterval);
            EventData      = animationData.EventData;
        }

        public enum Event
        {
            Attack,
            AttackStart
        }
    }
}