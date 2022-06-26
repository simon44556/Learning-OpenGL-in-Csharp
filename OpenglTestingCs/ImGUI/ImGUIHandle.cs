using System;
using System.Numerics;
using OpenglTestingCs.Engine;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace OpenglTestingCs.ImGUI
{
    public class ImGUIHandle
    {
        private ImGuiController imgui;

        private double updateTime = 0;

        private Camera _camera;

        private int WIDTH = 300;
        private int HEIGHT = 500;

        public ImGUIHandle (GL gl, IWindow window, IInputContext input, Camera c)
        {
            imgui = new ImGuiController(gl, window, input);
            _camera = c;

        }

        public unsafe void Render(double time, int drawCalls)
        {
            imgui.Update((float)time);

            ImGuiNET.ImGui.SetWindowSize(new Vector2(WIDTH, HEIGHT));

            //ImGuiNET.ImGui.ShowDemoWindow();
            ImGuiNET.ImGui.Text("Position");
            ImGuiNET.ImGui.Text(_camera.getCameraPosition().ToString());
            ImGuiNET.ImGui.Text("Dir");
            ImGuiNET.ImGui.Text(_camera.getCameraDirection().ToString());
            ImGuiNET.ImGui.Text("Front");
            ImGuiNET.ImGui.Text(_camera.getCameraFront().ToString());
            ImGuiNET.ImGui.Text("UP");
            ImGuiNET.ImGui.Text(_camera.getCameraUp().ToString());
            ImGuiNET.ImGui.Text("Proj");
            ImGuiNET.ImGui.Text(_camera.GetProjectionMatrix().GetDeterminant().ToString());
            ImGuiNET.ImGui.Text("View");
            ImGuiNET.ImGui.Text(_camera.GetViewMatrix().GetDeterminant().ToString());
            ImGuiNET.ImGui.Text("FrameTime (ms): " + Math.Round(time * 1000, 2));
            ImGuiNET.ImGui.Text("FPS: " + Math.Round(1 / time, 2));
            ImGuiNET.ImGui.Text("UPS: " + Math.Round(1 / updateTime, 2));
            ImGuiNET.ImGui.Text("Cubes: " + drawCalls);


            imgui.Render();
        }

        public void onUpdate(double time)
        {
            updateTime = time;
        }
    }
}
