using OpenTK.Mathematics;

namespace ConsoleApp21;

struct Light
{
    public Vector3 position;

    public Vector3 ambient;
    public Vector3 diffuse;
    public Vector3 specular;

    public Light(Vector3 position, Vector3 ambient, Vector3 diffuse, Vector3 specular)
    {
        this.position = position;
        this.ambient = ambient;
        this.diffuse = diffuse;
        this.specular = specular;
    }

    public void Apply(Shader shader, string name)
    {
        shader.SetVector($"{name}.position", position);
        shader.SetVector($"{name}.ambient", ambient);
        shader.SetVector($"{name}.diffuse", diffuse);
        shader.SetVector($"{name}.specular", specular);
    }

    public void Layout()
    {
        ImGui.DragFloat3("position", ref position.ImGui());
        ImGui.ColorEdit3("ambient", ref ambient.ImGui());
        ImGui.ColorEdit3("diffuse", ref diffuse.ImGui());
        ImGui.ColorEdit3("specular", ref specular.ImGui());
    }
}