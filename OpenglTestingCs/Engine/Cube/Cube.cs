using OpenglTestingCs.Engine.Enums;
using OpenglTestingCs.Engine.RenderObjects;
using Silk.NET.OpenGL;
using System;
using System.Numerics;

namespace OpenglTestingCs.Engine.Cube
{
    public class Cube
    {
        private Vector3 cubeColor;
        private Vector3 cubePos;
        private CubeType cubeType;

        Random rand = new Random();

        Transform cubeTransform;

        public Cube()
        {
            cubeType = CubeType.Green;
            cubePos = new Vector3(0, 0, 0);
            cubeColor = _getColor(cubeType);
            cubeTransform = new Transform();
            cubeTransform.Position = cubePos;
        }

        public Cube(Vector3 pos, CubeType type)
        {
            cubeType = type;
            cubePos = pos;
            cubeColor = _getColor(type);

            cubeTransform = new Transform();
            cubeTransform.Position = cubePos;
        }

        public Cube(Vector3 pos)
        {
            cubeType = (CubeType) rand.Next(4);

            cubePos = pos;
            cubeColor = _getColor(cubeType);

            cubeTransform = new Transform();
            cubeTransform.Position = cubePos;
        }


        public Vector3 GetPos()
        {
            return cubePos;
        }

        public Vector3 GetColor()
        {
            return cubeColor;
        }
        public CubeType GetCubeType()
        {
            return cubeType;
        }

        public Transform GetTransform()
        {
            return cubeTransform;
        }

        public Matrix4x4 GetModelMatrix()
        {
            return cubeTransform.ViewMatrix;
        }

        private Vector3 _getColor(CubeType type)
        {
            switch (type)
            {
                case CubeType.Red: return new Vector3(1.0f, 0.0f, 0.0f);
                case CubeType.Green: return new Vector3(0.0f, 1.0f, 0.0f);
                case CubeType.Blue: return new Vector3(0.0f, 0.0f, 1.0f);
                case CubeType.None: return new Vector3(1.0f, 1.0f, 1.0f);
                    default: return new Vector3(0.0f, 0.0f, 0.0f);
            }
        }

    }
}
