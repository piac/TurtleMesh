using System;
using Grasshopper.Kernel;
using Turtle;
using Turtle.Serialization;
using TurtleGh.Properties;

namespace TurtleGh
{
    public class ToObjTextComponent : GH_Component
    {
        public ToObjTextComponent()
            : base("To Obj Text", "ToObjT", "Gives the Obj representation of a TurtleMesh", "Mesh", "Turtle")
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
            pManager.AddTextParameter("Obj Text", "Obj", "The Obj text", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ITurtleMesh m = null;
            if (DA.GetData(0, ref m))
            {
                var r = Persistance.WritableText(m);

                DA.SetData(0, r);
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("058e8a9c-63b5-44f9-b0f2-7e4f97794e32"); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.turtle_toobj;
            }
        }
    }
}
