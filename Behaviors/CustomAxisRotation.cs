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
using WindingLibrary;


// </Custom "using" statements>


#region padding (this ensures the line number of this file match with those in the code editor of the C# Script component
















#endregion

public partial class CustomAxisRotation : GH_ScriptInstance
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


    private void RunScript(List<System.Object> iWP, ref object oAngles)

    {
        // <Custom code>
        List<WindingClass> windingObjects = new List<WindingClass>();
        DataTree<double> positionerAngles = new DataTree<double>();
        for (var index = 0; index < iWP.Count-1; index++)
        {
            GH_Path pth = new GH_Path(index);
            WindingClass wp = (WindingClass)iWP[index];

            List<Curve> surfaceEdges = new List<Curve>();
            Brep closestBrep = wp.srf.ToBrep();
            surfaceEdges.AddRange(closestBrep.Curves3D);
            NurbsCurve edge = surfaceEdges[wp.frameIndex].ToNurbsCurve();
            edge.Domain = new Interval(0, 1);
            double param;
            edge.ClosestPoint(wp.pln.Origin, out param);


            if (wp.frameIndex == 1)
            {
                if (param > 0.5)
                {
                    positionerAngles.Add(-195, pth);
                }
                else
                {
                    positionerAngles.Add(-215, pth);
                }
            }
            else if (wp.frameIndex == 3)
            {
                if (param > 0.5)
                {
                    positionerAngles.Add(-215, pth);
                }
                else
                {
                    positionerAngles.Add(-190, pth);
                }
            }
            else if (wp.frameIndex == 2)
            {
                positionerAngles.Add(-175, pth);


            }
            else if (wp.frameIndex == 0)
            {
                positionerAngles.Add(-215, pth);

            }

        }

        oAngles = positionerAngles;
        // </Custom code>
    }

    // <Custom additional code>

    // </Custom additional code>
}
