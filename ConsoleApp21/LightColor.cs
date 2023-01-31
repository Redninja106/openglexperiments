using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp21;
internal struct LightColor
{
    public Vector3 ambient;
    public Vector3 diffuse;
    public Vector3 specular;

    public LightColor(Vector3 ambient, Vector3 diffuse, Vector3 specular)
    {
        this.ambient = ambient;
        this.diffuse = diffuse;
        this.specular = specular;
    }

    public void Apply(Shader shader, string name)
    {
        shader.SetVector($"{name}.ambient", ambient);
        shader.SetVector($"{name}.diffuse", diffuse);
        shader.SetVector($"{name}.specular", specular);
    }

    public void Layout()
    {
        ImGui.ColorEdit3("ambient", ref ambient.ImGui());
        ImGui.ColorEdit3("diffuse", ref diffuse.ImGui());
        ImGui.ColorEdit3("specular", ref specular.ImGui());
    }
}
