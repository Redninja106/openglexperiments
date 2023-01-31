using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ConsoleApp21;

struct VertexPositionTextureNormal : IVertex
{
    public static int SizeInBytes => Unsafe.SizeOf<VertexPositionTextureNormal>();

    public Vector3 Position;
    public Vector2 TexCoord;
    public Vector3 Normal;

    public VertexPositionTextureNormal(Vector3 position, Vector2 texCoord, Vector3 normal)
    {
        Position = position;
        TexCoord = texCoord;
        Normal = normal;
    }

    public static void SetAttributes()
    {
        nint positionOffset = Marshal.OffsetOf<VertexPositionTextureNormal>(nameof(Position));
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, SizeInBytes, positionOffset);
        GL.EnableVertexAttribArray(0);

        nint texCoordOffset = Marshal.OffsetOf<VertexPositionTextureNormal>(nameof(TexCoord));
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, SizeInBytes, texCoordOffset);
        GL.EnableVertexAttribArray(1);

        nint normalOffset = Marshal.OffsetOf<VertexPositionTextureNormal>(nameof(Normal));
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, SizeInBytes, normalOffset);
        GL.EnableVertexAttribArray(2);
    }
}