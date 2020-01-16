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


    private void RunScript(List<string> iAxisSFile, List<string> iPointSFile, ref object AxisLine, ref object AxisPt,
        ref object Names, ref object XYZ, ref object ABC
        , ref object Status, ref object Turn, ref object EXAxis)
    {
        // <Custom code>
        //  ==============Axis survey datatree constructor  ==============
        DataTree<string> axisDataTree = new DataTree<string>(SurveyFileExtar(iAxisSFile, Text0));

        List<double> axisPointX = new List<double>();
        for (int i = 0; i < axisDataTree.BranchCount; i++)
        {
            axisPointX.Add(Convert.ToDouble(axisDataTree.Branch(i)[0]));
        }

        List<double> axisPointY = new List<double>();
        for (int i = 0; i < axisDataTree.BranchCount; i++)
        {
            axisPointY.Add(Convert.ToDouble(axisDataTree.Branch(i)[1]));
        }

        List<double> axisPointZ = new List<double>();
        for (int i = 0; i < axisDataTree.BranchCount; i++)
        {
            axisPointZ.Add(Convert.ToDouble(axisDataTree.Branch(i)[2]));
        }

        
        List<Point3d> axisSurveyPointList = new List<Point3d>();
        List<Point3d> leftPointListCir = new List<Point3d>();
        List<Point3d> rightPointListCir = new List<Point3d>();

        for (int i = 0; i < axisPointX.Count; i++)
        {
            axisSurveyPointList.Add(new Point3d(axisPointX[i], axisPointY[i], axisPointZ[i]));
        }

        for (int i = 0; i < axisSurveyPointList.Count; i++)
        {
            if (axisSurveyPointList[i].Y > 0)
                rightPointListCir.Add(axisSurveyPointList[i]);

            else
                leftPointListCir.Add(axisSurveyPointList[i]);
        }

        Circle rightCircle;
        Rhino.Geometry.Circle.TryFitCircleToPoints(rightPointListCir, out rightCircle);
        Circle leftCircle;
        Rhino.Geometry.Circle.TryFitCircleToPoints(leftPointListCir, out leftCircle);


        // ==============PinPoint survey datatree constructor ==============
        DataTree<string> pinDataTree = new DataTree<string>(SurveyFileExtar(iPointSFile, Text));

        List<double> point_X = new List<double>();
        for (int i = 0; i < pinDataTree.BranchCount; i++)
        {
            point_X.Add(Convert.ToDouble(pinDataTree.Branch(i)[0]));
        }

        List<double> point_Y = new List<double>();
        for (int i = 0; i < pinDataTree.BranchCount; i++)
        {
            point_Y.Add(Convert.ToDouble(pinDataTree.Branch(i)[1]));
        }

        List<double> point_Z = new List<double>();
        for (int i = 0; i < pinDataTree.BranchCount; i++)
        {
            point_Z.Add(Convert.ToDouble(pinDataTree.Branch(i)[2]));
        }
        
        List<Point3d> surveyPointList = new List<Point3d>();
        for (int i = 0; i < point_X.Count; i++)
        {
            surveyPointList.Add(new Point3d(point_X[i], point_Y[i], point_Z[i]));
        }

        List<double> point_A = new List<double>();
        for (int i = 0; i < pinDataTree.BranchCount; i++)
        {
            point_A.Add(Convert.ToDouble(pinDataTree.Branch(i)[3]));
        }

        List<double> point_B = new List<double>();
        for (int i = 0; i < pinDataTree.BranchCount; i++)
        {
            point_B.Add(Convert.ToDouble(pinDataTree.Branch(i)[4]));
        }

        List<double> point_C = new List<double>();
        for (int i = 0; i < pinDataTree.BranchCount; i++)
        {
            point_C.Add(Convert.ToDouble(pinDataTree.Branch(i)[5]));
        }
        
        List<Point3d> PointABCList = new List<Point3d>();
        for (int i = 0; i < point_A.Count; i++)
        {
            PointABCList.Add(new Point3d(point_A[i], point_B[i], point_C[i]));
        }

        List<double> statusList = new List<double>();
        for (int i = 0; i < pinDataTree.BranchCount; i++)
        {
            statusList.Add(Convert.ToDouble(pinDataTree.Branch(i)[6]));
        }

        List<double> turnList = new List<double>();
        for (int i = 0; i < pinDataTree.BranchCount; i++)
        {
            turnList.Add(Convert.ToDouble(pinDataTree.Branch(i)[7]));
        }

        DataTree<double> EXAxisTree = new DataTree<double>();
        for (int i = 0; i < pinDataTree.BranchCount; i++)
        {
            GH_Path pth = new GH_Path(i);
            EXAxisTree.Add(Convert.ToDouble(pinDataTree.Branch(i)[8]), pth);
            EXAxisTree.Add(Convert.ToDouble(pinDataTree.Branch(i)[9]), pth);
            EXAxisTree.Add(Convert.ToDouble(pinDataTree.Branch(i)[10]), pth);
            EXAxisTree.Add(Convert.ToDouble(pinDataTree.Branch(i)[11]), pth);
            EXAxisTree.Add(Convert.ToDouble(pinDataTree.Branch(i)[12]), pth);
            EXAxisTree.Add(Convert.ToDouble(pinDataTree.Branch(i)[13]), pth);
        }

        Line axisLine = new Line(leftCircle.Center, rightCircle.Center);
        AxisLine = axisLine;
        Vector3d rotateAxis = rightCircle.Center - leftCircle.Center;
        Point3d rotationCenter = new Point3d((leftCircle.Center.X + rightCircle.Center.X) * 0.5,
            (leftCircle.Center.Y + rightCircle.Center.Y) * 0.5, (leftCircle.Center.Z + rightCircle.Center.Z) * 0.5);
        List<Point3d> rotatedPointList = new List<Point3d>();

        for (int i = 0; i < surveyPointList.Count; i++)
        {
            rotatedPointList.Add(surveyPointList[i]);
            Transform rotationTrans = Transform.Rotation(EXAxisTree.Branch(i)[0], rotateAxis, rotationCenter);
            rotatedPointList[i].Transform(rotationTrans);
        }

        AxisPt = axisSurveyPointList;
        XYZ = surveyPointList;
        ABC = PointABCList;
        Status = statusList;
        Turn = turnList;
        EXAxis = EXAxisTree;

        // </Custom code>

    }

    // <Custom additional code>

    
    public string Text;
    public string Text0;

    //  ==============SurveyFileExtract function Start ==============
    public DataTree<string> SurveyFileExtar(List<string> text,string tt)
    {

        foreach (string t in text)
        {
            tt += t;
        }
        GH_String replacedFile = new GH_String(tt);
        var replacedScript = replacedFile.Value.Replace("DECL E6POS", "^$$$^");
        string[] spliter_0 = { "^" };
        GH_String replacedScript2 = new GH_String(replacedScript);
        string[] splitedArray;
        splitedArray = replacedScript2.Value.Split(spliter_0, StringSplitOptions.RemoveEmptyEntries);

        List<int> cullIndex = new List<int>();
        List<string> surveyPoints = new List<string>();
        for (int i = 0; i < splitedArray.Length; i++)
        {
            GH_String arrayToCull = new GH_String(splitedArray[i]);
            int cullndex = arrayToCull.Value.IndexOf("$$$");
            if (cullndex == 0)
            {
                surveyPoints.Add(splitedArray[i + 1]);
            }
            cullIndex.Add(cullndex);
        }

        // split each survey point detail data
        string[] spliter_1 = { "=" };
        List<string> tempList = new List<string>();
        for (int i = 0; i < surveyPoints.Count; i++)
        {
            GH_String splitDetailPointData = new GH_String(surveyPoints[i]);
            tempList.AddRange(splitDetailPointData.Value.Split(spliter_1, StringSplitOptions.RemoveEmptyEntries));
        }
        List<string> pointDetailDataList = new List<string>();
        List<string> pointNames = new List<string>();
        for (int i = 1; i < tempList.Count; i = i + 4)
        {
            pointDetailDataList.Add(tempList[i]);
        }

        for (int i = 0; i < tempList.Count; i = i + 4)
        {
            pointNames.Add(tempList[i]);
        }
        List<string> pointDataTempList_0 = new List<string>();
        for (int i = 0; i < pointDetailDataList.Count; i++)
        {
            GH_String splitDetailPointData = new GH_String(pointDetailDataList[i]);
            pointDataTempList_0.Add(splitDetailPointData.Value.Replace("{", ""));
        }
        string[] spliter_2 = { "}" };
        List<string> pointDataTempList_1 = new List<string>();
        for (int i = 0; i < pointDetailDataList.Count; i++)
        {
            GH_String splitDetailPointData = new GH_String(pointDataTempList_0[i]);
            pointDataTempList_1.AddRange(splitDetailPointData.Value.Split(spliter_2, StringSplitOptions.RemoveEmptyEntries));
        }

        List<string> pointDataTempList_2 = new List<string>();
        for (int i = 0; i < pointDataTempList_1.Count; i = i + 2)
        {
            pointDataTempList_2.Add(pointDataTempList_1[i]);
        }

        string[] spliter_3 = { " " };
        List<string> pointDataTempList_3 = new List<string>();
        for (int i = 0; i < pointDataTempList_2.Count; i++)
        {
            GH_String splitDetailPointData = new GH_String(pointDataTempList_2[i]);
            pointDataTempList_3.AddRange(splitDetailPointData.Value.Split(spliter_3, StringSplitOptions.RemoveEmptyEntries));
        }

        string[] spliter_4 = { "," };
        List<string> pointDataTempList_4 = new List<string>();
        for (int i = 0; i < pointDataTempList_3.Count; i++)
        {
            GH_String splitDetailPointData = new GH_String(pointDataTempList_3[i]);
            pointDataTempList_4.AddRange(splitDetailPointData.Value.Split(spliter_4, StringSplitOptions.RemoveEmptyEntries));
        }

        List<string> pointDataTempList_5 = new List<string>();
        for (int i = 1; i < pointDataTempList_4.Count; i = i + 2)
        {
            pointDataTempList_5.Add(pointDataTempList_4[i]);
        }

        DataTree<string> pointDataTree = new DataTree<string>();
        int count = 0;
        int branchIndex = 0;
        for (int i = 0; i < pointDataTempList_5.Count; i++)
        {
            GH_Path pth = new GH_Path(0, branchIndex);
            pointDataTree.Add(pointDataTempList_5[i], pth);
            count++;

            if (count >= 14)
            {
                count = 0;
                branchIndex++;
            }
        }
        return pointDataTree;
    }
    //  ==============SurveyFileExtract function End ==============



    // </Custom additional code>
}
