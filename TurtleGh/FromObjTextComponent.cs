using System;
using System.Collections.Generic;
using System.Text;
using Grasshopper.Kernel;
using Turtle;
using Turtle.Serialization;
using TurtleGh.Properties;

namespace TurtleGh
{
    public class FromObjTextComponent : GH_Component
    {
        public FromObjTextComponent()
            : base("From Obj Text", "FromObjT", "Gives a TurtleMesh from the Obj representation", "Mesh", "Turtle")
        { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Obj Text", "Obj", "The Obj text", GH_ParamAccess.item);
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
            string m = null;
            if (DA.GetData(0, ref m))
            {
                var t = Persistance.ReadText(m);

                DA.SetData(0, t);
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("0d7790ab-a373-4213-a568-faf2062e8325"); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.turtle_fromobj;
            }
        }
    }
}
