using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

using OpenglTestingCs.Engine.Enums;

using System;
using System.Numerics;

namespace OpenglTestingCs.Engine
{
    public class InputHandler
    {
        private IWindow window;

        private Vector2 lastMousePosition = Vector2.Zero;
        private Vector2 mouseMoveVector = Vector2.Zero;

        private float mouseMoveSensitivity;

        private IKeyboard keyboard;
        private CameraMove cameraMove = CameraMove.None;

        public InputHandler(IWindow window)
        {
            mouseMoveSensitivity = 1.0f;
            this.window = window;

            IInputContext input = window.CreateInput();

            keyboard = input.Keyboards[0];

            for (int i = 0; i < input.Keyboards.Count; i++)
            {
                input.Keyboards[i].KeyDown += this.KeyDown;
                input.Keyboards[i].KeyUp += this.KeyUp;
            }

            for (int i = 0; i < input.Mice.Count; i++)
            {
                input.Mice[i].Cursor.CursorMode = CursorMode.Raw;
                input.Mice[i].MouseMove += this.MouseMove;
                input.Mice[i].MouseDown += this.MouseDown;
                input.Mice[i].MouseUp += this.MouseUp;
                input.Mice[i].Scroll += this.MouseScroll;
                input.Mice[i].Click += this.MouseClick;
            }
        }

        public void onUpdate()
        {
            mouseMoveVector = Vector2.Zero;
        }

        public void KeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            Console.WriteLine("Key down test: " + key);

            if(key == Key.Escape)
            {
                window.Close();
            }
        }

        public void KeyUp(IKeyboard keyboard, Key key, int arg3)
        {
            Console.WriteLine("Key up test: " + key);

            if (key == Key.Escape)
            {
                window.Close();
            }
        }

        public void MouseMove(IMouse mouse, Vector2 coords)
        {
            if(lastMousePosition == default) { lastMousePosition = coords; return; }
            mouseMoveVector = (coords - lastMousePosition) * mouseMoveSensitivity;
            lastMousePosition = coords;
        }

        public void MouseClick(IMouse mouse, MouseButton key, Vector2 coords)
        {
        }
        public void MouseDown(IMouse mouse, MouseButton key)
        {
        }
        public void MouseUp(IMouse mouse, MouseButton key)
        {
        }
        public void MouseScroll(IMouse mouse, ScrollWheel wheel)
        {
        }

        public Vector2 getLastMousePos()
        {
            return lastMousePosition;
        }
        public Vector2 getMouseMoveVector()
        {
            return mouseMoveVector;
        }

        public CameraMove getCameraMove()
        {
            cameraMove = CameraMove.None;

            //Normal movenemnt
            if (keyboard.IsKeyPressed(Key.W))
            {
                cameraMove = cameraMove | CameraMove.Front;
            }
            if (keyboard.IsKeyPressed(Key.S))
            {
                cameraMove = cameraMove | CameraMove.Back;
            }
            if (keyboard.IsKeyPressed(Key.A))
            {
                cameraMove = cameraMove | CameraMove.Left;
            }
            if (keyboard.IsKeyPressed(Key.D))
            {
                cameraMove = cameraMove | CameraMove.Right;
            }

            //Tilt
            if (keyboard.IsKeyPressed(Key.E))
            {
                cameraMove |= CameraMove.TiltRight;
            }
            if (keyboard.IsKeyPressed(Key.Q))
            {
                cameraMove |= CameraMove.TiltLeft;
            }

            //UP AND DOWN
            if (keyboard.IsKeyPressed(Key.C))
            {
                cameraMove = cameraMove | CameraMove.Down;
            }
            if (keyboard.IsKeyPressed(Key.Space))
            {
                cameraMove = cameraMove | CameraMove.Up;
            }

            //Boost
            if (keyboard.IsKeyPressed(Key.ShiftLeft))
            {
                cameraMove = cameraMove | CameraMove.ExtraSpeed;
            }

            return cameraMove;
        }
    }
}
