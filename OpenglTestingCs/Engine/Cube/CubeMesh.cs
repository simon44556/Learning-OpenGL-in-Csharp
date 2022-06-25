using System.Numerics;
using System.Collections.Generic;
using Silk.NET.OpenGL;
using System;

namespace OpenglTestingCs.Engine.Cube
{
    public class CubeMesh : Cube, IDisposable
    {
        private List<Cube> cubeList;

        int CUBE_COUNT = 1;

        Random rand = new Random();
        public CubeMesh(GL gl)
        {
            generateCubeList(gl);
        }

        public CubeMesh(GL gl, Vector3 dimensions)
        {
            generateCubeList(gl);
        }

        public void generateCubeList(GL gl)
        {
            cubeList = new List<Cube>();
            for (int x = 0; x < CUBE_COUNT; x++)
                for (int y = 0; y < CUBE_COUNT; y++)
                    for (int z = 0; z < CUBE_COUNT; z++)
                    {
                        Cube c = new Cube(new Vector3(x, y, z), Enums.CubeType.Green, new Vector3(1,1,1));
                        c.CreateMesh(gl);
                        cubeList.Add(c);
                    }
        }

        public int RenderCubes(GL gl, Camera _camera, Shader _shader)
        {
            int count = 0;
            for(int i = 0; i < cubeList.Count; i++)
            {
                count += cubeList[i].RenderMesh(gl, _camera, _shader);
            }
            return count;
        }



        //Get model pos
        public Matrix4x4 getTranslation(int index)
        {
            if (index > cubeList.Count)
            {
                index = cubeList.Count - 1;
            }

            return Matrix4x4.CreateTranslation(cubeList[index].GetPos());
        }

        public void Dispose()
        {
            cubeList.Clear();
            //Remember to dispose all the instances.
            /*VBO.Dispose();
            EBO.Dispose();
            VAO.Dispose();*/
        }
    }
}
