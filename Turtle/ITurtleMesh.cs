using System;
using System.Collections.Generic;
using System.Text;

namespace Turtle
{
    public interface ITurtleMesh
    {
        ITurtleFace FaceAt(int index);

        ITurtleVertex VertexAt(int index);

        int VertexCount { get; }

        int FaceCount { get; }
    }
}
