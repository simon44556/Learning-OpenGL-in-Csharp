using OpenglTestingCs.Engine.Enums;
using OpenglTestingCs.Engine.RenderObjects;
using Silk.NET.OpenGL;
using System;
using System.Numerics;

namespace OpenglTestingCs.Engine.Cube
{
    public class Cube : IDisposable
    {
        private Vector3 cubeColor;
        private Vector3 cubePos;
        private CubeType type;

        private uint[] indices;
        private float[] vertices;

        private float meshLength;
        private float meshHeight;
        private float meshWidth;

        private VertexArrayObject<float, uint> VAO; //Stores VBOS and EBOS
        private BufferObject<float> VBO; //vertex store
        private BufferObject<uint> EBO; //indices store

        public Cube()
        {
            meshHeight = 1;
            meshWidth = 1;
            meshLength = 1;

            type = CubeType.Green;
            cubePos = new Vector3(0, 0, 0);
            cubeColor = _getColor(type);
            genIndicesAndVertices();
        }

        public Cube(Vector3 pos, CubeType type, Vector3 dimensions)
        {
            meshHeight = dimensions.X;
            meshWidth = dimensions.Y;
            meshLength = dimensions.Z;

            this.type = type;
            cubePos = pos;
            cubeColor = _getColor(type);
            genIndicesAndVertices();
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
                -meshWidth + cubePos.X, -meshHeight + cubePos.Y,  meshLength + cubePos.Z, 1,0,0, //0
                 meshWidth + cubePos.X, -meshHeight + cubePos.Y,  meshLength + cubePos.Z, 1,0,0, //1
                -meshWidth + cubePos.X,  meshHeight + cubePos.Y,  meshLength + cubePos.Z, 0,1,0, //2
                 meshWidth + cubePos.X,  meshHeight + cubePos.Y,  meshLength + cubePos.Z, 0,1,0,//3
                -meshWidth + cubePos.X, -meshHeight + cubePos.Y, -meshLength + cubePos.Z, 0,0,1,//4
                 meshWidth + cubePos.X, -meshHeight + cubePos.Y, -meshLength + cubePos.Z, 0,0,1,//5
                -meshWidth + cubePos.X,  meshHeight + cubePos.Y, -meshLength + cubePos.Z, 1,0,1,//6
                 meshWidth + cubePos.X,  meshHeight + cubePos.Y, -meshLength + cubePos.Z, 1,0,1//7
            };
        }


        public void CreateMesh(GL GL)
        {
            //Sample data prepare
            EBO = new BufferObject<uint>(GL, indices, BufferTargetARB.ElementArrayBuffer);
            VBO = new BufferObject<float>(GL, vertices, BufferTargetARB.ArrayBuffer);
            VAO = new VertexArrayObject<float, uint>(GL, VBO, EBO);

            //Telling the VAO object how to lay out the attribute pointers
            //layout 0
            VAO.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 6, 0); //vec3

            //layout 1
            VAO.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, 6, 3); //vec4
        }

        public unsafe int RenderMesh(GL GL, Camera _camera, Shader shader)
        {
            int DrawCalls = 0;

            //Binding and using our VAO and shader.
            VAO.Bind();
            //Setting a uniform.

            //Matrix4x4 model = Matrix4x4.CreateTranslation(cubePos);
            //Is model in view
            float v = Vector3.Dot(_camera.getCameraDirection(), _camera.getCameraPosition() - cubePos);

            bool shouldRender = v < 0;
            if (shouldRender)
            {
                //Disco mode
                //shader.SetUniform("uColor", new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()));

                //shader.SetUniform("uColor", cubeColor);
                //shader.SetUniform("uModel", model);

                GL.DrawElements(PrimitiveType.Triangles, (uint)indices.Length, DrawElementsType.UnsignedInt, null);
                DrawCalls++;
            }
            
            return DrawCalls;
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

        public void Dispose()
        {
            //Remember to dispose all the instances.
            VBO.Dispose();
            EBO.Dispose();
            VAO.Dispose();
        }
    }
}
