using System;
using System.Numerics;
using OpenglTestingCs.Engine.Enums;
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
        private float CameraZoom = 90f;

        private float moveSpeed;
        private float boostSpeed;

        public Camera(InputHandler inputHandler)
        {
            this.moveSpeed = 2.5f;
            this.boostSpeed = 5.0f;
            _inputHandler = inputHandler;
        }

        public Camera(InputHandler inputHandler, float moveSpeed, float boostSpeed)
        {
            this.moveSpeed = moveSpeed;
            this.boostSpeed = boostSpeed;
            _inputHandler = inputHandler;
        }

        public void OnUpdate(double deltaTime, CameraMove moveDirection)
        {
            updateCameraPosition(deltaTime, moveDirection);
            updateCameraRotation();
        }

        public Vector3 getCameraPosition () { return CameraPosition; }
        public Vector3 getCameraFront() { return CameraFront; }
        public Vector3 getCameraUp() { return CameraUp; }
        public Vector3 getCameraDirection() { return CameraDirection; }
        public float getCameraZoom() { return CameraZoom; }

        private void updateCameraPosition(double deltaTime, CameraMove moveDirection)
        {
            float currenctSpeed;
            //Boost
            if (moveDirection.HasFlag(CameraMove.ExtraSpeed)){
                currenctSpeed = (moveSpeed+boostSpeed) * (float)deltaTime;
            } else
            {
                currenctSpeed = moveSpeed * (float)deltaTime;
            }

            //Normal movement
            if (moveDirection.HasFlag( CameraMove.Front ))
            {
                CameraPosition += currenctSpeed * CameraFront;
            }
            if (moveDirection.HasFlag( CameraMove.Back ))
            {
                CameraPosition -= currenctSpeed * CameraFront;
            }
            if (moveDirection.HasFlag( CameraMove.Left ))
            {
                //Normalized cross product between up and front should be 90 degree angle vector
                CameraPosition -= currenctSpeed * Vector3.Normalize(Vector3.Cross(CameraFront, CameraUp));
            }
            if (moveDirection.HasFlag( CameraMove.Right ))
            {
                CameraPosition += currenctSpeed  * Vector3.Normalize(Vector3.Cross(CameraFront, CameraUp));
            }

            //Up and down
            if (moveDirection.HasFlag(CameraMove.Up))
            {
                CameraPosition += currenctSpeed * CameraUp;
            }
            if (moveDirection.HasFlag(CameraMove.Down))
            {
                CameraPosition -= currenctSpeed * CameraUp;
            }

            //Tilt control
            /*if (moveDirection.HasFlag(CameraMove.TiltRight))
            {
                Console.WriteLine(CameraUp);

                CameraUp = new Vector3(CameraUp.X + (0.1f*currenctSpeed), CameraUp.Y, CameraUp.Z);
            }
            if (moveDirection.HasFlag(CameraMove.TiltLeft))
            {
                CameraUp = new Vector3(CameraUp.X - (0.1f*currenctSpeed), CameraUp.Y, CameraUp.Z);
            }*/
        }

        private void updateCameraRotation()
        {
            //TODO: Check if we need delta time
            Vector2 moveVec2 = _inputHandler.getMouseMoveVector();
            
            CameraYaw += moveVec2.X;
            CameraPitch -= moveVec2.Y;

            CameraPitch = Math.Clamp(CameraPitch, -89.0f, 89.0f);

            CameraDirection.X = MathF.Cos(MathHelper.DegreesToRadians(CameraYaw)) * MathF.Cos(MathHelper.DegreesToRadians(CameraPitch));
            CameraDirection.Y = MathF.Sin(MathHelper.DegreesToRadians(CameraPitch));
            CameraDirection.Z = MathF.Sin(MathHelper.DegreesToRadians(CameraYaw)) * MathF.Cos(MathHelper.DegreesToRadians(CameraPitch));
            CameraFront = Vector3.Normalize(CameraDirection);

        }
        public Matrix4x4 GetViewMatrix()
        {
            return Matrix4x4.CreateLookAt(CameraPosition, CameraPosition + CameraFront, CameraUp);
        }

        public Matrix4x4 GetProjectionMatrix()
        {
            return Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(CameraZoom), (float)Program.WIDTH / (float)Program.HEIGHT, 0.1f, 100.0f);
        }
    }
}
