using System;
using System.Collections.Generic;
using System.Text;
using Grasshopper.Kernel;
using Turtle;
using Turtle.Serialization;
using TurtleGh.Properties;
using Grasshopper.Kernel.Attributes;
using System.Windows.Forms;
using System.IO;

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

        class SimpleChangeAttributes : GH_ComponentAttributes
        {
            public SimpleChangeAttributes(FromObjTextComponent cmp) : base(cmp) { }

            public override Grasshopper.GUI.Canvas.GH_ObjectResponse RespondToMouseDoubleClick(Grasshopper.GUI.Canvas.GH_Canvas sender, Grasshopper.GUI.GH_CanvasMouseEvent e)
            {
                ((FromObjTextComponent)Owner).OnImport(this, EventArgs.Empty);
                return base.RespondToMouseDoubleClick(sender, e);
            }
        }

        public override void CreateAttributes()
        {
            Attributes = new SimpleChangeAttributes(this);
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

        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);

            var item = menu.Items.Add("Load from .obj files...", null, OnImport);
            item.Font = new System.Drawing.Font(item.Font.FontFamily, item.Font.Size, System.Drawing.FontStyle.Bold);
            item.Enabled = Params.Input[0].SourceCount == 0;
        }

        private void OnImport(object obj, EventArgs e)
        {
            if(Locked) return;

            var param = (Grasshopper.Kernel.Parameters.Param_String)Params.Input[0];
            if (param.SourceCount != 0) return;

            bool doneSomething = false;

            var ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.Filter = "Obj File (*.obj)|*.obj|Any file (*.*)|*.*";
            ofd.FilterIndex = 0;
            ofd.Multiselect = true;
            if (OnPingDocument() != null)
            {
                var p = OnPingDocument().FilePath;
                if (!string.IsNullOrEmpty(p) && p.Trim().Length > 0)
                {
                    var dir = Path.GetDirectoryName(p);
                    if (Directory.Exists(dir))
                    {
                        ofd.InitialDirectory = dir;
                    }
                }
            }
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileNames != null)
                {
                    foreach (var file in ofd.FileNames)
                    {
                        if (!File.Exists(file)) continue;

                        try
                        {
                            if (!doneSomething)
                                param.PersistentData.Clear();
                            doneSomething = true;

                            var t = File.ReadAllText(file);
                            param.PersistentData.Append(new Grasshopper.Kernel.Types.GH_String(t));
                        }
                        catch (Exception ex)
                        {
                            if (MessageBox.Show(
                                    string.Format
                                        ("An error occurred when saving the file: \n\n{0}.\n\nContinue loading?",
                                        ex.Message),
                                    "Error loading one file", MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                                == DialogResult.No)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            if (doneSomething)
            {
                param.ExpireSolution(true);
            }
        }
    }
}
