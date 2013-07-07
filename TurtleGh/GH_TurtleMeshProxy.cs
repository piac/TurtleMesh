using System;
using System.Collections.Generic;
using System.Text;
using Grasshopper.Kernel.Types;
using System.ComponentModel;

namespace TurtleGh
{
    public class GH_TurtleMeshProxy : GH_GooProxy<GH_TurtleMesh>
    {
        public GH_TurtleMeshProxy(GH_TurtleMesh obj)
            : base(obj)
        { }

        [Category("Reference"), Description("Object ID of referenced ngon Turtle mesh"), DisplayName("Object ID"), RefreshProperties(RefreshProperties.All)]
        public string ObjectID
        {
            get
            {
                if (Owner.IsReferencedGeometry)
                {
                    return string.Format("{0}", Owner.ReferenceID);
                }
                return "No reference";
            }
            set
            {
                if (Owner.IsReferencedGeometry)
                {
                    try
                    {
                        Guid new_id = new Guid(value);
                        this.Owner.ReferenceID = new_id;
                        this.Owner.ClearCaches();
                        this.Owner.LoadGeometry();
                    }
                    catch {}
                }
            }
        }

        [Category("Topology"), Description("Total number of faces in the Turtle mesh")]
        public int FaceCount
        {
            get
            {
                if (Owner.Value == null)
                {
                    return 0;
                }
                return Owner.Value.FaceCount;
            }
        }

        [Category("Topology"), Description("Total number of vertices in the Turtle mesh")]
        public int VertexCount
        {
            get
            {
                if (Owner.Value == null)
                {
                    return 0;
                }
                return Owner.Value.VertexCount;
            }
        }
    }
}
