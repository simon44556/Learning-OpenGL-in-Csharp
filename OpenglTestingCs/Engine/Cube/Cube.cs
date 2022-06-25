using OpenglTestingCs.Engine.Enums;
using System.Numerics;

namespace OpenglTestingCs.Engine.Cube
{
    public class Cube
    {
        private Vector3 cubeColor;
        private Vector3 cubePos;
        private CubeType type;

        public Cube()
        {
            type = CubeType.Green;
            cubePos = new Vector3(0, 0, 0);
            cubeColor = _getColor(type);
        }

        public Cube(Vector3 pos, CubeType type)
        {
            this.type = type;
            cubePos = pos;
            cubeColor = _getColor(type);
        }

        public Vector3 GetPos()
        {
            return cubePos;
        }

        public Vector3 GetColor()
        {
            return cubeColor;
        }
        public CubeType GetType()
        {
            return type;
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
