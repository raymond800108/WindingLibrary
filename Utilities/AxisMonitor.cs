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


    private void RunScript(double A1, double A2, double A3, double A4, double A5, double A6, double E1_Rail, double E2_Positioner, double iTimestamp, bool iReset, ref object axis1Bool, ref object axis2Bool, ref object axis3Bool, ref object axis4Bool, ref object axis5Bool, ref object axis6Bool, ref object railAxisBool, ref object positionerAxisBool, ref object oErrorAxisColorIndex, ref object oSingularityAlert, ref object oSingularityCount, ref object oAxisProbCount, ref object oErrorLog)

    {
        // <Custom code>
        double axis1Angle = A1;
        double axis2Angle = A2;
        double axis3Angle = A3;
        double axis4Angle = A4;
        double axis5Angle = A5;
        double axis6Angle = A6;
        double railAxisValue = E1_Rail;
        double positionerAxisVlaue = E2_Positioner;
        if (iReset)
        {
            singularityCount = 0;
            axisProbCount = 0;
            oProblemOutput.Clear();
        }
        // KUKA KR420 R3080
        // Color index: Axis1 ~ Axis6 = 0~5 , railAxis: 6 , positionerAxis: 7

        List<int> axisColorIndex = new List<int>();

        // Singularity Color index: Axis1 ~ Axis6 = 0~5 , railAxis: 6 , positionerAxis: 7

        List<string> SingularityAxisTest = new List<string>();


        //Axis 1
        if (-185 <= A1 && A1 <= 185)
        {
            axis1Bool = true;
        }
        else
        {
            axis1Bool = false;
            axisColorIndex.Add(0);
            axisProbCount++;
            oProblemOutput.Add("T: " + iTimestamp + " E: Axis 1");
        }

        //Axis 2
        if (-130 <= axis2Angle && axis2Angle <= 20)
        {
            axis2Bool = true;
        }
        else
        {
            axis2Bool = false;
            axisColorIndex.Add(1);
            axisProbCount++;
            oProblemOutput.Add("T: " + iTimestamp + " E: Axis 2");

        }

        //Axis 3
        if (-100 <= axis3Angle && axis3Angle <= 144)
        {
            axis3Bool = true;

            if (-1 <= axis3Angle && axis3Angle <= 1)
            {
                SingularityAxisTest.Add("A6, A3, A2 almost coplanar: Elbow Singularity Alert");
                singularityCount++;
                oProblemOutput.Add("T: " + iTimestamp + " E: Elbow Singularity");

            }
        }
        else
        {
            axis3Bool = false;
            axisColorIndex.Add(2);
            axisProbCount++;
            oProblemOutput.Add("T: " + iTimestamp + " E: Axis 3");

        }

        //Axis 4
        if (-350 <= axis4Angle && axis4Angle <= 350)
        {
            axis4Bool = true;
        }
        else
        {
            axis4Bool = false;
            axisColorIndex.Add(3);
            axisProbCount++;
            oProblemOutput.Add("T: " + iTimestamp + " E: Axis 4");

        }

        //Axis 5
        if (-120 <= axis5Angle && axis5Angle <= 120)
        {
            axis5Bool = true;
            if (-1 <= axis5Angle && axis5Angle <= 1)
            {
                SingularityAxisTest.Add("A6 align with A4: Wrist Singularity Alert");
                singularityCount++;
                oProblemOutput.Add("T: " + iTimestamp + " E: Wrist Axis Singularity");

            }
        }
        else
        {
            axis5Bool = false;
            axisColorIndex.Add(4);
            axisProbCount++;
            oProblemOutput.Add("T: " + iTimestamp + " E: Axis 5");
        }

        //Axis 6
        if (-350 <= axis6Angle && axis6Angle <= 350)
        {
            axis6Bool = true;
        }
        else
        {
            axis6Bool = false;
            axisColorIndex.Add(5);
            axisProbCount++;
            oProblemOutput.Add("T: " + iTimestamp + " E: Axis 6");

        }

        //Rail Axis, Unit:meter(0.0~11.0), not quite sure the rail type and distance limitation
        if (0 <= railAxisValue && railAxisValue <= 11.000)
        {
            railAxisBool = true;
        }
        else
        {
            railAxisBool = false;
            axisColorIndex.Add(6);
            axisProbCount++;
            oProblemOutput.Add("T: " + iTimestamp + " E: Rail Error");

        }

        //Positioner Axis
        if (-350 <= positionerAxisVlaue && positionerAxisVlaue <= +350)
        {
            positionerAxisBool = true;
        }
        else
        {
            positionerAxisBool = false;
            axisColorIndex.Add(7);
            axisProbCount++;
            oProblemOutput.Add("T: " + iTimestamp + " E: Positioner Error");

        }

        // Shoulder Singularity
        double ShoulderSingularity = axis2Angle + axis3Angle;
        if (-90 <= ShoulderSingularity && ShoulderSingularity <= -70)
        {
            SingularityAxisTest.Add("A1 almost align with A6: Shoulder Singularity Alert");
            singularityCount++;
            oProblemOutput.Add("T: " + iTimestamp + " E: Shoulder Singularity");

        }

        oErrorAxisColorIndex = axisColorIndex;
        oSingularityAlert = SingularityAxisTest;
        oSingularityCount = singularityCount;
        oAxisProbCount = axisProbCount;
        oErrorLog = oProblemOutput;

        // </Custom code>
    }

    // <Custom additional code>
    int singularityCount = 0;
    int axisProbCount = 0;
    List<string> oProblemOutput = new List<string>();
    // </Custom additional code>
}
