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
using System.IO;

// </Custom "using" statements>


#region padding (this ensures the line number of this file match with those in the code editor of the C# Script component
















#endregion

public partial class KRLCodeGenerator : GH_ScriptInstance
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


    private void RunScript(string prog_name, string Home, DataTree<Point3d> XYZ, DataTree<Vector3d> ABC, DataTree<double> E2, DataTree<double> E1, DataTree<System.Object> Turn, double VEL_CP, double ACC_CP, int BASE, int TOOL, string loc, bool write, bool reset, ref object oCode)

    {
        // <Custom code>
        int VEL_PTP = 30;
        int APO_CVEL = 100;
        int APO_CDIS = 0;
        //int ADVANCE = 5;

        if (reset) _reset = 0; Print("File number reset");

        List<string> krlCode = new List<string>();

        krlCode.Add("DEF " + prog_name + _reset + "()");
        krlCode.Add(";------- Declaration section -------");
        krlCode.Add(";FOLD DECL");
        krlCode.Add("DECL E6AXIS HOME");
        krlCode.Add(";ENDFOLD");
        krlCode.Add(";------- Initialization ---------");
        //code.Add(";FOLD INIT (PTP: Vel = "+str(VEL_PTP)+"; LIN: Vel = "+str(VEL_CP)+", Acc = "+str(ACC_CP)+"; TOOL: "+str(TOOL)+"; BASE: "+str(BASE)+")")
        krlCode.Add("BAS (#INITMOV,0)");
        krlCode.Add("BAS(#VEL_PTP, "+VEL_PTP+")");
        krlCode.Add("$VEL.CP = " + VEL_CP);
        krlCode.Add("$ACC.CP = " + ACC_CP);
        krlCode.Add("$APO.CVEL = " + APO_CVEL);
        krlCode.Add("$APO.CDIS = " + APO_CDIS);
        krlCode.Add("$BASE = BASE_DATA[" + BASE + "]");
        krlCode.Add("$TOOL = TOOL_DATA[" + TOOL + "]");
        krlCode.Add("$ORI_TYPE = #JOINT");
        //code.Add("HOME = {A1 0,A2 -90,A3 90,A4 0,A5 30,A6 0, E1 0, E2 0, E3 0, E4 0, E5 0, E6 0}") #+Home+"}")
        krlCode.Add("HOME="+Home);
        //code.Add(";ENDFOLD")
        krlCode.Add(";----------- Main section ----------");
        //code.Add("PTP $POS_ACT")
        krlCode.Add("PTP HOME");

        for (int i = 0; i < XYZ.BranchCount; i++)
        {
            GH_Path path = XYZ.Path(i);
            krlCode.Add(";FOLD LIN P" + i + " (" + Math.Round((double)(i) * 100 / XYZ.BranchCount, 2) + "%)");
            for (int j = 0; j < XYZ.Branch(path).Count; j++)
            {
                Point3d _XYZ = XYZ.Branch(path)[j];
                Vector3d _ABC = ABC.Branch(path)[j];
                //double _E2 = E2.Branch(path)[j];
                double _E1 = E1.Branch(path)[j]; // fix positioner stuff!!!!!!
                // 2-Axis Rotation

                // 1-Axis Rotation
                string line = "LIN {E6POS: X " + Math.Round(_XYZ.X, 3) + ", Y " + Math.Round(_XYZ.Y, 3) +
                              ", Z " + Math.Round(_XYZ.Z, 3) + ", A " + Math.Round(_ABC.X, 3) + ", B " +
                              Math.Round(_ABC.Y, 3) + ", C " + Math.Round(_ABC.Z, 3) + ", E1 " +
                              Math.Round(_E1, 3) + ", E2 0.0, E3 0.0, E4 0.0, E5 0.0, E6 0.0}" + " C_VEL";

                krlCode.Add(line);              
            }
            krlCode.Add(";ENDFOLD");
        }
        krlCode.Add("END");
        oCode = krlCode;

        // Write
        if (write)
        {
            _reset += 1;
            string name = loc + "\\" + prog_name + _reset + ".src";
            using (StreamWriter outputFile = new StreamWriter(name))
            {
                foreach (string line in krlCode)
                {
                    outputFile.WriteLine(line);
                }
            }

            Print("File written to: " + name);
        }
        // </Custom code>
    }

    // <Custom additional code>
    private int _reset = 0;
    // </Custom additional code>
}
