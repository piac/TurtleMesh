using System;
using System.IO;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.DocObjects;
using Rhino.Geometry;
using Turtle;
using Turtle.Analysis;

namespace TurtleGh
{
    public class GH_TurtleMesh : GH_GeometricGoo<ITurtleMesh>,
        IGH_BakeAwareData, IGH_PreviewData, IGH_PreviewMeshData
    {
        Guid reference;
        BoundingBox _b = BoundingBox.Unset;
        Polyline[] _polylines;
        Mesh _mesh;

        public GH_TurtleMesh() : this(null)
        {
        }

        public GH_TurtleMesh(ITurtleMesh mesh)
        {
            m_value = mesh;

            ClearCaches();
        }

        public override ITurtleMesh Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
                ClearCaches();
            }
        }

        public override Guid ReferenceID
        {
            get
            {
                return reference;
            }
            set
            {
                reference = value;
            }
        }

        public override Rhino.Geometry.BoundingBox Boundingbox
        {
            get
            {
                if (m_value != null && !_b.IsValid)
                {
                    ITurtleVertex min, max;
                    SpacialAnalysis.ComputeBoundingBox(m_value, out min, out max);
                    _b = new BoundingBox(min.X, min.Y, min.Z, max.X, max.Y, max.Z);
                }
                return _b;
            }
        }

        public override IGH_GeometricGoo DuplicateGeometry()
        {
            if(m_value == null) return null;

            return new GH_TurtleMesh(m_value == null ? null : new TurtleMesh(m_value)) { ReferenceID = ReferenceID };
        }

        public override Rhino.Geometry.BoundingBox GetBoundingBox(Rhino.Geometry.Transform xform)
        {
            var b = Boundingbox;
            b.Transform(xform);
            return b;
        }

        public override IGH_GeometricGoo Morph(Rhino.Geometry.SpaceMorph xmorph)
        {
            if (m_value != null)
            {
                var m = new TurtleMesh(m_value);

                foreach (var v in m.GetVerticesEnumerable())
                {
                    Point3d p = new Point3d(v.X, v.Y, v.Z);
                    p = xmorph.MorphPoint(p);

                    v.X = (float)p.X;
                    v.Y = (float)p.Y;
                    v.Z = (float)p.Z;
                }

                return new GH_TurtleMesh(m);
            }
            else
                return new GH_TurtleMesh(null);
        }

        public override IGH_GeometricGoo Transform(Rhino.Geometry.Transform xform)
        {
            if (m_value != null)
            {
                var m = new TurtleMesh(m_value);

                foreach (var v in m.GetVerticesEnumerable())
                {
                    Point3f p = new Point3f(v.X, v.Y, v.Z);
                    p.Transform(xform);

                    v.X = p.X;
                    v.Y = p.Y;
                    v.Z = p.Z;
                }

                return new GH_TurtleMesh(m);
            }
            else
                return new GH_TurtleMesh(null);
        }

        public override string ToString()
        {
            if (m_value == null)
                return "<Null mesh>";
            else return m_value.ToString();
        }

        public override string TypeDescription
        {
            get { return "N-gonal Mesh provided by Turtle"; }
        }

        public override string TypeName
        {
            get { return "TurtleMesh"; }
        }

        public bool BakeGeometry(Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes att, out Guid obj_guid)
        {
            if (_polylines == null)
                ClearCaches();

            obj_guid = Guid.Empty;

            if (_polylines == null) return false;

            for (int i = 0; i < _polylines.Length; i++)
                doc.Objects.AddPolyline(_polylines[i]);

            return true;
        }



        #region IGH_PreviewData Members

        public BoundingBox ClippingBox
        {
            get { return Boundingbox; }
        }

        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            if (this.m_value == null || _polylines == null)
                return;

            if (args.Pipeline.SupportsShading)
            {
                var c = args.Material.Diffuse;
                c = System.Drawing.Color.FromArgb((int)(args.Material.Transparency * 255),
                    c);

                args.Pipeline.DrawMeshShaded(_mesh, args.Material);
            }
        }

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (this.m_value == null || _polylines == null)
                return;

            for (int i = 0; i < _polylines.Length; i++)
                args.Pipeline.DrawPolygon(_polylines[i], args.Color, false);
        }

        #endregion

        #region IGH_PreviewMeshData Members

        public void DestroyPreviewMeshes()
        {
            m_value = null;
        }

        public Mesh[] GetPreviewMeshes()
        {
            if (m_value == null)
            {
                _mesh = null;
                return null;
            }

            if (_mesh == null) _mesh = RhinoMeshSupport.ExportTriangolatedRhinoMesh(m_value);

            return new Mesh[]
            {
                _mesh,
            };
        }

        #endregion

        public override bool LoadGeometry(Rhino.RhinoDoc doc)
        {
            RhinoObject obj = doc.Objects.Find(ReferenceID);
            if (obj == null)
            {
                return false;
            }
            if (obj.Geometry.ObjectType == ObjectType.Curve)
            {
                var c = (Curve)obj.Geometry;

                m_value = RhinoMeshSupport.ExtractTMesh(c);
                ClearCaches();
                return true;
            }
            return false;
        }

        public override void ClearCaches()
        {
            //base.ClearCaches();

            if (m_value == null)
            {
                _polylines = null;
                _b = BoundingBox.Empty;
                _mesh = null;
            }
            else
            {
                _polylines = RhinoMeshSupport.ExportRhinoPolylines(m_value);

                _mesh = RhinoMeshSupport.ExportTriangolatedRhinoMesh(m_value);
            }
        }

        public override IGH_GooProxy EmitProxy()
        {
            return new GH_TurtleMeshProxy(this);
        }

        public override bool CastFrom(object source)
        {
            if(source == null)
            {
                m_value = null;
                ClearCaches();
                return true;
            }

            if (source is GH_GeometricGoo<Mesh>)
            {
                source = ((GH_GeometricGoo<Mesh>)source).Value;
            }
            else if (source is GH_GeometricGoo<Curve>)
            {
                source = ((GH_GeometricGoo<Curve>)source).Value;
            }

            if (source is ITurtleMesh)
            {
                m_value = source as ITurtleMesh;
                ClearCaches();
                return true;
            }
            else if (source is Mesh)
            {
                m_value = RhinoMeshSupport.ImportRhinoMesh((Mesh)source);
                ClearCaches();
                return true;
            }
            else if (source is Curve)
            {
                m_value = RhinoMeshSupport.ExtractTMesh((Curve)source);
                ClearCaches();
                return true;
            }
            else if (source is Grasshopper.Kernel.Types.GH_Curve)
            {
                m_value = RhinoMeshSupport.ExtractTMesh((Curve)source);
                ClearCaches();
                return true;
            }

            return base.CastFrom(source);
        }

        public override bool CastTo<Q>(out Q target)
        {
            if (typeof(Q) == typeof(Mesh) || typeof(Q) == typeof(GeometryBase))
            {
                target = (Q)(object)RhinoMeshSupport.ExportTriangolatedRhinoMesh(m_value);
                return true;
            }
            if (typeof(Q) == (typeof(GH_Mesh)))
            {
                target = (Q)(object)new GH_Mesh(RhinoMeshSupport.ExportTriangolatedRhinoMesh(m_value));
                return true;
            }
            if (typeof(Q) == typeof(ITurtleMesh))
            {
                target = (Q)(object)m_value;
                return true;
            }

            return base.CastTo<Q>(out target);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            var b = base.Read(reader);

            var t = reader.GetString("TurtleMesh");
            m_value = Turtle.Serialization.Persistance.Read(new StringReader(t));

            return b;
        }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if(m_value != null)
            {
                StringWriter sw = new StringWriter();
                Turtle.Serialization.Persistance.Write(m_value, sw);
                sw.Flush();
                var t = sw.ToString();

                writer.SetString("TurtleMesh", t);
            }

            return base.Write(writer);
        }

        public override object ScriptVariable()
        {
            return Value;
        }
    }
}
