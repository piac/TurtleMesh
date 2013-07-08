using System;
using Grasshopper.Kernel;
using Turtle;
using Turtle.Serialization;
using TurtleGh.Properties;
using System.Windows.Forms;
using System.IO;
using Grasshopper.Kernel.Attributes;

namespace TurtleGh
{
    public class ToObjTextComponent : GH_Component
    {
        public ToObjTextComponent()
            : base("To Obj Text", "ToObjT", "Gives the Obj representation of a TurtleMesh", "Mesh", "Turtle")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            var p = new GH_TurtleMeshParam();
            p.Name = "TurtleMesh";
            p.NickName = "T";
            pManager.AddParameter(p);
        }

        class SimpleChangeAttributes : GH_ComponentAttributes
        {
            public SimpleChangeAttributes(ToObjTextComponent cmp) : base(cmp){}

            public override Grasshopper.GUI.Canvas.GH_ObjectResponse RespondToMouseDoubleClick(Grasshopper.GUI.Canvas.GH_Canvas sender, Grasshopper.GUI.GH_CanvasMouseEvent e)
            {
                ((ToObjTextComponent)Owner).OnExport(this, EventArgs.Empty);
                return base.RespondToMouseDoubleClick(sender, e);
            }
        }

        public override void CreateAttributes()
        {
            Attributes = new SimpleChangeAttributes(this);
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

        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);

            var item = menu.Items.Add("Save as .obj file...", null, OnExport);
            item.Font = new System.Drawing.Font(item.Font.FontFamily, item.Font.Size, System.Drawing.FontStyle.Bold);
            item.Enabled = Params.Output[0].VolatileDataCount > 0;
        }

        private void OnExport(object obj, EventArgs e)
        {
            if(Locked) return;

            var param = Params.Output[0];
            if (param.VolatileData == null) return;

            foreach (var data in param.VolatileData.AllData(true))
            {
                if (data == null) continue;
                var sdata = data.ScriptVariable();
                if (sdata == null) continue;

                var txt = sdata.ToString();

                try
                {
                    var sfd = new SaveFileDialog();
                    sfd.AddExtension = true;
                    sfd.Filter = "Obj File (*.obj)|*.obj|Any file (*.*)|*.*";
                    sfd.FilterIndex = 0;
                    sfd.OverwritePrompt = true;
                    if (OnPingDocument() != null)
                    {
                        var p = OnPingDocument().FilePath;
                        if (!string.IsNullOrEmpty(p) && p.Trim().Length > 0)
                        {
                            var dir = Path.GetDirectoryName(p);
                            if (Directory.Exists(dir))
                            {
                                sfd.InitialDirectory = dir;
                            }
                        }
                    }
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(sfd.FileName, txt);
                    }
                    else
                        break;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        string.Format
                            ("An error occurred when saving the file: {0}",
                            ex.Message
                        )
                    );
                }
            }
        }
    }
}
