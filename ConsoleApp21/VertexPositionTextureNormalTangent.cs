using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp21;
internal struct VertexPositionTextureNormalTangent : IVertex
{
    public static int SizeInBytes => Unsafe.SizeOf<VertexPositionTextureNormalTangent>();

    public Vector3 Position;
    public Vector2 TexCoord;
    public Vector3 Normal;
    public Vector3 Tangent;
    public Vector3 Bitangent;

    public VertexPositionTextureNormalTangent(Vector3 position, Vector2 texCoord, Vector3 normal, Vector3 tangent, Vector3 bitangent)
    {
        Position = position;
        TexCoord = texCoord;
        Normal = normal;
        Tangent = tangent;
        Bitangent = bitangent;
    }

    public static void SetAttributes()
    {
        nint positionOffset = Marshal.OffsetOf<VertexPositionTextureNormalTangent>(nameof(Position));
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, SizeInBytes, positionOffset);
        GL.EnableVertexAttribArray(0);

        nint texCoordOffset = Marshal.OffsetOf<VertexPositionTextureNormalTangent>(nameof(TexCoord));
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, SizeInBytes, texCoordOffset);
        GL.EnableVertexAttribArray(1);

        nint normalOffset = Marshal.OffsetOf<VertexPositionTextureNormalTangent>(nameof(Normal));
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, SizeInBytes, normalOffset);
        GL.EnableVertexAttribArray(2);

        nint tangentOffset = Marshal.OffsetOf<VertexPositionTextureNormalTangent>(nameof(Tangent));
        GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, SizeInBytes, tangentOffset);
        GL.EnableVertexAttribArray(3);

        nint bitangentOffset = Marshal.OffsetOf<VertexPositionTextureNormalTangent>(nameof(Bitangent));
        GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, SizeInBytes, bitangentOffset);
        GL.EnableVertexAttribArray(4);
    }
}
