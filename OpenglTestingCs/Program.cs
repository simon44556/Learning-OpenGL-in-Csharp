using OpenglTestingCs.Engine;
using Silk.NET.OpenGL;
using Silk.NET.Maths;
using Silk.NET.Windowing;

using System;
using OpenglTestingCs.ImGUI;

namespace OpenglTestingCs
{
    /*
     TODO:
    - Implmement camera
    - Start experimenting with point data and geometry shader
     */
    public static class Program
    {
        private static IWindow window;
        private static InputHandler inputHandler;
        private static RenderHandle renderHandle;
        private static Camera camera;
        private static ImGUIHandle imGui;

        public static int WIDTH = 1024;
        public static int HEIGHT = 768;
        private static double FPS = 75.0d;
        private static double UPS = 120.0d;
        private static string TITLE = "Testing";
        private static bool VSYNC = false;

        static void Main(string[] args)
        {
            WindowOptions options = WindowOptions.Default;
            options.Size = new Vector2D<int>(WIDTH, HEIGHT);
            options.Title = TITLE;
            options.UpdatesPerSecond = UPS;
            options.FramesPerSecond = FPS;
            options.VSync = VSYNC;
            options.ShouldSwapAutomatically = false;

            //Thanks AMD
            options.PreferredDepthBufferBits = 24;  // 16 should be supported everywhere  https://github.com/dotnet/Silk.NET/issues/927
            options.PreferredStencilBufferBits = 8; // Based on google this should be 8

            window = Window.Create(options);
            
            window.Load += OnLoad;
            window.Render += OnRender;
            window.Update += OnUpdate;
            window.Closing += OnClose;
            window.FramebufferResize += OnFbResize;
            window.Resize += OnResize;

            window.Run();
        }

        private static void OnLoad()
        {
            inputHandler = new InputHandler(window);
            camera = new Camera(inputHandler);
            renderHandle = new RenderHandle(GL.GetApi(window), camera);
            imGui = new ImGUIHandle(renderHandle.getGL(), window, inputHandler.getInput(), camera);

            renderHandle.OnLoadRender();
        }

        private static void OnRender(double time)
        {
            renderHandle.Render();
            imGui.Render(time, renderHandle.getDrawCalls());


            window.SwapBuffers();
        }
        private static void OnUpdate(double time)
        {
            camera.OnUpdate(time, inputHandler.getCameraMove());
            inputHandler.onUpdate();
            imGui.onUpdate( time );
            
        }
        private static void OnClose() {
            renderHandle.Dispose();
        }
        private static void OnFbResize(Vector2D<int> size)
        {

        }
        private static void OnResize(Vector2D<int> size) {
        
        }
    }
}
