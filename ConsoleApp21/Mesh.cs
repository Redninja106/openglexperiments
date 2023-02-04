using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp21;
internal class Mesh<TVertex> : IDisposable where TVertex : struct, IVertex
{
    public readonly TVertex[] vertices;
    public readonly uint[]? indices;
    public readonly List<Texture> textures;
    private readonly int vao, vbo, ebo;

    public Mesh(TVertex[] vertices, uint[]? indices, Texture[] textures)
    {
        this.vertices = vertices;
        this.indices = indices;
        this.textures = new(textures);

        if (!this.textures.Any(t => t.Kind is TextureKind.Specular))
            this.textures.Add(Texture.SpecularAlways);

        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();

        GL.BindVertexArray(vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * TVertex.SizeInBytes, this.vertices, BufferUsageHint.StaticDraw);

        if (this.indices is not null)
        {
            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, this.indices.Length * sizeof(uint), this.indices, BufferUsageHint.StaticDraw);
        }

        TVertex.SetAttributes();
    }

    public void Draw(Shader shader)
    {
        // sampler name format - [kind]Map[i] - ex. diffuseMap1
        Dictionary<TextureKind, int> textureCounts = new();
        for (int i = 0; i < textures.Count; i++)
        {
            var texture = textures[i];

            if (!textureCounts.ContainsKey(texture.Kind))
            {
                textureCounts.Add(texture.Kind, 0);
            }

            var kind = texture.Kind.ToString().ToLower();
            var count = textureCounts[texture.Kind];

            texture.Apply(shader, $"material.{kind}Map{count}", i);

            textureCounts[texture.Kind]++;
        }

        GL.BindVertexArray(this.vao);

        if (indices is null)
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }
        else
        {
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        for (int i = 0; i < textures.Count; i++)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + i);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }

    public void Dispose()
    {
        GL.DeleteBuffer(vbo);
        
        if (indices is not null)
            GL.DeleteBuffer(ebo);

        GL.DeleteVertexArray(vao);
    }

    public void Layout()
    {
        if (ImGui.TreeNode("textures"))
        {
            foreach (var texture in textures)
            {
                ImGui.Columns(2);
                ImGui.Image(texture.TextureID, new(100, 100));
                ImGui.NextColumn();
                ImGui.Text(texture.Path ?? "");
                ImGui.Text(texture.Kind.ToString());
                ImGui.Columns(1);
            }
            ImGui.TreePop();
        }
    }

    public override string ToString()
    {
        return $"Mesh (diffuse: {textures.Count(t => t.Kind is TextureKind.Diffuse)}, normal: {textures.Count(t => t.Kind is TextureKind.Normal)}) ";
    }
}
