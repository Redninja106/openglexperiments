using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp21;
internal struct DirectionalLight
{
    public Vector3 direction;
    public LightColor color;

    public DirectionalLight(Vector3 direction, LightColor color)
    {
        this.direction = direction;
        this.color = color;
    }

    public void Apply(Shader shader, string name)
    {
        shader.SetVector($"{name}.direction", direction);
        color.Apply(shader, $"{name}.color");
    }

    public void Layout()
    {
        ImGui.DragFloat3("direction", ref direction.ImGui());
        color.Layout();
    }
}
