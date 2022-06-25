using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Silk.NET.OpenGL;

namespace OpenglTestingCs.Engine
{
    public class Shader : IDisposable
    {
        private uint _handle;
        private GL _gl;

        private Dictionary<ShaderType, uint> shaderList = new Dictionary<ShaderType, uint>();

        public Shader(GL gl, string vertexPath, string fragmentPath)
        {
            _gl = gl;

            HandleShaderCreate(vertexPath, fragmentPath);
            CompileProgram();
        }

        public Shader(GL gl, string vertexPath, string geometryPath, string fragmentPath)
        {
            _gl = gl;

            HandleShaderCreate(vertexPath, geometryPath, fragmentPath);
            CompileProgram();
        }

        public Shader(GL gl, string vertexPath, string tesselationPath, string tessEvaluationPath, string geometryPath, string fragmentPath)
        {
            _gl = gl;

            HandleShaderCreate(vertexPath, tesselationPath, tessEvaluationPath, geometryPath, fragmentPath);
            CompileProgram();
        }
        private void CompileProgram()
        {
            if(shaderList.Count < 1)
            {
                Console.WriteLine("No shaders added");
                return;
            }
            //Create the shader program.
            _handle = _gl.CreateProgram();
            //Attach the individual shaders.
            foreach (KeyValuePair<ShaderType, uint> kvp in shaderList)
            {
                _gl.AttachShader(_handle, kvp.Value);
            }
            _gl.LinkProgram(_handle);
            //Check for linking errors.
            _gl.GetProgram(_handle, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                throw new Exception($"Program failed to link with error: {_gl.GetProgramInfoLog(_handle)}");
            }
            //Detach and delete the shaders
            foreach (KeyValuePair<ShaderType, uint> kvp in shaderList)
            {
                _gl.DetachShader(_handle, kvp.Value);
                _gl.DeleteShader(kvp.Value);
            }

            shaderList.Clear();
        }

        public void HandleShaderCreate(string vertexPath, string fragmentPath)
        {
            string vertexCode = ReadFile(vertexPath);
            string fragmentCode = ReadFile(fragmentPath);

            //Load the individual shaders.
            CompileShader(vertexCode, ShaderType.VertexShader);
            CompileShader(fragmentCode, ShaderType.FragmentShader);
        }
        public void HandleShaderCreate(string vertexPath, string geometryPath, string fragmentPath)
        {
            HandleShaderCreate(vertexPath, fragmentPath);

            string geometryCode = ReadFile(geometryPath);

            //Load the individual shaders.
            CompileShader(geometryCode, ShaderType.GeometryShader);
        }
        public void HandleShaderCreate(string vertexPath, string tesselationPath, string tessEvaluationPath, string geometryPath, string fragmentPath)
        {
            HandleShaderCreate(vertexPath, geometryPath, fragmentPath);

            string tesselationCode = ReadFile(tesselationPath);
            string tesselationEvCode = ReadFile(tessEvaluationPath);

            //Load the individual shaders.
            CompileShader(tesselationCode, ShaderType.TessControlShader);
            CompileShader(tesselationEvCode, ShaderType.TessEvaluationShader);
        }


        private uint CompileShader(string shaderCode, ShaderType type)
        {
            //To load a single shader we need to:
            //1) Load the shader from a file.
            //2) Create the handle.
            //3) Upload the source to opengl.
            //4) Compile the shader.
            //5) Check for errors.
            
            if (shaderCode == "")
            {
                Console.WriteLine($"Failed to compile shader {type} code is empty");
                return 0;
            }

            string src = shaderCode;
            uint handle = _gl.CreateShader(type);
            _gl.ShaderSource(handle, src);
            _gl.CompileShader(handle);
            string infoLog = _gl.GetShaderInfoLog(handle);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
            }

            shaderList.Add(type, handle);

            Console.WriteLine($"Adding shader {type}");

            return handle;
        }

        private string ReadFile(string location)
        {
            string code = "";
            try
            {
                code = File.ReadAllText(location);
            } catch (Exception e) { Console.WriteLine(e.StackTrace); }

            return code;
        }

        public void UseShader()
        {
            _gl.UseProgram(_handle);
        }

        //Uniforms are properties that applies to the entire geometry
        public void SetUniform(string name, int value)
        {
            //Setting a uniform on a shader using a name.
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1) //If GetUniformLocation returns -1 the uniform is not found.
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            _gl.Uniform1(location, value);
        }

        public void SetUniform(string name, float value)
        {
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            _gl.Uniform1(location, value);
        }

        public void SetUniform(string name, Vector3 value)
        {
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            _gl.Uniform3(location, value);
        }

        public unsafe void SetUniform(string name, Matrix4x4 value)
        {
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            UseShader();
            _gl.UniformMatrix4(location, 1, false, (float*) &value);
        }


        public void Dispose()
        {
            //Remember to delete the program when we are done.
            _gl.DeleteProgram(_handle);
        }

    }
}
