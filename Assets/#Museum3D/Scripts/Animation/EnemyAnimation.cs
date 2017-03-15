namespace Mus3d.Anim
{
    public class EnemyAnimation : Animation
    {
        public bool                 IsDirectional   { get; private set; }
        public EnemyAnimator.State  State           { get; private set; }
        public Direction            Direction       { get; private set; }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public EnemyAnimation (EnemyAnimationData animationData, SpriteSheet spriteSheet) : base (animationData, spriteSheet)
        {
            State         = animationData.State;
            Direction     = animationData.Direction;
            IsDirectional = animationData.IsDirectional;
        }
    }
}