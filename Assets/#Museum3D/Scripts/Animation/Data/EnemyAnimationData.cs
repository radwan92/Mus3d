namespace Mus3d.Anim
{
    public class EnemyAnimationData : AnimationData
    {
        public bool                 IsDirectional = true;
        public EnemyAnimator.State  State;
        public Direction            Direction;
    }
}