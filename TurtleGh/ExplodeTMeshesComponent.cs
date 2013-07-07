using System;
using System.Collections.Generic;
using System.Text;
using Grasshopper.Kernel;
using Turtle;
using Turtle.Serialization;
using TurtleGh.Properties;

namespace TurtleGh
{
    public class ExplodeTMeshesComponent : GH_Component
    {
        public ExplodeTMeshesComponent()
            : base("Explode Turtle Meshes", "ExplT", "Explodes one TurtleMesh into several TurtleMeshs", "Mesh", "Turtle")
        { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            var p = new GH_TurtleMeshParam();
            p.Name = "TurtleMesh";
            p.NickName = "T";
            pManager.AddParameter(p);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            var p = new GH_TurtleMeshParam();
            p.Name = "TurtleMesh";
            p.NickName = "T";
            p.Access = GH_ParamAccess.list;
            pManager.AddParameter(p);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ITurtleMesh m = null;
            if (DA.GetData(0, ref m))
            {
                List<ITurtleMesh> l = new List<ITurtleMesh>();
                for (int i = 0; i < m.FaceCount; i++)
                {
                    l.Add(ITurtleMeshExtensions.ExportFaceAt(m, i));
                }

                DA.SetDataList(0, l);
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("1d8acbf0-ed65-43ad-8dad-7e7d52ec718a"); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.turtle_explode;
            }
        }
    }
}