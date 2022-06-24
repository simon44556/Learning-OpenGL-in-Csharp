using System;

using Silk.NET.OpenGL;
using Silk.NET.Maths;

using OpenglTestingCs.Engine.RenderObjects;
using System.Numerics;

namespace OpenglTestingCs.Engine
{
    public class RenderHandle : IDisposable
    {
        private GL GL;
        private Cube.Cube cube;

        private BufferObject<float> VBO; //vertex store
        private BufferObject<uint>  EBO; //indices store
        private VertexArrayObject<float, uint> VAO; //Stores VBOS and EBOS

        private Camera _camera;

        Random rand = new Random();

        private static Shader Shader;

        //////////////////////////////////////////
        //Sample data
        private static readonly float[] Vertices =
        {
            //X     Y     Z      R  G  B  A
             0.5f,  0.5f, 1.0f,  1, 0, 0, 1,
             0.5f, -0.5f, -1.0f, 0, 0, 0, 1,
            -0.5f, -0.5f, 1.0f,  0, 0, 1, 1,
            -0.5f,  0.5f, -1.5f, 0, 0, 0, 1
        };

        private static readonly uint[] Indices =
        {
            0, 1, 3,
            1, 2, 3
        };
        //////////////////////////////////////////

        public RenderHandle(GL GL, Camera camera)
        {
            this.GL = GL;
            _camera = camera;
            cube = new Cube.Cube();
        }

        public void OnLoadRender()
        {
            //Sample data prepare
            EBO = new BufferObject<uint>(GL, null, BufferTargetARB.ElementArrayBuffer);
            VBO = new BufferObject<float>(GL, cube.GetVertices(), BufferTargetARB.ArrayBuffer);
            VAO = new VertexArrayObject<float, uint>(GL, VBO, EBO);

            //Telling the VAO object how to lay out the attribute pointers
            VAO.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 3, 0);
            //VAO.VertexAttributePointer(1, 4, VertexAttribPointerType.Float, 7, 3);

            Shader = new Shader(GL, "Shaders/shader.vert", "Shaders/shader.frag");

            GL.ClearColor(0.0f, 0.5f, 0.5f, 1.0f);
        }

        public unsafe void Render()
        {
            
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.PolygonMode(GLEnum.FrontAndBack, GLEnum.Line);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Binding and using our VAO and shader.
            VAO.Bind();
            Shader.UseShader();

            Matrix4x4 view = Matrix4x4.CreateLookAt(_camera.getCameraPosition(), _camera.getCameraPosition() + _camera.getCameraFront(), _camera.getCameraUp());
            Matrix4x4 projection = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_camera.getCameraZoom()), (float)Program.WIDTH / (float)Program.HEIGHT, 0.1f, 100.0f);

            Shader.SetUniform("uView", view);
            Shader.SetUniform("uProjection", projection);
            //Setting a uniform.
            for (int x = 0; x < 32; x++)
                for(int y = 0; y < 32; y++)
                      for(int z = 0; z < 32; z++)
                    {

                        Matrix4x4 model = Matrix4x4.CreateTranslation(new Vector3(x, y, z));

                        //Is model in view
                        float v = Vector3.Dot(_camera.getCameraDirection(), _camera.getCameraPosition() - model.Translation);


                        bool shouldRender = v < 0;
                        if (shouldRender)
                        {
                            //Disco mode
                            //Shader.SetUniform("uColor", new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()));
                            Shader.SetUniform("uColor", new Vector3(1.0f,1.0f,1.0f));
                            Shader.SetUniform("uModel", model);

                            GL.DrawArrays(GLEnum.Triangles, 0, 12 * 3);
                            //GL.DrawElements(PrimitiveType.Triangles, (uint)Indices.Length, DrawElementsType.UnsignedInt, null);
                        }
                    }
        }

        public void Dispose()
        {
            //Remember to dispose all the instances.
            VBO.Dispose();
            EBO.Dispose();
            VAO.Dispose();
            Shader.Dispose();
        }
        public GL getGL()
        {
            return GL;
        }
    }
}
