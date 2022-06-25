using System;

using Silk.NET.OpenGL;
using Silk.NET.Maths;

using OpenglTestingCs.Engine.RenderObjects;
using System.Numerics;
using OpenglTestingCs.Engine.Cube;

namespace OpenglTestingCs.Engine
{
    public class RenderHandle : IDisposable
    {
        private GL GL;

        private CubeMesh _cubeMesh;

        private Camera _camera;

        private static Shader Shader;

        int drawCalls = 0;

        Matrix4x4 view;
        Matrix4x4 projection;

        public RenderHandle(GL GL, Camera camera)
        {
            this.GL = GL;
            _camera = camera;
            _cubeMesh = new CubeMesh(GL);
        }

        public void OnLoadRender()
        {
            Shader = new Shader(GL, "Shaders/shader.vert", "Shaders/shader.frag");

            GL.ClearColor(0.0f, 0.5f, 0.5f, 1.0f);
        }

        public void PreRender()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.DepthMask(true);
            //GL.DepthRange(1.0f, 0.0f);
            GL.CullFace(CullFaceMode.FrontAndBack);
            GL.PolygonMode(GLEnum.FrontAndBack, GLEnum.Line);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            view = _camera.GetViewMatrix();
            projection = _camera.GetProjectionMatrix();

            drawCalls = 0;
        }

        public unsafe void Render()
        {
            PreRender();
            Shader.UseShader();

            Shader.SetUniform("uView", view);
            Shader.SetUniform("uProjection", projection);
            //Shader.SetUniform("uColor", new Vector3(1.0f, 1.0f, 1.0f));

            Shader.SetUniform("uModel", Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(25f)));

            drawCalls += _cubeMesh.RenderCubes(GL, _camera, Shader);
        }

        public int getDrawCalls()
        {
            return drawCalls;
        }

        public void Dispose()
        {
            //Remember to dispose all the instances.
            Shader.Dispose();
        }
        public GL getGL()
        {
            return GL;
        }
    }
}
