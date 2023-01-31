using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ConsoleApp21;

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
