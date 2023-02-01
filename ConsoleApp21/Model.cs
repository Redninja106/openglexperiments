using Assimp;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp21;
internal class Model
{
    private static readonly AssimpContext assimpContext = new();
    
    private readonly List<Mesh<VertexPositionTextureNormalTangent>> meshes = new();
    private readonly string directory;

    public PointLight[] PointLights { get; }
    public SpotLight[] SpotLights { get; }
    public DirectionalLight[] DirectionalLights { get; }

    public Model(string path)
    {
        var scene = assimpContext.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs);
        directory = Path.GetDirectoryName(path) ?? throw new Exception();

        LoadNode(scene.RootNode, scene);

        PointLights = GetPointLights(scene);
        SpotLights = GetSpotLights(scene);
    }

    private SpotLight[] GetSpotLights(Scene scene)
    {
        List<SpotLight> spotLights = new();
        foreach (var light in scene.Lights.Where(l => l.LightType is LightSourceType.Spot))
        {
            SpotLight s;
            s.outerAngle = light.AngleOuterCone;
            s.innerAngle = light.AngleInnerCone;
            s.direction = light.Direction.AsVector3();

            s.pointLight.position = new(light.Position.X, light.Position.Y, light.Position.Z);
            s.pointLight.color.ambient = light.ColorAmbient.AsVector3();
            s.pointLight.color.diffuse = light.ColorDiffuse.AsVector3();
            s.pointLight.color.specular = light.ColorSpecular.AsVector3();

            s.pointLight.constant = light.AttenuationConstant;
            s.pointLight.linear = light.AttenuationLinear;
            s.pointLight.quadratic = light.AttenuationQuadratic;

            spotLights.Add(s);
        }
        return spotLights.ToArray();
    }

    private PointLight[] GetPointLights(Scene scene)
    {
        List<PointLight> spotLights = new();
        foreach (var light in scene.Lights.Where(l => l.LightType is LightSourceType.Point))
        {
            PointLight p;
            p.position = new(light.Position.X, light.Position.Y, light.Position.Z);
            p.color.ambient = light.ColorAmbient.AsVector3();
            p.color.diffuse = light.ColorDiffuse.AsVector3();
            p.color.specular = light.ColorSpecular.AsVector3();
            
            p.constant = light.AttenuationConstant;
            p.linear = light.AttenuationLinear;
            p.quadratic = light.AttenuationQuadratic;

            spotLights.Add(p);
        }

        return spotLights.ToArray();
    }

    private DirectionalLight[] GetDirectionalLights(Scene scene)
    {
        List<DirectionalLight> spotLights = new();
        foreach (var light in scene.Lights.Where(l => l.LightType is LightSourceType.Directional))
        {

        }
        return spotLights.ToArray();
    }

    private void LoadNode(Node node, Scene scene)
    {
        foreach (var meshIndex in node.MeshIndices)
        {
            meshes.Add(LoadMesh(scene.Meshes[meshIndex], scene));
        }

        foreach (var child in node.Children)
        {
            LoadNode(child, scene);
        }
    }

    private Mesh<VertexPositionTextureNormalTangent> LoadMesh(Mesh mesh, Scene scene)
    {
        var vertices = new VertexPositionTextureNormalTangent[mesh.VertexCount];

        for (int i = 0; i < mesh.VertexCount; i++)
        {
            var position = mesh.Vertices[i];
            vertices[i].Position = new(position.X, position.Y, position.Z);

            var normal = mesh.Normals[i];
            vertices[i].Normal = new(normal.X, normal.Y, normal.Z);

            var uv = mesh.TextureCoordinateChannels[0][i];
            vertices[i].TexCoord = new(uv.X, uv.Y);

            if (mesh.HasTangentBasis)
            {
                var tangent = mesh.Tangents[i];
                vertices[i].Tangent = tangent.AsVector3();
            }
        }

        var indices = new uint[mesh.FaceCount * 3];
        for (int i = 0; i < mesh.FaceCount; i++)
        {
            var face = mesh.Faces[i];
            indices[i * 3 + 0] = (uint)face.Indices[0];
            indices[i * 3 + 1] = (uint)face.Indices[1];
            indices[i * 3 + 2] = (uint)face.Indices[2];
        }

        List<Texture> textures = new();
        if (mesh.MaterialIndex >= 0)
        {
            var material = scene.Materials[mesh.MaterialIndex];
            textures.AddRange(LoadTextures(material, TextureType.Diffuse));
            textures.AddRange(LoadTextures(material, TextureType.Specular));
            textures.AddRange(LoadTextures(material, TextureType.Normals));
        }

        return new Mesh<VertexPositionTextureNormalTangent>(vertices, indices, textures.ToArray());
    }

    IEnumerable<Texture> LoadTextures(Assimp.Material material, TextureType type)
    {
        TextureKind kind = type switch
        {
            TextureType.Diffuse => TextureKind.Diffuse,
            TextureType.Specular => TextureKind.Specular,
            TextureType.Emissive => TextureKind.Emission,
            TextureType.Normals => TextureKind.Normal,
        };

        for (int i = 0; i < material.GetMaterialTextureCount(type); i++)
        {
            if (material.GetMaterialTexture(type, i, out var texture))
            {
                yield return Texture.Load(kind, Path.Combine(directory, texture.FilePath));
            }
        }
    }

    public void Draw(Shader shader)
    {
        foreach (var mesh in meshes)
        {
            mesh.Draw(shader);
        }
    }

    public void Layout()
    {
        foreach (var mesh in meshes)
        {
            ImGui.PushID(mesh.GetHashCode());
            if (ImGui.CollapsingHeader("mesh"))
                mesh.Layout();
            ImGui.PopID();
        }
    }
}
