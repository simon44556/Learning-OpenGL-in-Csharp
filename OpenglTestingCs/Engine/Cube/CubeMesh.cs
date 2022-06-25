using System.Numerics;
using System.Collections.Generic;

namespace OpenglTestingCs.Engine.Cube
{
    public class CubeMesh : Cube
    {
        private List<Cube> cubeList;

        private float meshLength;
        private float meshHeight;
        private float meshWidth;

        public CubeMesh()
        {
            meshHeight = 1.0f;
            meshLength = 1.0f;
            meshWidth = 1.0f;
        }

        public CubeMesh(Vector3 dimensions)
        {
            meshHeight = dimensions.X;
            meshLength = dimensions.Y;
            meshWidth = dimensions.Z;
        }
        
        //Get model pos
        public Matrix4x4 getTranslation(int index)
        {
            if(index > cubeList.Count)
            {
                index = cubeList.Count - 1;
            }

            return Matrix4x4.CreateTranslation(cubeList[index].GetPos());
        }

        public uint[] GetIndices()
        {
            return new uint[]
            {
                //Top
                2, 6, 7,
                2, 3, 7,

                //Bottom
                0, 4, 5,
                0, 1, 5,

                //Left
                0, 2, 6,
                0, 4, 6,

                //Right
                1, 3, 7,
                1, 5, 7,

                //Front
                0, 2, 3,
                0, 1, 3,

                //Back
                4, 6, 7,
                4, 5, 7
            };
        }

        public float[] GetVertices()
        {
            return new float[] {
                -meshWidth, -meshHeight,  meshLength, //0
                 meshWidth, -meshHeight,  meshLength, //1
                -meshWidth,  meshHeight,  meshLength, //2
                 meshWidth,  meshHeight,  meshLength, //3
                -meshWidth, -meshHeight, -meshLength, //4
                 meshWidth, -meshHeight, -meshLength, //5
                -meshWidth,  meshHeight, -meshLength, //6
                 meshWidth,  meshHeight, -meshLength  //7
            };
        }
    }
}
