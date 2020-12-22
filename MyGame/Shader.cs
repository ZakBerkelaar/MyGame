using System.Collections.Generic;
using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace MyGame
{
    //https://github.com/opentk/LearnOpenTK/blob/master/Common/Shader.cs
    public class Shader
    {
        public int Handle;

        private readonly Dictionary<string, int> uniformLocations;

        public Shader(string vertPath, string fragPath)
        {
            //Load vertex shader
            string source = LoadToText(vertPath);
            //Create empty shader of type vertex shader
            int vertShader = GL.CreateShader(ShaderType.VertexShader);
            //Bind source to shader
            GL.ShaderSource(vertShader, source);
            //Compile the shader
            CompileShader(vertShader);

            //Do the same for fragment
            source = LoadToText(fragPath);
            int fragShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragShader, source);
            CompileShader(fragShader);


            //Create shader program
            Handle = GL.CreateProgram();
            //Attach shaders to program
            GL.AttachShader(Handle, vertShader);
            GL.AttachShader(Handle, fragShader);
            //Link the program together
            LinkProgram(Handle);

            //Detach and delete unused shaders
            GL.DetachShader(Handle, vertShader);
            GL.DetachShader(Handle, fragShader);
            GL.DeleteShader(fragShader);
            GL.DeleteShader(vertShader);

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int uniformNum);

            uniformLocations = new Dictionary<string, int>();

            for (int i = 0; i < uniformNum; i++)
            {
                string name = GL.GetActiveUniform(Handle, i, out _, out _);

                int location = GL.GetUniformLocation(Handle, name);

                uniformLocations.Add(name, location);
            }
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        private void LinkProgram(int program)
        {
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int code);
            if(code != (int)All.True)
            {
                string error = GL.GetProgramInfoLog(program);
                Logger.LogError("Error occurred while linking program: " + error);
            }
        }

        private void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out int code);
            if(code != (int)All.True)
            {
                string error = GL.GetShaderInfoLog(shader);
                Logger.LogError("Error occurred while compiling shader: " + error);
            }
        }

        private static string LoadToText(string path)
        {
            return File.ReadAllText(path);
        }

        public void SetVector2(string name, Vector2 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform2(uniformLocations[name], data.x, data.y);
        }

        public void SetVector4(string name, Vector4 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform4(uniformLocations[name], data);
        }

        public void SetInt(string name, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(uniformLocations[name], data);
        }

        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(uniformLocations[name], data);
        }

        public void SetMat4(string name, ref Matrix4 mat)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(uniformLocations[name], true, ref mat);
        }
    }
}
