namespace Mus3d
{
    public static class Difficulty
    {
        public static Level Level_ { get; private set; }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static void Initialize ()
        {
            DifficultyMenu.E_DifficultySelected += SetLevel;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        static void SetLevel (Level level)
        {
            Level_ = level;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static float PlayerDamageTakenMultiplier
        {
            get
            {
                switch (Level_)
                {
                    case Level.CanIDaddy:
                        return 0.25f;
                    case Level.DontHurtMe:
                        return 0.5f;
                    case Level.BringEmOn:
                        return 1f;
                    case Level.DeathIncarnate:
                        return 2f;
                    default:
                        return 1f;
                }
            }
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static float EnemyShotChanceMultiplier
        {
            get
            {
                switch (Level_)
                {
                    case Level.CanIDaddy:
                    case Level.DontHurtMe:
                    case Level.BringEmOn:
                    default:
                        return 1f;
                    case Level.DeathIncarnate:
                        return 1.5f;
                }
            }
        }

        public enum Level
        {
            CanIDaddy      = 0,
            DontHurtMe     = 1,
            BringEmOn      = 2,
            DeathIncarnate = 3
        }
    }
}