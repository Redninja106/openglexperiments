using OpenTK.Mathematics;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp21;
internal class Texture : IDisposable
{
    private static readonly Dictionary<string, Texture> loadedTextures = new();
    public static Texture SpecularAlways { get; } = new(TextureKind.Specular, Create1x1ImageResult(0xFFFFFFFF));

    public TextureKind Kind { get; }
    public int TextureID { get; }
    public string? Path { get; }

    static Texture()
    {
        StbImage.stbi_set_flip_vertically_on_load(0);
    }

    private Texture(TextureKind kind, string? path)
    {
        this.Kind = kind;
        this.Path = path;

        this.TextureID = Initialize(LoadImage(path));
    }

    private Texture(TextureKind kind, ImageResult image)
    {
        this.Kind = kind;
        this.Path = null;

        this.TextureID = Initialize(image);
    }

    public Texture(TextureKind kind, int texture)
    {
        this.Kind = kind;
        this.TextureID = texture;
        this.Path = null;
    }

    public static Texture Load(TextureKind kind, string? path)
    {
        path ??= string.Empty;

        if (!loadedTextures.ContainsKey(path))
        {
            loadedTextures.Add(path, new(kind, path));
        }

        return loadedTextures[path];
    }

    private static ImageResult? LoadImage(string? path)
    {
        if (string.IsNullOrEmpty(path))
            return null;
            
        var bytes = File.ReadAllBytes(path);
        var image = ImageResult.FromMemory(bytes, ColorComponents.RedGreenBlue);
        return image;
    }

    private static int Initialize(ImageResult? image)
    {
        if (image is null)
            return 0;

        var texture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, texture);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Srgb, image.Width, image.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, image.Data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        return texture;
    }

    private static ImageResult Create1x1ImageResult(uint value)
    {
        ImageResult image = new();
        image.Width = image.Height = 1;
        unsafe 
        {
            image.Data = new Span<byte>(Unsafe.AsPointer(ref value), Unsafe.SizeOf<uint>()).ToArray();
        }
        return image;
    }

    public void Dispose()
    {
        loadedTextures.Remove(this.Path);
        GL.DeleteTexture(this.TextureID);
    }

    public void Apply(Shader shader, string name, int unit)
    {
        GL.ActiveTexture(TextureUnit.Texture0 + unit);
        GL.BindTexture(TextureTarget.Texture2D, this.TextureID);
        shader.SetInt(name, unit);
    }
}
