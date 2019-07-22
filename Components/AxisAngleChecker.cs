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

public partial class AxisAngleChecker : GH_ScriptInstance
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


    private void RunScript(double Axis1, double Axis2, double Axis3, double Axis4, double Axis5, double Axis6, ref object Axis1Bool, ref object Axis2Bool, ref object Axis3Bool, ref object Axis4Bool, ref object Axis5Bool, ref object Axis6Bool, ref object masterColor)

    {
        // <Custom code>
        masterColor = 1;

        // Axis1: Range +/-185
        if (Axis1 >= -185 && Axis1 <= 185)
        {
            Axis1Bool = 1;
        }
        else
        {
            Axis1Bool = 0;
            masterColor = 0;
        }

        // Axis2: Range -140/-5
        if (Axis1 >= -140 && Axis1 <= -5)
        {
            Axis1Bool = 1;
        }
        else
        {
            Axis1Bool = 0;
            masterColor = 0;
        }

        // Axis3: Range -120/+155
        if (Axis1 >= -120 && Axis1 <= 155)
        {
            Axis1Bool = 1;
        }
        else
        {
            Axis1Bool = 0;
            masterColor = 0;
        }

        // Axis4: Range +/-350
        if (Axis1 >= -350 && Axis1 <= 350)
        {
            Axis1Bool = 1;
        }
        else
        {
            Axis1Bool = 0;
            masterColor = 0;
        }

        // Axis5: Range +/-122.5
        if (Axis1 >= -115 && Axis1 <= 115)
        {
            Axis1Bool = 1;
        }
        else
        {
            Axis1Bool = 0;
            masterColor = 0;
        }

        // Axis6: Range +/-350
        if (Axis1 >= -350 && Axis1 <= 350)
        {
            Axis1Bool = 1;
        }
        else
        {
            Axis1Bool = 0;
            masterColor = 0;
        }

        Print(masterColor.ToString());
        // </Custom code>
    }

    // <Custom additional code>
    
    // </Custom additional code>
}
