using System;
using System.Collections.Generic;
using System.Text;

namespace Turtle
{
    /// <summary>
    /// Represents an implementation of the mesh vertex interface.
    /// </summary>
    public class TurtleVertex : ITurtleVertex, ICloneable
    {
        public TurtleVertex() { }
        public TurtleVertex(float x, float y, float z) { X = x; Y = y; Z = z; }
        public TurtleVertex(ITurtleVertex other) : this(other.X, other.Y, other.Z) { }

        public float CoordinateAt(int index)
        {
            switch (index)
            {
                case 0: return X;
                case 1: return Y;
                case 2: return Z;
                default: throw new ArgumentOutOfRangeException("index");
            }
        }

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public float Z
        {
            get;
            set;
        }

        public TurtleVertex Clone()
        {
            return new TurtleVertex(this);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
