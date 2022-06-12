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
        private int HEIGHT = 70;

        public ImGUIHandle (GL gl, IWindow window, IInputContext input)
        {
            imgui = new ImGuiController(gl, window, input);

        }

        public unsafe void Render(double time)
        {
            imgui.Update((float)time);

            ImGuiNET.ImGui.SetWindowSize(new Vector2(WIDTH, HEIGHT));

            //ImGuiNET.ImGui.ShowDemoWindow();
            ImGuiNET.ImGui.Text("Position");
            ImGuiNET.ImGui.Text(coords);


            imgui.Render();
        }

        public void SetCoords(String coords)
        {
            this.coords = coords;
        }
    }
}
