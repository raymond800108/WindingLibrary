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


    private void RunScript(double iZAxis_Vec, Line iRotationAxisLine, List<Point3d> iSurveyPoints, DataTree<double> iRotationAngle, ref object oSurveyPoints)
    {
        // <Custom code>
        Point3d startPtOnLine = iRotationAxisLine.PointAt(1);
        Point3d endPtOnLine = iRotationAxisLine.PointAt(0);
        Vector3d rotateAxis = startPtOnLine - endPtOnLine;

        Point3d rotationCenter = new Point3d((startPtOnLine.X + endPtOnLine.X) * 0.5,
            (startPtOnLine.Y + endPtOnLine.Y) * 0.5, (startPtOnLine.Z + endPtOnLine.Z) * 0.5);
        for (int i = 0; i < iSurveyPoints.Count; i++)
        {
            Transform rotatTrans = Transform.Rotation(RhinoMath.ToRadians(iRotationAngle.Branch(i)[0]), rotateAxis, rotationCenter);
            Point3d pass = iSurveyPoints[i];
            pass.Transform(rotatTrans);
            Transform rotatTrans90Degree = Transform.Rotation(RhinoMath.ToRadians(-90), rotateAxis, rotationCenter);
            pass.Transform(rotatTrans90Degree);

            Transform ZAxisTrans = Transform.Translation(Vector3d.ZAxis * iZAxis_Vec);
            pass.Transform(ZAxisTrans);
            iSurveyPoints[i] = pass;
        }
        oSurveyPoints = iSurveyPoints;

        // </Custom code>

    }

    // <Custom additional code>
    


    // </Custom additional code>
}
