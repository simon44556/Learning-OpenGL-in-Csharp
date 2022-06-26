using System.Numerics;
using System.Collections.Generic;
using Silk.NET.OpenGL;
using System;
using OpenglTestingCs.Engine.RenderObjects;

namespace OpenglTestingCs.Engine.Cube
{
    public class CubeMesh : IDisposable
    {
        private List<Cube> cubeList;

        private VertexArrayObject<float, uint> VAO; //Stores VBOS and EBOS
        private BufferObject<float> VBO; //vertex store
        private BufferObject<uint> EBO; //indices store

        private uint[] indices;
        private float[] vertices;

        private float meshWidth;
        private float meshHeight;
        private float meshLength;

        int CUBE_COUNT = 32;

        private GL _gl;

        Random rand = new Random();
        public CubeMesh(GL gl, Vector3 meshDimensions)
        {
            meshWidth = meshDimensions.X;
            meshHeight = meshDimensions.Y;
            meshLength = meshDimensions.Z;

            _gl = gl;
            //1. create indices, verts
            genIndicesAndVertices();
            //2. Send them to gpu
            CreateMesh();
            //3. Store positions and colors
            generateCubeList();
        }

        public void CreateMesh()
        {
            //Sample data prepare
            EBO = new BufferObject<uint>(_gl, indices, BufferTargetARB.ElementArrayBuffer);
            VBO = new BufferObject<float>(_gl, vertices, BufferTargetARB.ArrayBuffer);
            VAO = new VertexArrayObject<float, uint>(_gl, VBO, EBO);

            //Telling the VAO object how to lay out the attribute pointers
            //layout 0
            VAO.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 3, 0); //vec3


            /* Binding additional data
             * colorVBO = new BufferObject<float>(GL, vec3ToArray(cubeColor), BufferTargetARB.ArrayBuffer);
            VAO.BindAdditionalVBO(colorVBO);
            //layout 1
            VAO.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, 3, 0);
            */
        }

        private void genIndicesAndVertices()
        {
            indices = new uint[]
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

            vertices = new float[] {
                meshWidth    , meshLength    ,  meshHeight*2 ,  //0
                meshWidth*2  , meshLength    ,  meshHeight*2 ,  //1
                meshWidth    ,  meshLength*2 ,  meshHeight*2 ,  //2
                meshWidth*2  ,  meshLength*2 ,  meshHeight*2 ,  //3
                meshWidth    , meshLength    , meshHeight    ,  //4
                meshWidth*2  , meshLength    , meshHeight    ,  //5
                meshWidth    ,  meshLength*2 , meshHeight    ,  //6
                meshWidth*2  ,  meshLength*2 , meshHeight   //7
            };
        }

        public void generateCubeList()
        {
            cubeList = new List<Cube>();
            for (float x = 0; x < CUBE_COUNT; x+=1)
                for (float y = 0; y < CUBE_COUNT; y+=1)
                    for (float z = 0; z < CUBE_COUNT; z+=1)
                    {
                        addMesh(new Vector3(x, y, z));
                    }
        }
        public int RenderCubes(Camera _camera, Shader _shader)
        {
            int count = 0;
            for (int i = 0; i < cubeList.Count; i++)
            {
                if (cubeList[i].GetCubeType() == 0)
                {
                    continue;
                }
                count += RenderMesh(_camera, _shader, cubeList[i].GetModelMatrix(), cubeList[i].GetColor());
            }
            return count;
        }

        // HELPERS
        public unsafe int RenderMesh(Camera _camera, Shader shader, Matrix4x4 model, Vector3 color)
        {
            int DrawCalls = 0;

            //Binding and using our VAO and shader.
            VAO.Bind();
            //Setting a uniform.

            //Is model in view
            float angleBetweenCubeAndCamera = Vector3.Dot(_camera.getCameraDirection(), (_camera.getCameraPosition() - new Vector3(0.5f)/*Coordinate system adjustment*/) - model.Translation);

            bool shouldRender = angleBetweenCubeAndCamera < 0;
            if (shouldRender)
            {
                //Disco mode
                //shader.SetUniform("uColor", new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()));

                shader.SetUniform("uColor", color);
                shader.SetUniform("uModel", model);

                _gl.DrawElements(PrimitiveType.Triangles, (uint)indices.Length, DrawElementsType.UnsignedInt, null);
                DrawCalls++;
            }

            return DrawCalls;
        }

        public void addMesh(Vector3 coords)
        {
            Cube c = new Cube(coords);
            cubeList.Add(c);
        }

        public void Dispose()
        {
            cubeList.Clear();
            //Remember to dispose all the instances.
            VBO.Dispose();
            EBO.Dispose();
            VAO.Dispose();
         }
    }
}
