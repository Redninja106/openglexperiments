using OpenTK.Mathematics;

namespace ConsoleApp21;

struct Material
{
    public Texture? diffuseTexture;
    public Texture? specularTexture;
    public Texture? emissionTexture;
    public float shininess;

    public Material(Texture? diffuse, Texture? specular, Texture? emission, float shininess)
    {
        this.diffuseTexture = diffuse;
        this.specularTexture = specular;
        this.emissionTexture = emission;
        this.shininess = shininess;
    }

    public void Apply(Shader shader, string name)
    {
        diffuseTexture?.Apply(shader, $"{name}.emission", 0);
        specularTexture?.Apply(shader, $"{name}.emission", 1);
        emissionTexture?.Apply(shader, $"{name}.emission", 2);

        shader.SetFloat($"{name}.shininess", shininess);
    }

    public void Layout()
    {
        ImGui.Image(diffuseTexture?.TextureID ?? 0, new(100, 100));
        ImGui.SameLine();
        ImGui.Text("diffuse");
        
        ImGui.Image(specularTexture?.TextureID ?? 0, new(100, 100));
        ImGui.SameLine();
        ImGui.Text("specular");

        ImGui.Image(emissionTexture?.TextureID ?? 0, new(100, 100));
        ImGui.SameLine();
        ImGui.Text("emission");

        ImGui.DragFloat("shininess", ref shininess);
    }
}