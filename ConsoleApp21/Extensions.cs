using Assimp;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace ConsoleApp21;

static class Extensions
{
    public static ref System.Numerics.Vector2 ImGui(ref this Vector2 vector)
    {
        return ref Unsafe.As<Vector2, System.Numerics.Vector2>(ref vector);
    }

    public static ref System.Numerics.Vector3 ImGui(ref this Vector3 vector)
    {
        return ref Unsafe.As<Vector3, System.Numerics.Vector3>(ref vector);
    }

    public static ref System.Numerics.Vector4 ImGui(ref this Vector4 vector)
    {
        return ref Unsafe.As<Vector4, System.Numerics.Vector4>(ref vector);
    }

    public static Vector3 AsVector3(this Vector3D vector)
    {
        return new(vector.X, vector.Y, vector.Z);
    }

    public static Vector3 AsVector3(this Color3D vector)
    {
        return new(vector.R, vector.G, vector.B);
    }

    public static ref System.Numerics.Matrix4x4 ImGui(ref this Matrix4 matrix)
    {
        return ref Unsafe.As<Matrix4, System.Numerics.Matrix4x4>(ref matrix);
    }
}
