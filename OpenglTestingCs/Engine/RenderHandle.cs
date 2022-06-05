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

        private BufferObject<float> VBO; //vertex store
        private BufferObject<uint>  EBO; //indices store
        private VertexArrayObject<float, uint> VAO; //Stores VBOS and EBOS

        private Camera _camera;

        private static Shader Shader;

        /////////////////////////////////////////
        //Sample data
        private static readonly float[] Vertices =
        {
            //X    Y      Z     R  G  B  A
             0.5f,  0.5f, 1.0f, 1, 0, 0, 1,
             0.5f, -0.5f, -1.0f, 0, 0, 0, 1,
            -0.5f, -0.5f, 1.0f, 0, 0, 1, 1,
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
        }

        public void OnLoadRender()
        {
            //Sample data prepare
            EBO = new BufferObject<uint>(GL, Indices, BufferTargetARB.ElementArrayBuffer);
            VBO = new BufferObject<float>(GL, Vertices, BufferTargetARB.ArrayBuffer);
            VAO = new VertexArrayObject<float, uint>(GL, VBO, EBO);

            //Telling the VAO object how to lay out the attribute pointers
            VAO.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 7, 0);
            VAO.VertexAttributePointer(1, 4, VertexAttribPointerType.Float, 7, 3);

            Shader = new Shader(GL, "Shaders/shader.vert", "Shaders/shader.frag");

            GL.ClearColor(0.0f, 0.5f, 0.5f, 1.0f);
        }

        public unsafe void Render()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Clear((uint)ClearBufferMask.ColorBufferBit);

            //Binding and using our VAO and shader.
            VAO.Bind();
            Shader.UseShader();

            Matrix4x4 model = Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(10)) * Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(10));
            Matrix4x4 view = Matrix4x4.CreateLookAt(_camera.getCameraPosition(), _camera.getCameraPosition() + _camera.getCameraFront(), _camera.getCameraUp());
            Matrix4x4 projection = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_camera.getCameraZoom()), (float)Program.WIDTH / (float)Program.HEIGHT, 0.1f, 100.0f);

            //Setting a uniform.
            Shader.SetUniform("uBlue", (float)Math.Sin(DateTime.Now.Millisecond / 1000f * Math.PI));
            Shader.SetUniform("uModel", model);
            Shader.SetUniform("uView", view);
            Shader.SetUniform("uProjection", projection);

            GL.DrawElements(PrimitiveType.Triangles, (uint)Indices.Length, DrawElementsType.UnsignedInt, null);
        }

        public void Dispose()
        {
            //Remember to dispose all the instances.
            VBO.Dispose();
            EBO.Dispose();
            VAO.Dispose();
            Shader.Dispose();
        }
    }
}
