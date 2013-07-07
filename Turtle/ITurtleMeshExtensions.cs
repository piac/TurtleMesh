using System;
using System.Collections.Generic;
using System.Text;

namespace Turtle
{
    public static class ITurtleMeshExtensions
    {
        public static TurtleMesh ExportFaceAt(ITurtleMesh m, int index)
        {
            var nm = new TurtleMesh();

            var f = m.FaceAt(index);
            var nf = new TurtleFace(f.EdgesVerticesCount);

            for (int i = 0; i < f.EdgesVerticesCount; i++)
            {
                nm.AddVertex(new TurtleVertex(m.VertexAt(f[i])));
                nf.Add(i);
            }

            nm.AddFace(nf);

            return nm;
        }
    }
}
