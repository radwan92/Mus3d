namespace Mus3d
{
    public class HUDNumber : SpriteNumber
    {
        public Type _Type;

        public enum Type
        {
            Level,
            Score,
            Lives,
            Health,
            Ammo
        }
    }
}