using ImGuiNET;
using OpenTK.ImGui;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;
using StbImageSharp;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Transactions;

namespace ConsoleApp21;

class Window : GameWindow
{
    ImGuiController imGuiController;

    Shader litShader;
    Shader lampShader;
    int texture1, texture2;

    int vbo; //, ebo;
    int vao, lightVao;

    DebugProc debugMessageCallback;

    VertexPositionNormal[] vertices = new VertexPositionNormal[]
    {
        new(new(-0.5f, -0.5f, -0.5f), new( 0.0f,  0.0f, -1.0f)),
        new(new( 0.5f, -0.5f, -0.5f), new( 0.0f,  0.0f, -1.0f)),
        new(new( 0.5f,  0.5f, -0.5f), new( 0.0f,  0.0f, -1.0f)),
        new(new( 0.5f,  0.5f, -0.5f), new( 0.0f,  0.0f, -1.0f)),
        new(new(-0.5f,  0.5f, -0.5f), new( 0.0f,  0.0f, -1.0f)),
        new(new(-0.5f, -0.5f, -0.5f), new( 0.0f,  0.0f, -1.0f)),
        new(new(-0.5f, -0.5f,  0.5f), new( 0.0f,  0.0f,  1.0f)),
        new(new( 0.5f, -0.5f,  0.5f), new( 0.0f,  0.0f,  1.0f)),
        new(new( 0.5f,  0.5f,  0.5f), new( 0.0f,  0.0f,  1.0f)),
        new(new( 0.5f,  0.5f,  0.5f), new( 0.0f,  0.0f,  1.0f)),
        new(new(-0.5f,  0.5f,  0.5f), new( 0.0f,  0.0f,  1.0f)),
        new(new(-0.5f, -0.5f,  0.5f), new( 0.0f,  0.0f,  1.0f)),
        new(new(-0.5f,  0.5f,  0.5f), new(-1.0f,  0.0f,  0.0f)),
        new(new(-0.5f,  0.5f, -0.5f), new(-1.0f,  0.0f,  0.0f)),
        new(new(-0.5f, -0.5f, -0.5f), new(-1.0f,  0.0f,  0.0f)),
        new(new(-0.5f, -0.5f, -0.5f), new(-1.0f,  0.0f,  0.0f)),
        new(new(-0.5f, -0.5f,  0.5f), new(-1.0f,  0.0f,  0.0f)),
        new(new(-0.5f,  0.5f,  0.5f), new(-1.0f,  0.0f,  0.0f)),
        new(new( 0.5f,  0.5f,  0.5f), new( 1.0f,  0.0f,  0.0f)),
        new(new( 0.5f,  0.5f, -0.5f), new( 1.0f,  0.0f,  0.0f)),
        new(new( 0.5f, -0.5f, -0.5f), new( 1.0f,  0.0f,  0.0f)),
        new(new( 0.5f, -0.5f, -0.5f), new( 1.0f,  0.0f,  0.0f)),
        new(new( 0.5f, -0.5f,  0.5f), new( 1.0f,  0.0f,  0.0f)),
        new(new( 0.5f,  0.5f,  0.5f), new( 1.0f,  0.0f,  0.0f)),
        new(new(-0.5f, -0.5f, -0.5f), new( 0.0f, -1.0f,  0.0f)),
        new(new( 0.5f, -0.5f, -0.5f), new( 0.0f, -1.0f,  0.0f)),
        new(new( 0.5f, -0.5f,  0.5f), new( 0.0f, -1.0f,  0.0f)),
        new(new( 0.5f, -0.5f,  0.5f), new( 0.0f, -1.0f,  0.0f)),
        new(new(-0.5f, -0.5f,  0.5f), new( 0.0f, -1.0f,  0.0f)),
        new(new(-0.5f, -0.5f, -0.5f), new( 0.0f, -1.0f,  0.0f)),
        new(new(-0.5f,  0.5f, -0.5f), new( 0.0f,  1.0f,  0.0f)),
        new(new( 0.5f,  0.5f, -0.5f), new( 0.0f,  1.0f,  0.0f)),
        new(new( 0.5f,  0.5f,  0.5f), new( 0.0f,  1.0f,  0.0f)),
        new(new( 0.5f,  0.5f,  0.5f), new( 0.0f,  1.0f,  0.0f)),
        new(new(-0.5f,  0.5f,  0.5f), new( 0.0f,  1.0f,  0.0f)),
        new(new(-0.5f,  0.5f, -0.5f), new( 0.0f,  1.0f,  0.0f))
    };

    Vector3[] positions = new Vector3[]
    {
        new( 0.0f,  0.0f,  0.0f),
        new( 2.0f,  5.0f, -15.0f),
        new(-1.5f, -2.2f, -2.5f),
        new(-3.8f, -2.0f, -12.3f),
        new( 2.4f, -0.4f, -3.5f),
        new(-1.7f,  3.0f, -7.5f),
        new( 1.3f, -2.0f, -2.5f),
        new( 1.5f,  2.0f, -2.5f),
        new( 1.5f,  0.2f, -1.5f),
        new(-1.3f,  1.0f, -1.5f)
    };

