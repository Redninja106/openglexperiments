using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp21;
internal class Mesh<TVertex> : IDisposable where TVertex : struct, IVertex
{
    private readonly TVertex[] vertices;
    private readonly uint[] indices;
    private readonly Texture[] textures;
    private readonly int vao, vbo, ebo;

    public Mesh(TVertex[] vertices, uint[] indices, Texture[] textures)
    {
        this.vertices = vertices;
        this.indices = indices;
        this.textures = textures;

        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();
        ebo = GL.GenBuffer();

        GL.BindVertexArray(vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * TVertex.SizeInBytes, vertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        TVertex.SetAttributes();
    }

    public void Draw(Shader shader)
    {
        // sampler name format - [kind]Map[i] - ex. diffuseMap1
        Dictionary<TextureKind, int> textureCounts = new();
        for (int i = 0; i < textures.Length; i++)
        {
            var texture = textures[i];

            var kind = texture.Kind.ToString().ToLower();
            var count = textureCounts[texture.Kind];

            texture.Apply(shader, $"{kind}Map{count}", i);

            textureCounts[texture.Kind]++;
        }

        GL.BindVertexArray(this.vao);
        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    public void Dispose()
    {
        GL.DeleteBuffer(vbo);
        GL.DeleteBuffer(ebo);
        GL.DeleteVertexArray(vao);
    }
}
