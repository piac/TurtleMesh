using System;
using System.Collections.Generic;
using System.Text;

namespace Turtle.Analysis
{
    public static class SpacialAnalysis
    {
        public static void ComputeBoundingBox(ITurtleMesh m, out ITurtleVertex min, out ITurtleVertex max)
        {
            var minV = new TurtleVertex(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            var maxV = new TurtleVertex(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

            for (int i = 0; i < m.VertexCount; i++)
            {
                var v = m.VertexAt(i);

                if (minV.X > v.X) minV.X = v.X;
                if (minV.Y > v.Y) minV.Y = v.Y;
                if (minV.Z > v.Z) minV.Z = v.Z;

                if (maxV.X < v.X) maxV.X = v.X;
                if (maxV.Y < v.Y) maxV.Y = v.Y;
                if (maxV.Z < v.Z) maxV.Z = v.Z;
            }

            min = minV;
            max = maxV;
        }

        public static ITurtleVertex ComputeFaceVertexAvarage(ITurtleMesh t, int faceIndex)
        {
            var f = t.FaceAt(faceIndex);

            double x = 0, y = 0, z = 0;
            for (int i = 0; i < f.EdgesVerticesCount; i++)
            {
                var v = t.VertexAt(f[i]);
                x += v.X;
                y += v.Y;
                z += v.Z;
            }
            double inv = 1.0 / f.EdgesVerticesCount;
            return new TurtleVertex(
                (float)(x * inv), (float)(y * inv), (float)(z * inv));
        }
    }
}