    protected override void OnLoad()
    {
        imGuiController = new(this);
        Size = new(1280, 720);

        debugMessageCallback = this.DebugMessage;
        GL.DebugMessageCallback(debugMessageCallback, 0);

        lampShader = new("Shaders/vertexposition.glsl", "Shaders/lamp.glsl");
        litShader = new("Shaders/vertexpositionnormal.glsl", "Shaders/lit.glsl");

        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * VertexPositionNormal.SizeInBytes, vertices, BufferUsageHint.StaticDraw);
        VertexPositionNormal.SetAttributes();

        // ebo = GL.GenBuffer();
        // GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        // GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);

        lightVao = GL.GenVertexArray();
        GL.BindVertexArray(lightVao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

        VertexPositionNormal.SetAttributes();

        LoadTextures();

        xr = 0;
        yr = MathF.PI;
        cameraPosition = new Vector3(0, 0, 3);
        
        light = new(
            new(0, 0, -5), 
            new(0.2f, 0.2f, 0.2f), 
            new(0.5f, 0.5f, 0.5f), 
            new(1.0f, 1.0f, 1.0f)
            );

        Material bronze = new(
            new(0.2125f,  0.1275f,  0.054f),
            new(0.714f,   0.4284f,  0.18144f),
            new(0.393548f,    0.271906f,    0.166721f),
            0.2f
            );

        Material ruby = new(
            new(0.1745f, 0.01175f, 0.01175f),
            new(0.61424f, 0.04136f, 0.04136f),
            new(0.727811f, 0.626959f, 0.626959f),
            0.6f
            );

        material = ruby;

        base.OnLoad();
    }

    Vector3 cameraPosition;
    float xr, yr;

    float r;
    float t;

    Light light;
    Material material;

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        t += (float)args.Time;
        imGuiController.Update(this, (float)args.Time);

        HandleInput((float)args.Time);

        GL.ClearColor(new Color4(.1f, .1f, .12f, 1f));
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        ImGui.DragFloat("r", ref r);

        if (ImGui.CollapsingHeader("material"))
        {
            ImGui.PushID("material");
            material.Layout();
            ImGui.PopID();
        }

        if (ImGui.CollapsingHeader("light"))
        {
            ImGui.PushID("light");
            light.Layout();
            ImGui.PopID();
        }
        else 
        {
            Vector3 lightCol = new(MathF.Sin(t * 2), MathF.Sin(t * .7f), MathF.Sin(t * .2f));
            light.ambient = lightCol * .2f;
            light.diffuse = lightCol * .5f;
        }
        GL.Enable(EnableCap.DepthTest);
        var viewRotMatrix = Matrix3.CreateRotationY(yr) * Matrix3.CreateRotationX(xr);

