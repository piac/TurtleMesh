using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;
using Turtle;
using Turtle.Analysis;

namespace TurtleGh
{
    public static class RhinoMeshSupport
    {
        public static Mesh ExportTriangolatedRhinoMesh(ITurtleMesh ngons)
        {
            Mesh mesh = new Mesh();
            var vs = mesh.Vertices;
            for (int i = 0; i < ngons.VertexCount; i++)
            {
                var v = ngons.VertexAt(i);
                vs.Add(v.X, v.Y, v.Z);
            }

            var fs = mesh.Faces;
            for (int i = 0; i < ngons.FaceCount; i++)
            {
                var f = ngons.FaceAt(i);

                switch (f.EdgesVerticesCount)
                {
                    case 3:
                        fs.AddFace(f[0], f[1], f[2]);
                        break;
                    case 4:
                        fs.AddFace(f[0], f[1], f[2], f[3]);
                        break;
                    default:
                        if(f.EdgesVerticesCount > 2)
                        {
                            var av = SpacialAnalysis.ComputeFaceVertexAvarage(ngons, i);
                            int newIndex = vs.Count;
                            vs.Add(av.X, av.Y, av.Z);
                            
                            for (int j = 0; j < f.EdgesVerticesCount; j++)
                            {
                                fs.AddFace(f[j], f[(j + 1) % f.EdgesVerticesCount], newIndex);
                            }
                        }
                        break;
                }
            }
            return mesh;
        }

        public static Polyline[] ExportRhinoPolylines(ITurtleMesh ngons)
        {
            var polylines = new Polyline[ngons.FaceCount];

            for (int i = 0; i < ngons.FaceCount; i++)
            {
                var f = ngons.FaceAt(i);
                Polyline p = new Polyline(f.EdgesVerticesCount + 1);

                for (int j = 0; j < f.EdgesVerticesCount; j++)
                {
                    var v = ngons.VertexAt(f[j]);
                    p.Add(v.X, v.Y, v.Z);
                }
                var closure = ngons.VertexAt(f[0]);
                p.Add(closure.X, closure.Y, closure.Z);

                polylines[i] = p;
            }
            return polylines;
        }

        public static TurtleMesh ExtractTMesh(Curve c)
        {
            var m = new TurtleMesh();
            Polyline pl;
            c.TryGetPolyline(out pl);

            TurtleFace f = new TurtleFace(pl.Count - 1);

            for (int j = 0; j < pl.Count - 1; j++)
            {
                var v = pl[j];
                f.Add(m.VertexCount);
                m.AddVertex(new TurtleVertex((float)v.X, (float)v.Y, (float)v.Z));
            }
            m.AddFace(f);
            return m;
        }

        public static ITurtleMesh ImportRhinoMesh(Mesh m)
        {
            var t = new TurtleMesh();

            var mv = m.Vertices;
            for (int i = 0; i < mv.Count; i++)
            {
                var v = mv[i];
                t.AddVertex(new TurtleVertex(v.X, v.Y, v.Z));
            }

            var mf = m.Faces;
            for (int i = 0; i < mf.Count; i++)
            {
                var f = mf[i];
                if(f.IsTriangle)
                    t.AddFace(new TurtleFace(f.A, f.B, f.C));
                else
                    t.AddFace(new TurtleFace(f.A, f.B, f.C, f.D));
            }

            return t;
        }
    }
}
