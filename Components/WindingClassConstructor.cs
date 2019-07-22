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

public partial class WindingClassConstructor : GH_ScriptInstance
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


    private void RunScript(Polyline poly, Surface srf, double offset, ref object oIsoCurves, ref object oWindingPoints, ref object oPlanes)

    {
        // <Custom code>
        List<WindingClass> windingPoints = new List<WindingClass>();
        List<Plane> windingPlanes = new List<Plane>();
        List<Curve> curves = new List<Curve>();
        
        for (int i = 0; i < poly.Count; i++)
        {
            WindingClass tempWC = new WindingClass(poly, i, srf);
            windingPoints.Add(tempWC);
        }

        foreach (WindingClass wC in windingPoints)
        {
            windingPlanes.Add(wC.pln);
            curves.Add(wC._iso);
            Print(wC.frameIndex.ToString());
        }

        oWindingPoints = windingPoints;
        oPlanes = windingPlanes;
        oIsoCurves = curves;

        // </Custom code>
    }

    // <Custom additional code>
   
    // </Custom additional code>
}
