using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp21;
internal class Shader : IDisposable
{
    public readonly int program;
    private readonly int vertexShader, fragmentShader;

    public Shader(string vertexShaderPath, string fragmentShaderPath)
    {
        vertexShader = CompileShader(vertexShaderPath, ShaderType.VertexShader);
        fragmentShader = CompileShader(fragmentShaderPath, ShaderType.FragmentShader);

        program = GL.CreateProgram();
        GL.AttachShader(program, vertexShader);
        GL.AttachShader(program, fragmentShader);
        GL.LinkProgram(program);

        CheckProgramErrors(program);
    }

    private int CompileShader(string path, ShaderType type)
    {
        string source = File.ReadAllText(path);

        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        CheckShaderErrors(shader);

        return shader;
    }

    private void CheckShaderErrors(int shader)
    {
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
        if (success is 0) 
        {
            GL.GetShaderInfoLog(shader, out string info);
            throw new Exception(info);
        }
    }

    private void CheckProgramErrors(int shader)
    {
        GL.GetProgram(shader, GetProgramParameterName.LinkStatus, out int success);
        if (success is 0)
        {
            GL.GetProgramInfoLog(shader, out string info);
            throw new Exception(info);
        }
    }


    public void Dispose()
    {
        GL.DeleteProgram(program);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public void Use()
    {
        GL.UseProgram(program);
    }

    public void SetBool(string name, bool value)
    {
        var location = GL.GetUniformLocation(program, name);
        GL.Uniform1(location, value ? 1 : 0);
    }

    public void SetInt(string name, int value)
    {
        var location = GL.GetUniformLocation(program, name);
        GL.Uniform1(location, value);
    }

    public void SetFloat(string name, float value)
    {
        var location = GL.GetUniformLocation(program, name);
        GL.Uniform1(location, value);
    }

    public void SetVector(string name, Vector2 value)
    {
        var location = GL.GetUniformLocation(program, name);
        GL.Uniform2(location, value);
    }

    public void SetVector(string name, Vector3 value)
    {
        var location = GL.GetUniformLocation(program, name);
        GL.Uniform3(location, value);
    }

    public void SetVector(string name, Vector4 value)
    {
        var location = GL.GetUniformLocation(program, name);
        GL.Uniform4(location, value);
    }

    public void SetMatrix(string name, Matrix4 value)
    {
        var location = GL.GetUniformLocation(program, name);
        GL.UniformMatrix4(location, false, ref value);
    }
}
