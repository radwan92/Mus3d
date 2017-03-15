using System;
using UnityEngine;

namespace Mus3d
{
    public enum AmmoType
    {
        Knife,
        Bullet
    }

    public class Weapon : ScriptableObject
    {
        public event Action<Weapon> E_Shot;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Shot ()
        {
            E_Shot (this);
        }

        public Type         Type_;
        public Class        Class_;
        public AmmoType     AmmoType;
        public int          StartingAmmo;
        public SFX.Source   SoundSource;
        public bool         IsAvailable;
        public bool         IsRepeatable;
        public float        ShootDelay;
        public float        ShotDuration;
        public float        RangeSquare;
        public int          Damage;
        public Sprite       IdleSprite;
        public Sprite[]     AttackSprites = new Sprite[4];

        public enum Type
        {
            Knife      = 0,
            Pistol     = 1,
            MachineGun = 2,
            ChainGun   = 3
        }

        public enum Class
        {
            Melee,
            Gun
        }
    }
}