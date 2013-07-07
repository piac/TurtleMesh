using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Turtle.Serialization
{
    /// <summary>
    /// Allows to read and write an ngonal mesh from and to any file or stream.
    /// </summary>
    public static class Persistance
    {
        public static void Write(ITurtleMesh mesh, TextWriter sw)
        {
            sw.WriteLine("# OBJ file written by TurtleMesh");
            var ci = System.Globalization.CultureInfo.InvariantCulture;

            sw.WriteLine("# " + mesh.VertexCount.ToString(ci) + " vertices");

            for (int i = 0; i < mesh.VertexCount; i++)
            {
                var v = mesh.VertexAt(i);
                sw.Write("v ");
                sw.Write(v.X.ToString("r", ci));
                sw.Write(" ");
                sw.Write(v.Y.ToString("r", ci));
                sw.Write(" ");
                sw.WriteLine(v.Z.ToString("r", ci));
            }

            sw.WriteLine("# " + mesh.FaceCount.ToString(ci) + " faces");
            for (int i = 0; i < mesh.FaceCount; i++)
            {
                var f = mesh.FaceAt(i);
                sw.Write("f");

                for (int j = 0; j < f.EdgesVerticesCount; j++)
                {
                    sw.Write(" ");
                    sw.Write((f[j] + 1).ToString(ci));
                }
                sw.WriteLine();
            }
            sw.WriteLine("# end of OBJ file");

            sw.Flush();
            sw.Close();
        }

        public static void Write(ITurtleMesh mesh, string path)
        {
            using (var sw = new StreamWriter(path, false, Encoding.ASCII))
            {
                sw.AutoFlush = false;
                Write(mesh, sw);
            }
        }

        public static string WritableText(ITurtleMesh mesh)
        {
            using(StringWriter sw = new StringWriter())
            {
                Write(mesh, sw);
                return sw.ToString();
            }
        }

        static readonly char[] _separators = new char[] { ' ', '\t', '\0' };
        public static ITurtleMesh Read(TextReader sr)
        {
            TurtleMesh m = new TurtleMesh();

            var ci = System.Globalization.CultureInfo.InvariantCulture;
            
            int linecount = 1;

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.TrimStart(_separators).StartsWith("#", StringComparison.InvariantCultureIgnoreCase)) continue;

                var tokens = line.Split(_separators, StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length < 2) continue;

                var token0 = tokens[0].ToLowerInvariant();

                if (token0 == "v")
                {
                    if (tokens.Length < 4 || tokens.Length > 5)
                        throw new ArgumentOutOfRangeException(string.Format("Line {0} contains a vertex with number of coordinates different than 3 or 4.", linecount.ToString(ci)));

                    float x;
                    if(!float.TryParse(tokens[1], System.Globalization.NumberStyles.Any, ci, out x))
                        throw new ArgumentException(string.Format("Line {0} contains an invalid X coordinate.", linecount.ToString(ci)));

                    float y;
                    if (!float.TryParse(tokens[2], System.Globalization.NumberStyles.Any, ci, out y))
                        throw new ArgumentException(string.Format("Line {0} contains an invalid Y coordinate.", linecount.ToString(ci)));

                    float z;
                    if (!float.TryParse(tokens[3], System.Globalization.NumberStyles.Any, ci, out z))
                        throw new ArgumentException(string.Format("Line {0} contains an invalid Z coordinate.", linecount.ToString(ci)));

                    TurtleVertex v = new TurtleVertex(x, y, z);
                    m.AddVertex(v);
                }
                else if (token0 == "f")
                {
                    if (tokens.Length < 3)
                        throw new ArgumentOutOfRangeException(string.Format("Line {0} contains a face with less than three indices.", linecount.ToString(ci)));

                    var f = new TurtleFace();

                    for (int i = 1; i < tokens.Length; i++)
                    {
                        int loc;
                        if (!int.TryParse(tokens[i], System.Globalization.NumberStyles.Integer, ci, out loc))
                            throw new ArgumentException(string.Format("Line {0} contains an invalid face number in token {1}.", linecount.ToString(ci), (i - 1).ToString(ci)));

                        f.Add(loc - 1);
                    }

                    m.AddFace(f);
                }
                //else texture, else normal

                linecount++;
            }

            return m;
        }

        public static ITurtleMesh Read(string path)
        {
            using (var sr = new StreamReader(path, Encoding.ASCII))
            {
                return Read(sr);
            }
        }

        public static ITurtleMesh ReadText(string objmesh)
        {
            using (var sr = new StringReader(objmesh))
            {
                return Read(sr);
            }
        }
    }
}
