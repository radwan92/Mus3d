using System;
using UnityEngine;

namespace Mus3d
{
    public static class Inp
    {
        public enum Axis
        {
            L_Horizontal,
            L_Vertical,
            R_Horizontal,
            R_Vertical,
            LT,
            RT
        }

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
            Start,
            Select
        }

#if UNITY_EDITOR
        static IInp m_input = new KeyboardAndMouseInput ();
#elif UNITY_ANDROID
        static IInp m_input = new GearVrInput ();
#endif

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public static float Get (Axis axis)
        {
            return m_input.Get (axis);
        }

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
        // GEARVR
        /* ================================================================================================================================== */
        class GearVrInput : IInp
        {
            /* ---------------------------------------------------------------------------------------------------------------------------------- */
            public float Get (Axis axis)
            {
                switch (axis)
                {
                    case Axis.L_Horizontal:
                        return OVRInput.Get (OVRInput.Axis2D.PrimaryThumbstick).x;
                    case Axis.L_Vertical:
                        return OVRInput.Get (OVRInput.Axis2D.PrimaryThumbstick).y;
                    case Axis.R_Horizontal:
                        return OVRInput.Get (OVRInput.Axis2D.SecondaryThumbstick).x;
                    case Axis.R_Vertical:
                        return OVRInput.Get (OVRInput.Axis2D.SecondaryThumbstick).x;
                    case Axis.LT:
                        return OVRInput.Get (OVRInput.Axis1D.PrimaryIndexTrigger);
                    case Axis.RT:
                        return OVRInput.Get (OVRInput.Axis1D.SecondaryIndexTrigger);
                    default:
                        Debug.LogError ("Using unimplemented axis");
                        return 0f;
                }
            }

            /* ---------------------------------------------------------------------------------------------------------------------------------- */
            public bool Get (Key key)
            {
                return OVRInput.Get (OvrButtonForKey (key));
            }

            /* ---------------------------------------------------------------------------------------------------------------------------------- */
            public bool GetDown (Key key)
            {
                return OVRInput.GetDown (OvrButtonForKey (key));
            }

            /* ---------------------------------------------------------------------------------------------------------------------------------- */
            public bool GetUp (Key key)
            {
                return OVRInput.GetUp (OvrButtonForKey (key));
            }

            /* ---------------------------------------------------------------------------------------------------------------------------------- */
            OVRInput.Button OvrButtonForKey (Key key)
            {
                switch (key)
                {
                    case Key.Up:
                        return OVRInput.Button.DpadUp;
                    case Key.Down:
                        return OVRInput.Button.DpadDown;
                    case Key.Left:
                        return OVRInput.Button.DpadLeft;
                    case Key.Right:
                        return OVRInput.Button.DpadRight;
                    case Key.A:
                        return OVRInput.Button.One;
                    case Key.B:
                        return OVRInput.Button.Two;
                    case Key.X:
                        return OVRInput.Button.Three;
                    case Key.Y:
                        return OVRInput.Button.Four;
                    case Key.LS:
                        return OVRInput.Button.PrimaryShoulder;
                    case Key.RS:
                        return OVRInput.Button.SecondaryShoulder;
                    case Key.Start:
                        return OVRInput.Button.Start;
                    case Key.Select:
                        return OVRInput.Button.Back;
                }

                return OVRInput.Button.None;
            }
        }

        /* ================================================================================================================================== */
        // KEYBOARD
        /* ================================================================================================================================== */
        class KeyboardAndMouseInput : IInp
        {
            /* ---------------------------------------------------------------------------------------------------------------------------------- */
            public float Get (Axis axis)
            {
                switch (axis)
                {
                    case Axis.L_Horizontal:
                        return Get (Key.Right) ? 1f : (Get (Key.Left) ? -1f : 0f);
                    case Axis.L_Vertical:
                        return Get (Key.Up) ? 1f : (Get (Key.Down) ? -1f : 0f);
                    case Axis.R_Horizontal:
                        return Input.GetAxis ("MouseX");
                    case Axis.R_Vertical:
                        return Input.GetAxis ("MouseY");
                    case Axis.LT:
                        return Input.GetKey (KeyCode.Q) ? 1f : 0f;
                    case Axis.RT:
                        return Input.GetKey (KeyCode.E) ? 1f : 0f;
                    default:
                        Debug.LogError ("Using unimplemented axis");
                        return 0f;
                }
            }

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
                        return KeyCode.Alpha1;
                    case Key.RS:
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
        float Get (Inp.Axis axis);
        bool GetDown (Inp.Key key);
        bool GetUp (Inp.Key key);
    }
}