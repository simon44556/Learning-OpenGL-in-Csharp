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

        int drawCalls = 0;

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
            EBO = new BufferObject<uint>(GL, cube.GetIndices(), BufferTargetARB.ElementArrayBuffer);
            VBO = new BufferObject<float>(GL, cube.GetVertices(), BufferTargetARB.ArrayBuffer);
            VAO = new VertexArrayObject<float, uint>(GL, VBO, EBO);

            //Telling the VAO object how to lay out the attribute pointers
            //layout 0
            VAO.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 0, 0); //vec3
           
            //layout 1
            //VAO.VertexAttributePointer(1, 4, VertexAttribPointerType.Float, 7, 3); //vec4

            Shader = new Shader(GL, "Shaders/shader.vert", "Shaders/shader.frag");

            GL.ClearColor(0.0f, 0.5f, 0.5f, 1.0f);
        }

        public unsafe void Render()
        {
            drawCalls = 0;

            ReadOnlySpan<uint> indices = cube.GetIndices().AsSpan<uint>();

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
            Shader.SetUniform("uColor", new Vector3(1.0f, 1.0f, 1.0f));

            //Setting a uniform.
            int CUBE_COUNT = 5;
            for (int x = 0; x < CUBE_COUNT; x++)
                for(int y = 0; y < CUBE_COUNT; y++)
                      for(int z = 0; z < CUBE_COUNT; z++)
                    {

                        Matrix4x4 model = Matrix4x4.CreateTranslation(new Vector3(x, y, z));

                        //Is model in view
                        float v = Vector3.Dot(_camera.getCameraDirection(), _camera.getCameraPosition() - model.Translation);


                        bool shouldRender = v < 0;
                        if (shouldRender)
                        {
                            //Disco mode
                            //Shader.SetUniform("uColor", new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()));
                            Shader.SetUniform("uModel", model);


                            drawCalls++;
                            GL.DrawElements(PrimitiveType.Triangles, (uint)indices.Length, DrawElementsType.UnsignedInt, null);
                        }
                }
        }

        public int getDrawCalls()
        {
            return drawCalls;
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
