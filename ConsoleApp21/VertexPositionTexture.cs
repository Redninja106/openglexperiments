using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace ConsoleApp21;

struct VertexPositionTexture : IVertex
{
    public static int SizeInBytes => Unsafe.SizeOf<VertexPositionTexture>();

    public Vector3 Position;
    public Vector2 TexCoord;

    public VertexPositionTexture(Vector3 position, Vector2 texCoord)
    {
        Position = position;
        TexCoord = texCoord;
    }

    public static void SetAttributes()
    {
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, SizeInBytes, 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, SizeInBytes, Vector3.SizeInBytes);
        GL.EnableVertexAttribArray(1);
    }
}
