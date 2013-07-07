using System;
using System.Collections.Generic;
using System.Text;

namespace Turtle
{
    /// <summary>
    /// Represents a face that is part of the mesh.
    /// </summary>
    public interface ITurtleFace
    {
        int this[int index] { get; set; }

        int EdgesVerticesCount { get; }
    }
}