        Matrix4 view = Matrix4.LookAt(cameraPosition, cameraPosition + viewRotMatrix * Vector3.UnitZ, Vector3.UnitY);
        Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), ClientSize.X / (float)ClientSize.Y, .1f, 100f);

        GL.BindVertexArray(vao);
        litShader.Use();

        litShader.SetMatrix("view", view);
        litShader.SetMatrix("proj", proj);

        litShader.SetVector("viewPos", cameraPosition);

        material.Apply(litShader, "material");
        light.Apply(litShader, "light");

        for (int i = 0; i < positions.Length; i++)
        {
            Matrix4 translation = Matrix4.CreateTranslation(positions[i]);
            float angle = 20 * (i + r + (i % 3 is 0 ? t : 0));
            Matrix4 rotation = Matrix4.CreateFromAxisAngle(new(1, .3f, .5f), MathHelper.DegreesToRadians(angle));
            litShader.SetMatrix("model", rotation * translation);

            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }

        GL.BindVertexArray(lightVao);
        lampShader.Use();

        lampShader.SetMatrix("view", view);
        lampShader.SetMatrix("proj", proj);
        lampShader.SetMatrix("model", Matrix4.CreateTranslation(light.position));

        lampShader.SetVector("lightColor", light.diffuse + light.ambient);

        GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);

        imGuiController.Render();
        SwapBuffers();

        base.OnRenderFrame(args);
    }

    void LoadTextures()
    {

        StbImage.stbi_set_flip_vertically_on_load(1);

        var bytes1 = File.ReadAllBytes("Assets/container.jpg");
        var image1 = ImageResult.FromMemory(bytes1);

        texture1 = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, texture1);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, image1.Width, image1.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, image1.Data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        var bytes2 = File.ReadAllBytes("Assets/awesomeface.png");

        var image2 = ImageResult.FromMemory(bytes2, ColorComponents.RedGreenBlue);

        texture2 = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, texture2);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, image2.Width, image2.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, image2.Data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

    }

    private void HandleInput(float dt)
    {
        if (MouseState.IsButtonDown(MouseButton.Right))
        {
            xr -= MouseState.Delta.Y * 0.001f;
            yr += MouseState.Delta.X * 0.001f;

            xr = Math.Clamp(xr, -MathF.PI * .5f, MathF.PI * .5f);
        }

        var rotMatrix = Matrix3.CreateRotationY(yr) * Matrix3.CreateRotationX(xr);

        Vector3 delta = Vector3.Zero;
        if (IsKeyDown(Keys.W))
            delta += Vector3.UnitZ;
        if (IsKeyDown(Keys.A))
            delta += Vector3.UnitX;
        if (IsKeyDown(Keys.S))
            delta -= Vector3.UnitZ;
        if (IsKeyDown(Keys.D))
            delta -= Vector3.UnitX;
        if (IsKeyDown(Keys.Space))
            delta += Vector3.UnitY;
        if (IsKeyDown(Keys.C))
            delta -= Vector3.UnitY;

        cameraPosition += rotMatrix * (delta * dt * 5f);
    }

    protected override void OnUnload()
    {
        litShader.Dispose();
        GL.DeleteBuffer(vbo);
        imGuiController.Dispose();

        base.OnUnload();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, e.Width, e.Height);

        base.OnResize(e);
    }

    private unsafe void DebugMessage(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, nint message, nint userParam)
    {
        if (severity is not DebugSeverity.DebugSeverityHigh)
            return;

        var msg = new string((sbyte*)message, 0, length);
        Console.WriteLine($"{source}: {msg}");
    }

    private static NativeWindowSettings GetNativeWindowSettings()
    {
        var result = NativeWindowSettings.Default;
        result.Flags |= ContextFlags.Debug;
        result.APIVersion = new(4, 1);
        return result;
    }

    public Window() : base(GameWindowSettings.Default, GetNativeWindowSettings())
    {
    }
}

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

struct Material
{
    public Vector3 ambient;
    public Vector3 diffuse;
    public Vector3 specular;
    public float shininess;

    public Material(Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess)
    {
        this.ambient = ambient;
        this.diffuse = diffuse;
        this.specular = specular;
        this.shininess = shininess;
    }

    public void Apply(Shader shader, string name)
    {
        shader.SetVector($"{name}.ambient", ambient);
        shader.SetVector($"{name}.diffuse", diffuse);
        shader.SetVector($"{name}.specular", specular);
        shader.SetFloat($"{name}.shininess", shininess);
    }

    public void Layout()
    {
        ImGui.ColorEdit3("ambient", ref ambient.ImGui());
        ImGui.ColorEdit3("diffuse", ref diffuse.ImGui());
        ImGui.ColorEdit3("specular", ref specular.ImGui());
        ImGui.DragFloat("shininess", ref shininess);
    }
}

interface IVertex
{
    static abstract int SizeInBytes { get; }
    static abstract void SetAttributes();
}

struct VertexPosition : IVertex
{
    public static int SizeInBytes => Unsafe.SizeOf<VertexPosition>();
    public Vector3 Position;

    public VertexPosition(Vector3 position)
    {
        Position = position;
    }

    public static void SetAttributes()
    {
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, SizeInBytes, 0);
        GL.EnableVertexAttribArray(0);
    }
}

struct VertexPositionNormal : IVertex
{
    public static int SizeInBytes => Unsafe.SizeOf<VertexPositionNormal>();
    public Vector3 Position;
    public Vector3 Normal;

    public VertexPositionNormal(Vector3 position, Vector3 normal)
    {
        Position = position;
        Normal = normal;
    }

    public static void SetAttributes()
    {
        nint positionOffset = Marshal.OffsetOf<VertexPositionNormal>(nameof(Position));
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, SizeInBytes, positionOffset);
        GL.EnableVertexAttribArray(0);

        nint normalOffset = Marshal.OffsetOf<VertexPositionNormal>(nameof(Normal));
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, SizeInBytes, normalOffset);
        GL.EnableVertexAttribArray(1);
    }
}

struct VertexPositionTexCoord : IVertex
{
    public static int SizeInBytes => Unsafe.SizeOf<VertexPositionTexCoord>();

    public Vector3 Position;
    public Vector2 TexCoord;

    public VertexPositionTexCoord(Vector3 position, Vector2 texCoord)
    {
        Position = position;
        TexCoord = texCoord;
    }

    public static void SetAttributes()
    {
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, SizeInBytes, 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, SizeInBytes, Vector3.SizeInBytes);
        GL.EnableVertexAttribArray(1);
    }
}

static class Extensions
{
    public static ref System.Numerics.Vector3 ImGui(ref this Vector3 vector)
    {
        return ref Unsafe.As<Vector3, System.Numerics.Vector3>(ref vector);
    }
}