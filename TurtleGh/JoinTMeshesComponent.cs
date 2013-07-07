using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Turtle;
using TurtleGh.Properties;

namespace TurtleGh
{
    public class JoinTMeshesComponent : GH_Component
    {
        public JoinTMeshesComponent()
            : base("Join Turtle Meshes", "JoinT", "Joins several TurtleMeshs into one TurtleMesh", "Mesh", "Turtle")
        { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            var p = new GH_TurtleMeshParam();
            p.Name = "TurtleMesh";
            p.NickName = "T";
            p.Access = GH_ParamAccess.list;
            pManager.AddParameter(p);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            var p = new GH_TurtleMeshParam();
            p.Name = "TurtleMesh";
            p.NickName = "T";
            pManager.AddParameter(p);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var list = new List<ITurtleMesh>();
            if (DA.GetDataList(0, list))
            {
                var nm = new TurtleMesh();

                for (int i = 0; i < list.Count; i++)
                {
                    if(list[i] != null)
                        nm.AppendOther(list[i]);
                }

                DA.SetData(0, nm);
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("b58e8a3c-63b5-42f9-b062-7e44e7924e54"); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.turtle_join;
            }
        }
    }
}
