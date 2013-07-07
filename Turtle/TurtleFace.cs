using System;
using System.Collections.Generic;
using System.Text;

namespace Turtle
{
    /// <summary>
    /// Represents an implementation of mesh face that uses a list of indices.
    /// </summary>
    public class TurtleFace : ITurtleFace, ICloneable
    {
        List<int> _indices;

        public TurtleFace() : this(4){ }
        public TurtleFace(int a, int b, int c) { _indices = new List<int> { a, b, c }; }
        public TurtleFace(int a, int b, int c, int d) { _indices = new List<int> { a, b, c, d }; }
        public TurtleFace(IEnumerable<int> indices) { _indices = new List<int>(indices); }
        public TurtleFace(int expectedsize) { _indices = new List<int>(expectedsize); }
        public TurtleFace(TurtleFace other) { _indices = new List<int>(other._indices); }
        public TurtleFace(ITurtleFace other)
        {
            _indices = new List<int>(other.EdgesVerticesCount);
            for (int i = 0; i < other.EdgesVerticesCount; i++)
                _indices.Add(other[i]);
        }
        public int this[int index]
        {
            get
            {
                return _indices[index];
            }
            set
            {
                while (index >= _indices.Count)
                {
                    _indices.Add(-1);
                }
                _indices[index] = value;
            }
        }

        public int EdgesVerticesCount
        {
            get { return _indices.Count; }
        }

        public void Add(int index)
        {
            _indices.Add(index);
        }

        public TurtleFace Clone()
        {
            return new TurtleFace(this);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public bool IsValid
        {
            get
            {
                return _indices.Count > 0;
            }
        }
    }
}
