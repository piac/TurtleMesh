using System;
using System.Collections.Generic;
using System.Text;

namespace Turtle
{
    /// <summary>
    /// Represents a three-dimentional vertex that can be access from a mesh.
    /// </summary>
    public interface ITurtleVertex
    {
        float CoordinateAt(int index);

        float X { get; }
        float Y { get; }
        float Z { get; }
    }
}