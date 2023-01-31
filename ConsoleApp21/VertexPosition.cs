using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace ConsoleApp21;

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
