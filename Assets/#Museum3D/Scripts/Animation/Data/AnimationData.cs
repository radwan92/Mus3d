using UnityEngine;

namespace Mus3d.Anim
{
    public class AnimationData : ScriptableObject
    {
        public Vector2[]    SpritePositions = { Vector2.zero };
        public float        FrameInterval   = 0.2f;
        public EventData[]  EventData;
    }
}