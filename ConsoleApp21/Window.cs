using ImGuiNET;
using OpenTK.ImGui;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;
using StbImageSharp;
using System;
using System.Drawing;
using System.Transactions;

namespace ConsoleApp21;

class Window : GameWindow
{
    ImGuiController imGuiController;

    Shader litShader;
    Shader lampShader;
    Texture texture1, texture2;
    Texture container2, container2Specular, container2Emission;

    int vbo; //, ebo;
    int vao, lightVao;

    DebugProc debugMessageCallback;

    VertexPositionTextureNormal[] vertices = new VertexPositionTextureNormal[]
    {
        new(new(-0.5f, -0.5f, -0.5f),  new( 0.0f, 0.0f), new( 0.0f,  0.0f, -1.0f)),
        new(new( 0.5f, -0.5f, -0.5f),  new( 1.0f, 0.0f), new( 0.0f,  0.0f, -1.0f)),
        new(new( 0.5f,  0.5f, -0.5f),  new( 1.0f, 1.0f), new( 0.0f,  0.0f, -1.0f)),
        new(new( 0.5f,  0.5f, -0.5f),  new( 1.0f, 1.0f), new( 0.0f,  0.0f, -1.0f)),
        new(new(-0.5f,  0.5f, -0.5f),  new( 0.0f, 1.0f), new( 0.0f,  0.0f, -1.0f)),
        new(new(-0.5f, -0.5f, -0.5f),  new( 0.0f, 0.0f), new( 0.0f,  0.0f, -1.0f)),
        new(new(-0.5f, -0.5f,  0.5f),  new( 0.0f, 0.0f), new( 0.0f,  0.0f,  1.0f)),
        new(new( 0.5f, -0.5f,  0.5f),  new( 1.0f, 0.0f), new( 0.0f,  0.0f,  1.0f)),
        new(new( 0.5f,  0.5f,  0.5f),  new( 1.0f, 1.0f), new( 0.0f,  0.0f,  1.0f)),
        new(new( 0.5f,  0.5f,  0.5f),  new( 1.0f, 1.0f), new( 0.0f,  0.0f,  1.0f)),
        new(new(-0.5f,  0.5f,  0.5f),  new( 0.0f, 1.0f), new( 0.0f,  0.0f,  1.0f)),
        new(new(-0.5f, -0.5f,  0.5f),  new( 0.0f, 0.0f), new( 0.0f,  0.0f,  1.0f)),
        new(new(-0.5f,  0.5f,  0.5f),  new( 1.0f, 0.0f), new(-1.0f,  0.0f,  0.0f)),
        new(new(-0.5f,  0.5f, -0.5f),  new( 1.0f, 1.0f), new(-1.0f,  0.0f,  0.0f)),
        new(new(-0.5f, -0.5f, -0.5f),  new( 0.0f, 1.0f), new(-1.0f,  0.0f,  0.0f)),
        new(new(-0.5f, -0.5f, -0.5f),  new( 0.0f, 1.0f), new(-1.0f,  0.0f,  0.0f)),
        new(new(-0.5f, -0.5f,  0.5f),  new( 0.0f, 0.0f), new(-1.0f,  0.0f,  0.0f)),
        new(new(-0.5f,  0.5f,  0.5f),  new( 1.0f, 0.0f), new(-1.0f,  0.0f,  0.0f)),
        new(new( 0.5f,  0.5f,  0.5f),  new( 1.0f, 0.0f), new( 1.0f,  0.0f,  0.0f)),
        new(new( 0.5f,  0.5f, -0.5f),  new( 1.0f, 1.0f), new( 1.0f,  0.0f,  0.0f)),
        new(new( 0.5f, -0.5f, -0.5f),  new( 0.0f, 1.0f), new( 1.0f,  0.0f,  0.0f)),
        new(new( 0.5f, -0.5f, -0.5f),  new( 0.0f, 1.0f), new( 1.0f,  0.0f,  0.0f)),
        new(new( 0.5f, -0.5f,  0.5f),  new( 0.0f, 0.0f), new( 1.0f,  0.0f,  0.0f)),
        new(new( 0.5f,  0.5f,  0.5f),  new( 1.0f, 0.0f), new( 1.0f,  0.0f,  0.0f)),
        new(new(-0.5f, -0.5f, -0.5f),  new( 0.0f, 1.0f), new( 0.0f, -1.0f,  0.0f)),
        new(new( 0.5f, -0.5f, -0.5f),  new( 1.0f, 1.0f), new( 0.0f, -1.0f,  0.0f)),
        new(new( 0.5f, -0.5f,  0.5f),  new( 1.0f, 0.0f), new( 0.0f, -1.0f,  0.0f)),
        new(new( 0.5f, -0.5f,  0.5f),  new( 1.0f, 0.0f), new( 0.0f, -1.0f,  0.0f)),
        new(new(-0.5f, -0.5f,  0.5f),  new( 0.0f, 0.0f), new( 0.0f, -1.0f,  0.0f)),
        new(new(-0.5f, -0.5f, -0.5f),  new( 0.0f, 1.0f), new( 0.0f, -1.0f,  0.0f)),
        new(new(-0.5f,  0.5f, -0.5f),  new( 0.0f, 1.0f), new( 0.0f,  1.0f,  0.0f)),
        new(new( 0.5f,  0.5f, -0.5f),  new( 1.0f, 1.0f), new( 0.0f,  1.0f,  0.0f)),
        new(new( 0.5f,  0.5f,  0.5f),  new( 1.0f, 0.0f), new( 0.0f,  1.0f,  0.0f)),
        new(new( 0.5f,  0.5f,  0.5f),  new( 1.0f, 0.0f), new( 0.0f,  1.0f,  0.0f)),
        new(new(-0.5f,  0.5f,  0.5f),  new( 0.0f, 0.0f), new( 0.0f,  1.0f,  0.0f)),
        new(new(-0.5f,  0.5f, -0.5f),  new( 0.0f, 1.0f), new( 0.0f,  1.0f,  0.0f))
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

    DirectionalLight directionalLight = default;
    PointLight[] pointLights = new PointLight[8];
    SpotLight[] spotLights = new SpotLight[8];

    Vector3 cameraPosition;
    float xr, yr;

    float r;
    float t;

    Material material;

    protected override void OnLoad()
    {
        imGuiController = new(this);
        Size = new(1280, 720);

        debugMessageCallback = this.DebugMessage;
        GL.DebugMessageCallback(debugMessageCallback, 0);

        lampShader = new("Shaders/Vertex/VertexPosition.glsl", "Shaders/lamp.glsl");
        litShader = new("Shaders/Vertex/VertexPositionTextureNormal.glsl", "Shaders/lit.glsl");

        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * VertexPositionTextureNormal.SizeInBytes, vertices, BufferUsageHint.StaticDraw);
        VertexPositionTextureNormal.SetAttributes();

        // ebo = GL.GenBuffer();
        // GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        // GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);

        lightVao = GL.GenVertexArray();
        GL.BindVertexArray(lightVao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

        VertexPositionTextureNormal.SetAttributes();

        texture1 = new Texture(TextureKind.Diffuse, "Assets/container.jpg");
        texture2 = new Texture(TextureKind.Diffuse, "Assets/awesomeface.png");
        container2 = new Texture(TextureKind.Diffuse, "Assets/container2.png");
        container2Specular = new Texture(TextureKind.Specular, "Assets/container2_specular.png");
        container2Emission = new Texture(TextureKind.Emission, "Assets/matrix.jpg");

        xr = 0;
        yr = MathF.PI;
        cameraPosition = new Vector3(0, 0, 3);

        pointLights[0] = new(
            new(0, 0, -5), 
            new(
                new(0.2f, 0.2f, 0.2f), 
                new(0.5f, 0.5f, 0.5f), 
                new(1.0f, 1.0f, 1.0f)
                )
            );

        material = new(
            container2,
            container2Specular,
            null,//container2Emission,
            .25f
            );

        for (int i = 0; i < pointLights.Length; i++)
        {
            ref var light = ref pointLights[i];
            light.position = new(
                (Random.Shared.NextSingle() - .5f) * 10,
                (Random.Shared.NextSingle() - .5f) * 10,
                (Random.Shared.NextSingle() - .5f) * 10
                );
            light.constant = 1.0f;
            light.linear = 0.09f;
            light.quadratic = 0.032f;

            light.color.specular = Vector3.One;
        }

        base.OnLoad();
    }

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

        if (ImGui.CollapsingHeader("directional light"))
        {
            ImGui.PushID("directional light");
            directionalLight.Layout();
            ImGui.PopID();
        }

        for (int i = 0; i < pointLights.Length; i++)
        {
            ref var light = ref pointLights[i];
            if (ImGui.CollapsingHeader("light " + i))
            {
                ImGui.PushID("light " + i);
                light.Layout();
                ImGui.PopID();
            }
            else
            {
                // Vector3 lightCol = new(MathF.Sin(t * 2), MathF.Sin(t * .7f), MathF.Sin(t * .2f));
                // lightCol = (lightCol + Vector3.One) * .5f;
                // light.color.ambient = lightCol * .2f;
                // light.color.diffuse = lightCol * .5f;
            }
        }

        for (int i = 0; i < spotLights.Length; i++)
        {
            ref var light = ref spotLights[i];
            if (ImGui.CollapsingHeader("spotlight " + i))
            {
                ImGui.PushID("spotlight " + i);
                light.Layout();
                ImGui.PopID();
            }
            else
            {
                // Vector3 lightCol = new(MathF.Sin(t * 2), MathF.Sin(t * .7f), MathF.Sin(t * .2f));
                // lightCol = (lightCol + Vector3.One) * .5f;
                // light.color.ambient = lightCol * .2f;
                // light.color.diffuse = lightCol * .5f;
            }
        }

        GL.Enable(EnableCap.DepthTest);
        var viewRotMatrix = Matrix3.CreateRotationY(yr) * Matrix3.CreateRotationX(xr);

        Matrix4 view = Matrix4.LookAt(cameraPosition, cameraPosition + viewRotMatrix * Vector3.UnitZ, Vector3.UnitY);
        Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), ClientSize.X / (float)ClientSize.Y, .1f, 100f);

        GL.BindVertexArray(vao);
        litShader.Use();

        litShader.SetMatrix("view", view);
        litShader.SetMatrix("proj", proj);
        litShader.SetFloat("time", t);
        litShader.SetVector("viewPos", cameraPosition);

        material.Apply(litShader, "material");
        directionalLight.Apply(litShader, "directionalLight");
        
        for (int i = 0; i < pointLights.Length; i++)
        {
            var light = pointLights[i];
            light.Apply(litShader, $"pointLights[{i}]");
        }

        for (int i = 0; i < spotLights.Length; i++)
        {
            var light = spotLights[i];
            light.Apply(litShader, $"spotLights[{i}]");
        }

        for (int i = 0; i < positions.Length; i++)
        {
            Matrix4 translation = Matrix4.CreateTranslation(positions[i]);
            float angle = 20 * (i + r + (i % 3 is 0 ? t : 0));
            Matrix4 rotation = Matrix4.CreateFromAxisAngle(new(1, .3f, .5f), MathHelper.DegreesToRadians(angle));
            litShader.SetMatrix("model", rotation * translation);

            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }
        
        litShader.SetMatrix("model", Matrix4.CreateScale(100) * Matrix4.CreateTranslation(0,-55,0));
        GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);

        GL.BindVertexArray(lightVao);
        lampShader.Use();

        lampShader.SetMatrix("view", view);
        lampShader.SetMatrix("proj", proj);

        foreach (var light in pointLights)
        {
            lampShader.SetMatrix("model", Matrix4.CreateTranslation(new(light.position)));
            lampShader.SetVector("lightColor", light.color.diffuse + light.color.ambient);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }

        imGuiController.Render();
        SwapBuffers();

        base.OnRenderFrame(args);
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
