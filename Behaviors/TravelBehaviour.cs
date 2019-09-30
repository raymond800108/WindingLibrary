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
using Rhino.Display;
using Rhino.Geometry.Collections;
using Rhino.Geometry.Intersect;
using WindingLibrary;

// </Custom "using" statements>


#region padding (this ensures the line number of this file match with those in the code editor of the C# Script component
















#endregion

public partial class TravelBehaviour : GH_ScriptInstance
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


    private void RunScript(List<System.Object> iWindingObjects, double iVecAmp, Curve iAxis, List<Curve> iSyntaxCurves, bool iBackSyntax, ref object oWindingObjects, ref object iTravelPlanes, ref object iAllPlanes)
    {
        // <Custom code>
     
        DataTree<Plane> allPlanes = new DataTree<Plane>();
        DataTree<Plane> travelPlanes = new DataTree<Plane>();
        GH_Path pth = new GH_Path(0);
        List<WindingClass> windingObjects = new List<WindingClass>();


        for (var index = 0; index < iWindingObjects.Count-1; index++)
        {
            pth = new GH_Path(index);
            WindingClass wC = (WindingClass)iWindingObjects[index];
            wC.travelPath = CreateTravelPath(wC, (WindingClass) iWindingObjects[index + 1], iSyntaxCurves[index], iAxis, iVecAmp, iBackSyntax);
            travelPlanes.AddRange(wC.travelPath, pth);
            allPlanes.AddRange(wC.windingPath, pth);
            allPlanes.AddRange(wC.travelPath, pth);
            windingObjects.Add(wC);
        }

        // Deal with last winding plane since it dosn't have two neighbors
        WindingClass lastItem = (WindingClass) iWindingObjects[iWindingObjects.Count - 1];
        allPlanes.AddRange(lastItem.windingPath, pth);
        windingObjects.Add((WindingClass)windingObjects.Last());

        oWindingObjects = windingObjects;
        iTravelPlanes = travelPlanes;
        iAllPlanes = allPlanes;

        // </Custom code>
    }

    // <Custom additional code>
    List<Plane> CreateTravelPath(WindingClass wC, WindingClass nextWC, Curve curve, Curve axis, double VecAmp, bool isBackSyntax)
    {
        List<Plane> path = new List<Plane>();

        Point3d[] geoDiv;
        int divisionCount = 30;
        curve.DivideByCount(divisionCount, true, out geoDiv);
        curve.Domain = new Interval(0, 1);
        Point3d midPoint = axis.PointAt(0.55);

        for (int i = 0; i < geoDiv.Length - 1; i++)
        {
            double u;
            double v;
            wC.srf.ClosestPoint(geoDiv[i], out u, out v);
            Plane rpln;
            wC.srf.FrameAt(u, v, out rpln);
            double param;
            axis.ClosestPoint(geoDiv[i], out param);

          
            // Offset plane from surface
            Vector3d V = rpln.ZAxis;
            V.Unitize();
            Vector3d vec = V * VecAmp;
            rpln.Origin -= vec;

            // If is back syntax add vector that attracts the planes towards the middle
            //if (isBackSyntax || rpln.Origin.Y > 2000)
            //{
            //    Vector3d toMid = nextWC.basePlane.Origin - midPoint;
            //    Vector3d toMidNoZ = new Vector3d(toMid.X, toMid.Y, 0);

            //    Vector3d toMidCurrent = wC.basePlane.Origin - midPoint;
            //    Vector3d toMidNoZCurrent = new Vector3d(toMidCurrent.X, toMidCurrent.Y, 0);

            //    Vector3d localToMid = rpln.Origin - midPoint;
            //    Vector3d localToMidNoZ = new Vector3d(localToMid.X, localToMid.Y, 0);

            //    if (toMidNoZ.Length >= 1000)
            //    {
            //        if (toMidNoZCurrent.Length >= 1000 && wC.edgeIndex != nextWC.edgeIndex)
            //        {
            //            localToMidNoZ += Vector3d.ZAxis*-400;
            //            rpln.Origin -= localToMidNoZ * 0.75;
            //        }
            //        else if(wC.edgeIndex != nextWC.edgeIndex)
            //        {
            //            rpln.Origin -= localToMidNoZ * 0.5;
            //        }
            //    }
            //    else
            //    {
            //        rpln.Origin -= toMidNoZ * 0.3;
            //    }
                
            //}



            Plane xyPlane = Plane.WorldXY;
            xyPlane.Origin = rpln.Origin;
            double distance = wC.basePlane.Origin.DistanceTo(nextWC.basePlane.Origin);
            xyPlane.Rotate(RhinoMath.ToRadians(180), xyPlane.XAxis, xyPlane.Origin);
            xyPlane.Rotate(RhinoMath.ToRadians(-15), xyPlane.YAxis, xyPlane.Origin);

            // Rotate plane so robot is pulling fiber, reduces friction

            if (nextWC.basePlane.Origin.Y > wC.basePlane.Origin.Y)
            {

                xyPlane.Rotate(RhinoMath.ToRadians(-45), xyPlane.XAxis, xyPlane.Origin);
                if (xyPlane.Origin.Y > 2000)
                {
                    xyPlane.Rotate(RhinoMath.ToRadians(45), xyPlane.YAxis, xyPlane.Origin);
                }
                if (xyPlane.Origin.Y < -2000)
                {
                    xyPlane.Rotate(RhinoMath.ToRadians(95), xyPlane.YAxis, xyPlane.Origin);
                }
            }
            else
            {
                xyPlane.Rotate(RhinoMath.ToRadians(45), xyPlane.XAxis, xyPlane.Origin);
                if (xyPlane.Origin.Y < -2000)
                {
                    xyPlane.Rotate(RhinoMath.ToRadians(55), xyPlane.YAxis, xyPlane.Origin);
                }
                
            }
           
            if (i < 6 || i > divisionCount-6)
            {

            }
            else
            {
                
                path.Add(xyPlane);
            }

        }
        return path;
    }
    // </Custom additional code>
}
