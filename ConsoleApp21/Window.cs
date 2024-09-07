using ImGuiNET;
using OpenTK.ImGui;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace ConsoleApp21;

class Window : GameWindow
{
    ImGuiController imGuiController;

    Shader litShader, depthShader;
    Shader lampShader;
    Shader quadShader;

    Mesh<VertexPositionTextureNormalTangent> planeMesh;
    Model backpack;
    Model sponza;
    Mesh<VertexPositionTextureNormal> cube;
    Mesh<VertexPositionTexture> quad;

    DebugProc debugMessageCallback;

    VertexPositionTextureNormal[] cubeVertices = new VertexPositionTextureNormal[]
    {
        new(new(-0.5f, -0.5f, -0.5f),  new( 0.0f, 0.0f), new( 0.0f,  0.0f, -1.0f)),
        new(new( 0.5f,  0.5f, -0.5f),  new( 1.0f, 1.0f), new( 0.0f,  0.0f, -1.0f)),
        new(new( 0.5f, -0.5f, -0.5f),  new( 1.0f, 0.0f), new( 0.0f,  0.0f, -1.0f)),
        new(new( 0.5f,  0.5f, -0.5f),  new( 1.0f, 1.0f), new( 0.0f,  0.0f, -1.0f)),
        new(new(-0.5f, -0.5f, -0.5f),  new( 0.0f, 0.0f), new( 0.0f,  0.0f, -1.0f)),
        new(new(-0.5f,  0.5f, -0.5f),  new( 0.0f, 1.0f), new( 0.0f,  0.0f, -1.0f)),
        
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
        new(new( 0.5f, -0.5f, -0.5f),  new( 0.0f, 1.0f), new( 1.0f,  0.0f,  0.0f)),
        new(new( 0.5f,  0.5f, -0.5f),  new( 1.0f, 1.0f), new( 1.0f,  0.0f,  0.0f)),
        new(new( 0.5f, -0.5f, -0.5f),  new( 0.0f, 1.0f), new( 1.0f,  0.0f,  0.0f)),
        new(new( 0.5f,  0.5f,  0.5f),  new( 1.0f, 0.0f), new( 1.0f,  0.0f,  0.0f)),
        new(new( 0.5f, -0.5f,  0.5f),  new( 0.0f, 0.0f), new( 1.0f,  0.0f,  0.0f)),
        
        new(new(-0.5f, -0.5f, -0.5f),  new( 0.0f, 1.0f), new( 0.0f, -1.0f,  0.0f)),
        new(new( 0.5f, -0.5f, -0.5f),  new( 1.0f, 1.0f), new( 0.0f, -1.0f,  0.0f)),
        new(new( 0.5f, -0.5f,  0.5f),  new( 1.0f, 0.0f), new( 0.0f, -1.0f,  0.0f)),
        new(new( 0.5f, -0.5f,  0.5f),  new( 1.0f, 0.0f), new( 0.0f, -1.0f,  0.0f)),
        new(new(-0.5f, -0.5f,  0.5f),  new( 0.0f, 0.0f), new( 0.0f, -1.0f,  0.0f)),
        new(new(-0.5f, -0.5f, -0.5f),  new( 0.0f, 1.0f), new( 0.0f, -1.0f,  0.0f)),
        
        new(new(-0.5f,  0.5f, -0.5f),  new( 0.0f, 1.0f), new( 0.0f,  1.0f,  0.0f)),
        new(new( 0.5f,  0.5f,  0.5f),  new( 1.0f, 0.0f), new( 0.0f,  1.0f,  0.0f)),
        new(new( 0.5f,  0.5f, -0.5f),  new( 1.0f, 1.0f), new( 0.0f,  1.0f,  0.0f)),
        new(new( 0.5f,  0.5f,  0.5f),  new( 1.0f, 0.0f), new( 0.0f,  1.0f,  0.0f)),
        new(new(-0.5f,  0.5f, -0.5f),  new( 0.0f, 1.0f), new( 0.0f,  1.0f,  0.0f)),
        new(new(-0.5f,  0.5f,  0.5f),  new( 0.0f, 0.0f), new( 0.0f,  1.0f,  0.0f)),
    };

