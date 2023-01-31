using OpenTK.Mathematics;

namespace ConsoleApp21;

struct Material
{
    public int diffuseTexture;
    public int specularTexture;
    public int emissionTexture;
    public float shininess;

    public Material(int diffuse, int specular, int emission, float shininess)
    {
        this.diffuseTexture = diffuse;
        this.specularTexture = specular;
        this.emissionTexture = emission;
        this.shininess = shininess;
    }

    public void Apply(Shader shader, string name)
    {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, this.diffuseTexture);
        shader.SetInt($"{name}.diffuse", 0);

        GL.ActiveTexture(TextureUnit.Texture1);
        GL.BindTexture(TextureTarget.Texture2D, this.specularTexture);
        shader.SetInt($"{name}.specular", 1);

        GL.ActiveTexture(TextureUnit.Texture2);
        GL.BindTexture(TextureTarget.Texture2D, this.emissionTexture);
        shader.SetInt($"{name}.emission", 2);

        shader.SetFloat($"{name}.shininess", shininess);
    }

    public void Layout()
    {
        ImGui.Image(diffuseTexture, new(100, 100));
        ImGui.SameLine();
        ImGui.Text("diffuse");
        
        ImGui.Image(specularTexture, new(100, 100));
        ImGui.SameLine();
        ImGui.Text("specular");

        ImGui.Image(emissionTexture, new(100, 100));
        ImGui.SameLine();
        ImGui.Text("emission");

        ImGui.DragFloat("shininess", ref shininess);
    }
}