using System;
using System.Numerics;
using Silk.NET.OpenGL;

namespace OpenglTestingCs.Engine
{
    public class Camera
    {
        private InputHandler _inputHandler;

        private Vector3 CameraPosition = new Vector3(0.0f, 0.0f, 5.0f);
        private Vector3 CameraFront = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 CameraUp = Vector3.UnitY;
        private Vector3 CameraDirection = Vector3.Zero;

        private float CameraYaw = -90f;
        private float CameraPitch = 0f;
        private float CameraZoom = 45f;

        public Camera(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        public void OnUpdate()
        {
            Vector2 moveVec2 = _inputHandler.getMouseMoveVector();

            CameraYaw += moveVec2.X;
            CameraPitch -= moveVec2.Y;

            CameraPitch = Math.Clamp(CameraPitch, -89.0f, 89.0f);

            CameraDirection.X = MathF.Cos(MathHelper.DegreesToRadians(CameraYaw)) * MathF.Cos(MathHelper.DegreesToRadians(CameraPitch));
            CameraDirection.Y = MathF.Sin(MathHelper.DegreesToRadians(CameraPitch));
            CameraDirection.Z = MathF.Sin(MathHelper.DegreesToRadians(CameraYaw)) * MathF.Cos(MathHelper.DegreesToRadians(CameraPitch));
            CameraFront = Vector3.Normalize(CameraDirection);
        }


    }
}
