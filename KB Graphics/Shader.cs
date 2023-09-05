using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace KB_Graphics
{
    public class Shader
    {
        public int Handle;
        Dictionary<string,int> _uniformLocations;
        public Shader(string vertexPath, string fragmentPath) {

            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;

            var vertexShaderSource = File.ReadAllText(Path.Combine(projectDirectory, vertexPath));
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            CompileShader(vertexShader);
            
            var fragmentShaderSource = File.ReadAllText(Path.Combine(projectDirectory, fragmentPath));
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            CompileShader(fragmentShader);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            LinkProgram(Handle);

            GL.DetachShader(Handle, vertexShader);
            GL.DeleteShader(vertexShader);

            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            _uniformLocations = new Dictionary<string, int>();

            for (var i = 0; i < numberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(Handle, i, out _, out _);

                var location = GL.GetUniformLocation(Handle, key);

                _uniformLocations.Add(key, location);
            }
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out var status);
            if (status != (int)All.True)
            {
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Failed to compile Shader({shader})\n\n{infoLog}");
            }
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                throw new Exception($"Failed to link Program({program})");
            }
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GettAttributeLocation(string attributeName)
        {
            return GL.GetAttribLocation(Handle, attributeName);
        }

        public void SetInt(string name, int value)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], value);
        }

        public void SetFloat(string name, float value)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], value);
        }

        public void SetMatrix4(string name, Matrix4 value)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref value);
        }

        public void SetVector3(string name, Vector3 value)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocations[name], value);
        }
    }
}
