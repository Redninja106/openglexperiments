using OpenTK.Mathematics;
using System.Xml.Linq;

namespace ConsoleApp21;

struct PointLight
{
    public Vector3 position;

    public float constant;
    public float linear;
    public float quadratic;

    public LightColor color;

    public PointLight(Vector3 position, LightColor color)
    {
        this.position = position;
        this.color = color;
    }

    public void Apply(Shader shader, string name)
    {
        shader.SetVector($"{name}.position", position);
        shader.SetFloat($"{name}.constant", constant);
        shader.SetFloat($"{name}.linear", linear);
        shader.SetFloat($"{name}.quadratic", quadratic);
        color.Apply(shader, $"{name}.color");
    }

    public void Layout()
    {
        ImGui.DragFloat3("position", ref position.ImGui());
        ImGui.SliderFloat("constant", ref constant, 0, 2);
        ImGui.SliderFloat("linear", ref linear, 0.001f, .1f);
        ImGui.SliderFloat("quadratic", ref quadratic, 0.001f, .1f);
        color.Layout();
    }
}