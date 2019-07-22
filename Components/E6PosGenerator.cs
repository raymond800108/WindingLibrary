using System;
using System.Collections;
using System.Collections.Generic;
using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

// <Custom "using" statements>

using System.Linq;
using Rhino.Geometry.Collections;
using Rhino.Geometry.Intersect;


// </Custom "using" statements>


#region padding (this ensures the line number of this file match with those in the code editor of the C# Script component
















#endregion

public partial class E6PosGenerator : GH_ScriptInstance
{
    #region Do_not_modify_this_region
    private void Print(string text) { }
    private void Print(string format, params object[] args) { }
    private void Reflect(object obj) { }
    private void Reflect(object obj, string methodName) { }
    public override void InvokeRunScript(IGH_Component owner, object rhinoDocument, int iteration, List<object> inputs, IGH_DataAccess DA) { }
    public RhinoDoc RhinoDocument;
    public GH_Document GrasshopperDocument;
    public IGH_Component Component;
    public int Iteration;
    #endregion


    private void RunScript(Plane source, Plane target, ref object XYZ, ref object ABC)

    {
        // <Custom code>
        Transform temp = Transform.PlaneToPlane(source, target);
        double C = RhinoMath.ToDegrees(Math.Atan2(temp.M21, temp.M22));
        double B = RhinoMath.ToDegrees(-Math.Atan2(temp.M20, Math.Sqrt((temp.M21*temp.M21 + temp.M22*temp.M22))));
        double A = RhinoMath.ToDegrees(Math.Atan2(temp.M10, temp.M00));
        ABC = new Vector3d(A, B, C);
        Point3d _XYZ;
        source.RemapToPlaneSpace(target.Origin, out _XYZ);
        XYZ = _XYZ;
        // </Custom code>
    }

    // <Custom additional code>

    // </Custom additional code>
}
