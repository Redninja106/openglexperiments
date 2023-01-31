using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp21;
internal class Texture : IDisposable
{
    public TextureKind Kind { get; }

    public int TextureID { get; }

    static Texture()
    {
        StbImage.stbi_set_flip_vertically_on_load(1);
    }

    public Texture(TextureKind kind, string path)
    {
        this.Kind = kind;

        this.TextureID = Load(path);
    }

    private int Load(string path)
    {
        var bytes = File.ReadAllBytes(path);
        var image = ImageResult.FromMemory(bytes, ColorComponents.RedGreenBlue);

        var texture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, texture);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, image.Data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        return texture;
    }

    public void Dispose()
    {
    }

    public void Apply(Shader shader, string name, int unit)
    {
        GL.ActiveTexture(TextureUnit.Texture0 + unit);
        GL.BindTexture(TextureTarget.Texture2D, this.TextureID);
        shader.SetInt(name, 2);
    }
}
