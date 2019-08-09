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

public partial class Tool_Priority : GH_ScriptInstance
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


    private void RunScript(List<Vector3d> iVectors, Plane iRotationPlane, List<Plane> iTargetPlanes, ref object oAngle, ref object Debug)
    {
        // <Custom code>

        double referenceAngle = Vector3d.VectorAngle(iVectors[0], iTargetPlanes[0].YAxis, iRotationPlane);

        List<double> refAngles = new List<double>();
        if (iVectors.Count == iTargetPlanes.Count)
        {
            for (int i = 0; i < iVectors.Count-1; i++)
            {
                refAngles.Add(Vector3d.VectorAngle(iTargetPlanes[i].YAxis, iTargetPlanes[i + 1].YAxis, iRotationPlane));
            }
        }else if (iTargetPlanes.Count == 1)
        {
            for (int i = 0; i < iVectors.Count-1; i++)
            {
                refAngles.Add(0);
            }
        }
        else
        {
            Debug = "Data tress do not match!";
        }

        List<double> angles = new List<double>();
        for (int i = 0; i < iVectors.Count-1; i++)
        {
            double angle = Vector3d.VectorAngle(iVectors[i + 1], iVectors[i], iRotationPlane);
            angles.Add(minPI(angle));
        }

        List<double> sum = new List<double>();
        double total = minPI(referenceAngle);
        sum.Add(total);

        for (int i = 0; i < angles.Count; i++)
        {
            total += angles[i] + minPI(refAngles[i]);
            sum.Add(total);
        }

        oAngle = sum;


        // </Custom code>
    }

    // <Custom additional code>
    double minPI(double value)
    {
        if (value >= 0.5 * Math.PI)
        {
            value = value - 2 * Math.PI;
        }

        return value;
    }
    // </Custom additional code>
}
