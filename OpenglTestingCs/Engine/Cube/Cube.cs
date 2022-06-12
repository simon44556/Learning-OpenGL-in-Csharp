using System;
using System.Numerics;

namespace OpenglTestingCs.Engine.Cube
{
    public class Cube
    {
        private float cubeLength;
        private float cubeHeight;
        private float cubeWidth;

        private Vector3 cubePos;

        public Cube()
        {
            cubeHeight = 1.0f;
            cubeLength = 1.0f;
            cubeWidth = 1.0f;

            cubePos = new Vector3(0,0,0);
        }

    }
}
