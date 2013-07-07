using System;
using System.Collections.Generic;
using System.Text;

namespace Turtle
{
    /// <summary>
    /// Represents an implementation of the TurtleMesh data interface.
    /// </summary>
    public class TurtleMesh : ITurtleMesh, ICloneable
    {
        public TurtleMesh() { }
        public TurtleMesh(ITurtleMesh other)
        {
            if (other == null) throw new ArgumentNullException("other");

            for (int i = 0; i < other.VertexCount; i++)
            {
                AddVertex(new TurtleVertex(other.VertexAt(i)));
            }

            for (int i = 0; i < other.FaceCount; i++)
            {
                AddFace(new TurtleFace(other.FaceAt(i)));
            }
        }

        List<TurtleFace> _faces = new List<TurtleFace>();
        List<TurtleVertex> _vertices = new List<TurtleVertex>();

        public void AddFace(TurtleFace face)
        {
            _faces.Add(face);
        }

        public TurtleFace FaceAt(int index)
        {
            return _faces[index];
        }

        ITurtleFace ITurtleMesh.FaceAt(int index)
        {
            return FaceAt(index);
        }

        public void AddVertex(TurtleVertex vertex)
        {
            _vertices.Add(vertex);
        }

        public void AppendOther(ITurtleMesh other)
        {
            int s = _vertices.Count;

            for (int i = 0; i < other.VertexCount; i++)
                _vertices.Add(new TurtleVertex(other.VertexAt(i)));

            for (int i = 0; i < other.FaceCount; i++)
            {
                var of = other.FaceAt(i);
                var nf = new TurtleFace(of.EdgesVerticesCount);
                for(int j=0; j<of.EdgesVerticesCount; j++)
                {
                    nf.Add(of[j] + s);
                }

                _faces.Add(nf);
            }
        }

        public TurtleVertex VertexAt(int index)
        {
            return _vertices[index];
        }

        ITurtleVertex ITurtleMesh.VertexAt(int index)
        {
            return VertexAt(index);
        }

        public int VertexCount
        {
            get { return _vertices.Count; }
        }

        public int FaceCount
        {
            get { return _faces.Count; }
        }

        public TurtleMesh Clone()
        {
            return new TurtleMesh(this);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public IEnumerable<TurtleVertex> GetVerticesEnumerable()
        {
            return _vertices;
        }

        public IEnumerable<TurtleFace> GetFacesEnumerable()
        {
            return _faces;
        }

        public bool IsValid
        {
            get
            {
                if (!(_faces.Count > 0) || !(_vertices.Count > 0))
                    return false;

                for (int i = 0; i < _faces.Count; i++)
                {
                    var f = _faces[i];

                    if (!f.IsValid)
                        return false;

                    for (int j = 0; j < f.EdgesVerticesCount; j++)
                    {
                        if (f[j] >= _vertices.Count)
                            return false;
                    }
                }

                return true;
            }
        }

        public override string ToString()
        {
            if (!IsValid) return "Invalid Ngonal TurtleMesh";

            return string.Format("TurtleMesh with {0} faces and {1} vertices", _faces.Count, _vertices.Count);
        }
    }
}