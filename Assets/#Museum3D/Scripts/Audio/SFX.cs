using System.Collections.Generic;
using UnityEngine;

namespace Mus3d
{
    public static class SFX
    {
        // 32
        public enum Source
        {
            Knife = 0, Pistol = 1, MachineGun = 2, ChainGun = 3,
            AmmoPickup = 4, MachineGunPickup = 5, ChainGunPickup = 6,
            KeyPickup = 7, CrossPickup = 8, ExtraLifePickup = 9,
            GuardShout1 = 10, GuardDeath1 = 11, GuardDeath2 = 24, GuardDeath3 = 25, GuardHit1 = 12, GuardHit2 = 13, GuardHit3 = 14, GuardAttack1 = 15,
            SSShout1 = 16, SSShout2 = 29, SSShout3 = 31, SSAttack1 = 17, SSDeath1 = 19,
            OfficerShout1 = 20, OfficerShout2 = 30, OfficerDeath1 = 21, OfficerDeath2 = 22,
            HoundDeath1 = 18, HoundShout1 = 23,
            GenericDeath1 = 26, GenericDeath2 = 27, GenericDeath3 = 28,
            Explosion1 = 32

        }

        static Dictionary<Source, AudioClip> m_soundsBySource = new Dictionary<Source, AudioClip> ();

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static AudioClip Get (Source source)
        {
            return m_soundsBySource[source];
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Load ()
        {
            var bgms = Resources.LoadAll <AudioClip> ("SFX");
            for (int i = 0; i < bgms.Length; i++)
            {
                var clip = bgms[i];
                var clipPostfix = clip.name.Split ('_')[2];

                Source clipSource;
                if (!sourceByClipPostfix.TryGetValue (clipPostfix, out clipSource))
                    continue;

                m_soundsBySource.Add (clipSource, clip);
            }
        }

        static Dictionary<string, Source> sourceByClipPostfix = new Dictionary<string, Source> ()
        {
            { "0", Source.GuardShout1 },
            { "46", Source.Knife }, { "4", Source.MachineGun }, { "5", Source.Pistol }, { "6", Source.ChainGun }, 
            { "47", Source.ExtraLifePickup }, { "48", Source.AmmoPickup }, { "51", Source.KeyPickup }, { "52", Source.CrossPickup },
            { "49", Source.ChainGunPickup }, { "50", Source.MachineGunPickup },
            { "9", Source.GuardDeath1 }, { "34", Source.GuardDeath2 }, { "35", Source.GuardDeath3 }, { "12", Source.GuardHit1 }, { "13", Source.GuardHit2 }, { "14", Source.GuardHit3 }, { "21", Source.GuardAttack1 },
            { "7", Source.SSShout1 }, { "23", Source.SSShout2 }, { "37", Source.SSShout3 }, { "11", Source.SSAttack1 }, { "17", Source.SSDeath1 },
            { "18", Source.OfficerShout1 }, { "27", Source.OfficerShout2 }, { "19", Source.OfficerDeath1 }, { "20", Source.OfficerDeath2 },
            { "16", Source.HoundDeath1 }, { "29", Source.HoundShout1 },
            { "40", Source.GenericDeath1 }, { "41", Source.GenericDeath2 }, { "42", Source.GenericDeath3 },
            { "53", Source.Explosion1 }
        };
    }
}