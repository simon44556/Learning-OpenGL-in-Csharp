using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

using System;
using System.Numerics;

namespace OpenglTestingCs.Engine
{
    public class InputHandler
    {
        private IWindow window;

        private Vector2 lastMousePosition = Vector2.Zero;

        private Vector2 mouseMoveVector = Vector2.Zero;

        public InputHandler(IWindow window)
        {
            this.window = window;

            IInputContext input = window.CreateInput();

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
            float sensitivity = 0.1f;

            mouseMoveVector = (coords - lastMousePosition) * sensitivity;
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
    }
}
