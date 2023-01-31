using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace ConsoleApp21;

static class Extensions
{
    public static ref System.Numerics.Vector3 ImGui(ref this Vector3 vector)
    {
        return ref Unsafe.As<Vector3, System.Numerics.Vector3>(ref vector);
    }
}
