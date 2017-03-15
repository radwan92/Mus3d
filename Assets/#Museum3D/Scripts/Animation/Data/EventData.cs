using System;

namespace Mus3d.Anim
{
    [Serializable]
    public struct EventData
    {
        public Animation.Event  Event;
        public int              FrameIndex;
    }
}