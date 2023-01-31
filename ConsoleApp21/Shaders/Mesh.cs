using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp21.Shaders;
internal class Mesh<T> : IDisposable where T : IVertex
{
    private readonly T[] vertices;
    private readonly uint[] indices;
    private readonly Texture[] textures;

    public Mesh()
    {

    }

    public void Dispose()
    {
    }
}
