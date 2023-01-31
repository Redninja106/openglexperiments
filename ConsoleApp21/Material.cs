using OpenTK.Mathematics;

namespace ConsoleApp21;

struct Material
{
    public int diffuseTexture;
    public Vector3 specular;
    public float shininess;

    public Material(int diffuse, Vector3 specular, float shininess)
    {
        this.diffuseTexture = diffuse;
        this.specular = specular;
        this.shininess = shininess;
    }

    public void Apply(Shader shader, string name, int textureUnit)
    {
        GL.ActiveTexture(TextureUnit.Texture0 + textureUnit);
        GL.BindTexture(TextureTarget.Texture2D, this.diffuseTexture);
        shader.SetInt($"{name}.diffuse", textureUnit);
        shader.SetVector($"{name}.specular", specular);
        shader.SetFloat($"{name}.shininess", shininess);
    }

    public void Layout()
    {
        ImGui.Image(diffuseTexture, new(0, 0));
        ImGui.ColorEdit3("specular", ref specular.ImGui());
        ImGui.DragFloat("shininess", ref shininess);
    }
}