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


// </Custom "using" statements>


#region padding (this ensures the line number of this file match with those in the code editor of the C# Script component
















#endregion

public partial class WindingLibrary : GH_ScriptInstance
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


    private void RunScript(bool iReset, double iTimeline, List<double> iAxisValues, ref object Axis_1, ref object Axis_2, ref object Axis_3, ref object Axis_4, ref object Axis_5, ref object Axis_6)
    {
        // <Custom code>
        List<double> test = new List<double>();
        if (iTimeline < 1.0)
        {
            if (iReset == false)
            {
                Axis_1_List.Add(iAxisValues[0]);
                Axis_2_List.Add(iAxisValues[1]);
                Axis_3_List.Add(iAxisValues[2]);
                Axis_4_List.Add(iAxisValues[3]);
                Axis_5_List.Add(iAxisValues[4]);
                Axis_6_List.Add(iAxisValues[5]);
            }

            else if (iReset == true)
            {
                //Axis 1 
                Axis_1_List.Clear();
                Axis_1_List.Add(185);
                Axis_1_List.Add(-185);

                //Axis 2 
                Axis_2_List.Clear();
                Axis_2_List.Add(-5);
                Axis_2_List.Add(-140);

                //Axis 3 
                Axis_3_List.Clear();
                Axis_3_List.Add(155);
                Axis_3_List.Add(-120);

                //Axis 4 
                Axis_4_List.Clear();
                Axis_4_List.Add(350);
                Axis_4_List.Add(-350);

                //Axis 5 
                Axis_5_List.Clear();
                Axis_5_List.Add(122.5);
                Axis_5_List.Add(-122.5);

                //Axis 6 
                Axis_6_List.Clear();
                Axis_6_List.Add(350);
                Axis_6_List.Add(-350);
            }
        }
        else
        {
            test = Axis_1_List;
        }
        Axis_1 = Axis_1_List;
        Axis_2 = Axis_2_List;
        Axis_3 = Axis_3_List;
        Axis_4 = Axis_4_List;
        Axis_5 = Axis_5_List;
        Axis_6 = Axis_6_List;
        // </Custom code>

    }

    // <Custom additional code>
    List<int> counter = new List<int>();
    List<double> Axis_1_List = new List<double>();
    List<double> Axis_2_List = new List<double>();
    List<double> Axis_3_List = new List<double>();
    List<double> Axis_4_List = new List<double>();
    List<double> Axis_5_List = new List<double>();
    List<double> Axis_6_List = new List<double>();
    // </Custom additional code>
}
