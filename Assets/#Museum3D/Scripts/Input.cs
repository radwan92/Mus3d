using UnityEngine;

namespace Mus3d
{
    public static class Inp
    {
        public enum Key
        {
            Up,
            Down,
            Left,
            Right,
            X,
            Y,
            A,
            B,
            LS,
            RS,
            LT,
            RT,
            Start,
            Select
        }

        static IInp m_input = new KeyboardAndMouseInput ();

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static bool Get (Key key)
        {
            return m_input.Get (key);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static bool GetDown (Key key)
        {
            return m_input.GetDown (key);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static bool GetUp (Key key)
        {
            return m_input.GetUp (key);
        }

        /* ================================================================================================================================== */
        // KEYBOARD
        /* ================================================================================================================================== */
        class KeyboardAndMouseInput : IInp
        {
            /* ---------------------------------------------------------------------------------------------------------------------------------- */
            public bool Get (Key key)
            {
                if (key == Key.A)
                    return Input.GetMouseButton (0);
                else if (key == Key.B)
                    return Input.GetMouseButton (1);

                return Input.GetKey (KeyCodeForKey (key));
            }

            /* ---------------------------------------------------------------------------------------------------------------------------------- */
            public bool GetDown (Key key)
            {
                if (key == Key.A)
                    return Input.GetMouseButtonDown (0);
                else if (key == Key.B)
                    return Input.GetMouseButtonDown (1);

                return Input.GetKeyDown (KeyCodeForKey (key));
            }

            /* ---------------------------------------------------------------------------------------------------------------------------------- */
            public bool GetUp (Key key)
            {
                if (key == Key.A)
                    return Input.GetMouseButtonUp (0);
                else if (key == Key.B)
                    return Input.GetMouseButtonUp (1);

                return Input.GetKeyUp (KeyCodeForKey (key));
            }

            /* ---------------------------------------------------------------------------------------------------------------------------------- */
            KeyCode KeyCodeForKey (Key key)
            {
                switch (key)
                {
                    case Key.Up:
                        return KeyCode.W;
                    case Key.Down:
                        return KeyCode.S;
                    case Key.Left:
                        return KeyCode.A;
                    case Key.Right:
                        return KeyCode.D;
                    // Handled by mouse
                    //case Key.A:
                    //    return KeyCode.A;
                    //case Key.B:
                    //    return KeyCode.S;
                    case Key.X:
                        return KeyCode.Z;
                    case Key.Y:
                        return KeyCode.X;
                    case Key.LS:
                        return KeyCode.Q;
                    case Key.RS:
                        return KeyCode.W;
                    case Key.LT:
                        return KeyCode.Alpha1;
                    case Key.RT:
                        return KeyCode.Alpha2;
                    case Key.Start:
                        return KeyCode.Return;
                    case Key.Select:
                        return KeyCode.Space;
                }

                return KeyCode.None;
            }
        }
    }

    public interface IInp
    {
        bool Get (Inp.Key key);
        bool GetDown (Inp.Key key);
        bool GetUp (Inp.Key key);
    }
}