    VertexPositionTextureNormalTangent[] planeVertices = new VertexPositionTextureNormalTangent[]
    {
        new(new(-0.5f,  0f, -0.5f),  new(0.0f, 1.0f), new(0.0f,  1.0f,  0.0f), new(1.0f, 0.0f, 0.0f), new(0.0f, 0.0f, 1.0f)),
        new(new( 0.5f,  0f,  0.5f),  new(1.0f, 0.0f), new(0.0f,  1.0f,  0.0f), new(1.0f, 0.0f, 0.0f), new(0.0f, 0.0f, 1.0f)),
        new(new( 0.5f,  0f, -0.5f),  new(1.0f, 1.0f), new(0.0f,  1.0f,  0.0f), new(1.0f, 0.0f, 0.0f), new(0.0f, 0.0f, 1.0f)),
        
        new(new( 0.5f,  0f,  0.5f),  new(1.0f, 0.0f), new(0.0f,  1.0f,  0.0f), new(1.0f, 0.0f, 0.0f), new(0.0f, 0.0f, 1.0f)),
        new(new(-0.5f,  0f, -0.5f),  new(0.0f, 1.0f), new(0.0f,  1.0f,  0.0f), new(1.0f, 0.0f, 0.0f), new(0.0f, 0.0f, 1.0f)),
        new(new(-0.5f,  0f,  0.5f),  new(0.0f, 0.0f), new(0.0f,  1.0f,  0.0f), new(1.0f, 0.0f, 0.0f), new(0.0f, 0.0f, 1.0f)),
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
    float zoom = 45;

    float r;
    float t;

    bool renderNormals;
    bool imguiWindowOpen;

    Vector3 cubePosition = new Vector3(0, 1, 0);

    VertexPositionTexture[] GetFullScreenQuad()
    {
        return new VertexPositionTexture[]
        {
            new(new(-1, -1f,  0f),  new(0.0f, 0.0f)),
            new(new( 1, -1f,  0f),  new(1.0f, 0.0f)),
            new(new( 1,  1f,  0f),  new(1.0f, 1.0f)),
            new(new( 1,  1f,  0f),  new(1.0f, 1.0f)),
            new(new(-1,  1f,  0f),  new(0.0f, 1.0f)),
            new(new(-1, -1f,  0f),  new(0.0f, 0.0f)),
        };
    }

    int shadowFBO, shadowDepthMap;
    int depthMapSize = 1024*8;
    bool showShadowMap;
    
    protected override void OnLoad()
    {
        imGuiController = new(this);
        Size = new(1280, 720);

        debugMessageCallback = this.DebugMessage;
        GL.DebugMessageCallback(debugMessageCallback, 0);

        lampShader = new("Shaders/Vertex/VertexPosition.glsl", "Shaders/lamp.glsl");
        litShader = new("Shaders/Vertex/VertexPositionTextureNormalTangent.glsl", "Shaders/lit.glsl");
        depthShader = new("Shaders/Depth/DepthVS.glsl", "Shaders/Depth/DepthFS.glsl");
        quadShader = new("Shaders/Vertex/VertexPositionTexture.glsl", "Shaders/textured.glsl");

        shadowFBO = GL.GenFramebuffer();

        shadowDepthMap = GL.GenTexture();

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, shadowFBO);
        
        GL.BindTexture(TextureTarget.Texture2D, shadowDepthMap);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, depthMapSize, depthMapSize, 0, PixelFormat.DepthComponent, PixelType.Float, 0);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, shadowDepthMap, 0);

        GL.DrawBuffer(DrawBufferMode.None);
        GL.ReadBuffer(ReadBufferMode.None);

        var s = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
        if (s != FramebufferErrorCode.FramebufferComplete)
        {
            throw new Exception();
        }

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        // vao = GL.GenVertexArray();
        // GL.BindVertexArray(vao);
        // 
        // vbo = GL.GenBuffer();
        // GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        // GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * VertexPositionTextureNormal.SizeInBytes, vertices, BufferUsageHint.StaticDraw);
        // VertexPositionTextureNormal.SetAttributes();

        // ebo = GL.GenBuffer();
        // GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        // GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);

        // lightVao = GL.GenVertexArray();
        // GL.BindVertexArray(lightVao);
        // 
        // GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        // 
        // VertexPositionTextureNormal.SetAttributes();

        //var texture1 = Texture.Load(TextureKind.Diffuse, "Assets/container.jpg");
        //var texture2 = Texture.Load(TextureKind.Diffuse, "Assets/awesomeface.png");
        var container2 = Texture.Load(TextureKind.Diffuse, "Assets/container2.png");
        var container2Specular = Texture.Load(TextureKind.Specular, "Assets/container2_specular.png");
        //var container2Emission = Texture.Load(TextureKind.Emission, null); // "Assets/matrix.jpg");
        //var wood = Texture.Load(TextureKind.Diffuse, "Assets/wood.png");
        var brickwall = Texture.Load(TextureKind.Diffuse, "Assets/brickwall/brickwall.jpg");
        var brickwallNormal = Texture.Load(TextureKind.Normal, "Assets/brickwall/brickwall_normal.jpg");

        cube = new(cubeVertices, null, new[] { container2, container2Specular });
        planeMesh = new(planeVertices, null, new[] { brickwall, brickwallNormal, Texture.SpecularAlways });
        quad = new(GetFullScreenQuad(), null, new[] { new Texture(TextureKind.Diffuse, shadowDepthMap) });
        // backpack = new("Assets/backpack/backpack.obj");
        sponza = new Model("Assets/sponza/sponza.gltf");

        xr = 0;
        yr = MathF.PI;
        cameraPosition = new Vector3(0, 0, 3);

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
        }

        //pointLights[0] = new()
        //{
        //    position = new(0, 0, 1),
        //    constant = 1.0f,
        //    linear = 0.09f,
        //    quadratic = 0.032f,
        //    color = new()
        //    {
        //        ambient = new(0.2f, 0.2f, 0.2f),
        //        diffuse = new(0.8f, 0.8f, 0.8f),
        //        specular = new(0.5f, 0.5f, 0.5f)
        //    },
        //};

        directionalLight.color = new()
        {
            ambient = new(0.2f, 0.2f, 0.2f),
            diffuse = new(0.8f, 0.8f, 0.8f),
            specular = new(0.5f, 0.5f, 0.5f)
        };

        base.OnLoad();
    }

    Vector2 orthosize = new Vector2(20);
    Vector3 pos = new Vector3(-2.0f, 4.0f, -1.0f);
    float near = 1, far = 7.5f;
    float depthBias;

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        t += (float)args.Time;
        imGuiController.Update(this, (float)args.Time);
        Layout();

        HandleInput((float)args.Time);

        GL.Enable(EnableCap.CullFace);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.Multisample);

        // render to depth map
        GL.CullFace(CullFaceMode.Front);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, shadowFBO);
        GL.Viewport(0, 0, depthMapSize, depthMapSize);
        GL.ClearColor(new Color4(.1f, .1f, .12f, 1f));
        GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        
        Matrix4 lightProjection = Matrix4.CreateOrthographic(orthosize.X, orthosize.Y, near, far);
        Matrix4 lightView = Matrix4.LookAt(pos, Vector3.Zero, Vector3.UnitY);

        Matrix4 lightSpaceMatrix = lightView * lightProjection;
        directionalLight.direction = -pos.Normalized();
        depthShader.Use();
        depthShader.SetMatrix("lightSpaceMatrix", lightSpaceMatrix);
        RenderScene(depthShader);

        // render main scene
        GL.CullFace(CullFaceMode.Back);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.FramebufferSrgb);
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        GL.ClearColor(new Color4(.1f, .1f, .12f, 1f));
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        if (showShadowMap)
        {
            quadShader.Use();
            quadShader.SetMatrix("model", Matrix4.CreateScale(ClientSize.Y / (float)ClientSize.X, 1, 1));
            quadShader.SetMatrix("view", Matrix4.Identity);
            quadShader.SetMatrix("proj", Matrix4.Identity);
            quad.Draw(quadShader);
        }

        var viewRotMatrix = Matrix3.CreateRotationY(yr) * Matrix3.CreateRotationX(xr);
        Matrix4 view = Matrix4.LookAt(cameraPosition, cameraPosition + viewRotMatrix * Vector3.UnitZ, Vector3.UnitY);
        Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(zoom), ClientSize.X / (float)ClientSize.Y, .1f, 100f);

        litShader.Use();
        litShader.SetMatrix("view", view);
        litShader.SetMatrix("proj", proj);
        litShader.SetMatrix("lightSpaceMatrix", lightSpaceMatrix);
        litShader.SetTexture("depthMap", shadowDepthMap, 8);
        litShader.SetFloat("depthBias", depthBias);
        RenderScene(litShader);
        //litShader.SetMatrix("model", Matrix4.CreateTranslation(pos));
        //cube.Draw(litShader);

        GL.Disable(EnableCap.DepthTest);
        GL.Disable(EnableCap.Multisample);
        GL.Disable(EnableCap.FramebufferSrgb);
        
        imGuiController.Render();
        SwapBuffers();

        base.OnRenderFrame(args);
    }

    void RenderScene(Shader shader)
    {
        shader.Use();

        shader.SetFloat("time", t);
        shader.SetVector("uvScale", Vector2.One);
        shader.SetVector("viewPos", cameraPosition);
        shader.SetBool("renderNormals", renderNormals);

        directionalLight.Apply(shader, "directionalLight");

        for (int i = 0; i < pointLights.Length; i++)
        {
            var light = pointLights[i];
            light.Apply(shader, $"pointLights[{i}]");
        }

        for (int i = 0; i < spotLights.Length; i++)
        {
            var light = spotLights[i];
            light.Apply(shader, $"spotLights[{i}]");
        }

        shader.SetMatrix("model", Matrix4.CreateScale(.025f) * Matrix4.CreateTranslation(cubePosition));
        sponza.Draw(shader);

        shader.SetMatrix("model", Matrix4.CreateScale(15f) * Matrix4.CreateTranslation(0, 0, 0));
        planeMesh.Draw(shader);
    }

    private void HandleInput(float dt)
    {
        if (MouseState.IsButtonDown(MouseButton.Right))
        {
            xr -= MouseState.Delta.Y * 0.001f;
            yr += MouseState.Delta.X * 0.001f;

            xr = Math.Clamp(xr, -MathF.PI * .499f, MathF.PI * .499f);
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

        float zoomDelta = 0;
        if (KeyboardState.IsKeyDown(Keys.Minus))
            zoomDelta++;
        if (KeyboardState.IsKeyDown(Keys.Equal))
            zoomDelta--;

        zoom += zoomDelta * dt * 50;
        zoom = Math.Clamp(zoom, 1, 179);
    }

    private void Layout()
    {
        if (KeyboardState.IsKeyPressed(Keys.F1))
            imguiWindowOpen = !imguiWindowOpen;

        if (imguiWindowOpen && ImGui.Begin("debug", ref imguiWindowOpen))
        {
            //if (ImGui.Button("recompile shaders"))
            //{
            //    litShader?.Dispose();
            //    litShader = new("Shaders/Vertex/VertexPositionTextureNormalTangent.glsl", "Shaders/lit.glsl");
            //}

            ImGui.Checkbox("render normals", ref renderNormals);
            ImGui.Checkbox("show shadow depth map", ref showShadowMap);

            ImGui.DragFloat("depth bias", ref depthBias);
            ImGui.DragFloat("r", ref r);

            ImGui.DragFloat("near", ref near);
            near = MathF.Max(near, 0.001f);
            ImGui.DragFloat("far", ref far);
            ImGui.DragFloat2("orthosize", ref orthosize.ImGui());
            ImGui.DragFloat3("light pos", ref pos.ImGui());
            ImGui.DragFloat3("cube pos", ref cubePosition.ImGui());

            if (ImGui.BeginTabBar("tab bar"))
            {
                if (ImGui.BeginTabItem("shadow map"))
                {
                    ImGui.Image(shadowDepthMap, new(500, 500));
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("directional light"))
                {
                    if (ImGui.CollapsingHeader("directional light"))
                    {
                        ImGui.PushID("directional light");
                        directionalLight.Layout();
                        ImGui.PopID();
                    }
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("point lights"))
                {
                    for (int i = 0; i < pointLights.Length; i++)
                    {
                        ref var light = ref pointLights[i];
                        if (ImGui.CollapsingHeader("light " + i))
                        {
                            ImGui.PushID("light " + i);
                            light.Layout();
                            ImGui.PopID();
                        }
                    }
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("spot lights"))
                {
                    for (int i = 0; i < spotLights.Length; i++)
                    {
                        ref var light = ref spotLights[i];
                        if (ImGui.CollapsingHeader("spotlight " + i))
                        {
                            ImGui.PushID("spotlight " + i);
                            light.Layout();
                            ImGui.PopID();
                        }
                    }
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("sponza"))
                {
                    sponza?.Layout();
                }

                if (ImGui.BeginTabItem("brick wall"))
                {
                    planeMesh?.Layout();
                }

                if (ImGui.BeginTabItem("backpack"))
                {
                    backpack?.Layout();
                }

                ImGui.EndTabBar();
            }
        }
    }

    protected override void OnUnload()
    {
        cube?.Dispose();
        lampShader?.Dispose();
        litShader?.Dispose();

        imGuiController.Dispose();

        base.OnUnload();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
    }

    private unsafe void DebugMessage(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, nint message, nint userParam)
    {
        if (severity is DebugSeverity.DebugSeverityNotification)
            return;

        var msg = new string((sbyte*)message, 0, length);
        Console.WriteLine($"{source}: {msg}");
    }

    private static NativeWindowSettings GetNativeWindowSettings()
    {
        var settings = NativeWindowSettings.Default;
        settings.Flags |= ContextFlags.Debug;
        settings.APIVersion = new(4, 1);
        settings.NumberOfSamples = 16;
        return settings;
    }

    public Window() : base(GameWindowSettings.Default, GetNativeWindowSettings())
    {
    }
}
