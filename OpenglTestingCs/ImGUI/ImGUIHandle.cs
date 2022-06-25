using System;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace OpenglTestingCs.ImGUI
{
    public class ImGUIHandle
    {
        private ImGuiController imgui;

        private String coords = "";

        private int WIDTH = 300;
        private int HEIGHT = 120;

        public ImGUIHandle (GL gl, IWindow window, IInputContext input)
        {
            imgui = new ImGuiController(gl, window, input);

        }

        public unsafe void Render(double time, int drawCalls)
        {
            imgui.Update((float)time);

            ImGuiNET.ImGui.SetWindowSize(new Vector2(WIDTH, HEIGHT));

            //ImGuiNET.ImGui.ShowDemoWindow();
            ImGuiNET.ImGui.Text("Position");
            ImGuiNET.ImGui.Text(coords);
            ImGuiNET.ImGui.Text("FrameTime (ms): " + Math.Round(time * 1000, 2));
            ImGuiNET.ImGui.Text("FPS: " + Math.Round(1 / time, 2));
            ImGuiNET.ImGui.Text("Draws: " + drawCalls);


            imgui.Render();
        }

        public void SetCoords(String coords)
        {
            this.coords = coords;
        }
    }
}
