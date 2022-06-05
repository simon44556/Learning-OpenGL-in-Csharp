using System;

using Silk.NET.OpenGL;
using Silk.NET.Maths;

using OpenglTestingCs.Engine.RenderObjects;

namespace OpenglTestingCs.Engine
{
    public class RenderHandle : IDisposable
    {
        private GL GL;

        private BufferObject<float> VBO; //vertex store
        private BufferObject<uint>  EBO; //indices store
        private VertexArrayObject<float, uint> VAO; //Stores VBOS and EBOS

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

        public RenderHandle(GL GL)
        {
            this.GL = GL;
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
            GL.Clear((uint)ClearBufferMask.ColorBufferBit);

            //Binding and using our VAO and shader.
            VAO.Bind();
            Shader.UseShader();
            //Setting a uniform.

            /*
             glm::mat4 Camera::calculateViewMatrix() {
	            return glm::lookAt(position, position + front, up);
            }
            var model = Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(difference)) * Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(difference));
            var view = Matrix4x4.CreateLookAt(CameraPosition, CameraPosition + CameraFront, CameraUp);
            var projection = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(CameraZoom), Width / Height, 0.1f, 100.0f);
             */
            //Matrix4X4 model = Matrix4X4.CreateLookAt<>

            Shader.SetUniform1("uBlue", (float)Math.Sin(DateTime.Now.Millisecond / 1000f * Math.PI));

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
