using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp21;
internal struct SpotLight
{
    public Vector3 direction;
    public float innerAngle;
    public float outerAngle;
    public PointLight pointLight;

    public SpotLight(PointLight pointLight, Vector3 direction, float innerAngle, float outerAngle)
    {
        this.pointLight = pointLight;
        this.direction = direction;
        this.innerAngle = innerAngle;
        this.outerAngle = outerAngle;
    }

    public void Apply(Shader shader, string name)
    {
        pointLight.Apply(shader, $"{name}.pointLight");
        shader.SetVector($"{name}.direction", direction);
        shader.SetFloat($"{name}.innerCutoff", MathF.Cos(innerAngle));
        shader.SetFloat($"{name}.outerCutoff", MathF.Cos(outerAngle));
    }

    public void Layout()
    {
        ImGui.DragFloat("innerAngle", ref innerAngle);
        ImGui.DragFloat("outerAngle", ref outerAngle);
        ImGui.DragFloat3("direction", ref direction.ImGui());
        pointLight.Layout();
    }
}
