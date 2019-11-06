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

public partial class WindingBehaviour : GH_ScriptInstance
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


    private void RunScript(List<System.Object> iWindingObjects, Curve iWindingPolyline, ref object oWindingPoints, ref object oWrap, ref object oNoWrap)
    {
        // <Custom code>
        DataTree<Plane> wrap = new DataTree<Plane>();
        DataTree<Plane> noWrap = new DataTree<Plane>();
        List<WindingClass> windingObjects = new List<WindingClass>();

        BoundingBox polylineBox = iWindingPolyline.GetBoundingBox(true); //G

        Plane polyBasePlane = new Plane(polylineBox.PointAt(0.5, 0.5, 0.4), new Vector3d(0, 0, -1)); //G
        
        for (var index = 0; index < iWindingObjects.Count; index++)
        {
            GH_Path pth = new GH_Path(index);
            WindingClass wC = (WindingClass)iWindingObjects[index];

            int loopDir = CheckWindingSide(iWindingObjects, index);
            wC.windingPath = CreateWindingPath(wC, iWindingPolyline, polyBasePlane, loopDir);
            wC.transitionPath = GenerateTransitionPath(wC);

            //wrap.AddRange(wC.transitionPath, pth);

            wrap.AddRange(wC.windingPath, pth);
            noWrap.Add(wC.attackAngle, pth);
            windingObjects.Add(wC);
        }

        oNoWrap = noWrap;
        oWrap = wrap;
        oWindingPoints = windingObjects;

        // </Custom code>
    }

    // <Custom additional code>
  
    int CheckWindingSide(List<object> windingObjects, int i)
    {
        // 0 - clockwise ; 1 - counter clockwise

        if (i == 0 || i == windingObjects.Count - 1) return 0; // knot
        else
        {
            double p1y = ((WindingClass)windingObjects[i - 1]).attackAngle.Origin.Y;
            double p2y = ((WindingClass)windingObjects[i + 1]).attackAngle.Origin.Y;
            if (p1y <= p2y) return 1;
            else return 0;
        }
    }

    List<Plane> CreateWindingPath(WindingClass wC, Curve windingPolyline, Plane polyBasePlane, int loopDirection)
    {
        List<Plane> behav = new List<Plane>();
        List<Point3d> corners = new List<Point3d>();

        Curve polylineToCopy = windingPolyline.DuplicateCurve();

        wC.windingCurvePlane = GenerateTempPlane(wC);
        Transform tr = Transform.PlaneToPlane(polyBasePlane, wC.windingCurvePlane);

        polylineToCopy.Transform(tr);
        if (wC.edgeIndex == 0 && (wC.pinIndex == 32 || wC.pinIndex == 33 || wC.pinIndex == 35 || wC.pinIndex == 31 || wC.pinIndex == 37))
        {
            Vector3d tfVec = (wC.windingCurvePlane.ZAxis*-80) + (wC.windingCurvePlane.XAxis*0);
            Transform tf = Transform.Translation(tfVec);
            polylineToCopy.Transform(tf);
        }

        if (wC.edgeIndex == 1 || wC.edgeIndex == 3)
        {
            Vector3d tfVec = (wC.windingCurvePlane.ZAxis * 20);
            Transform tf = Transform.Translation(tfVec);
            polylineToCopy.Transform(tf);
        }
        //  Mirror the Polyline if loopDirection is counter clockwise - 1
        if (loopDirection == 0)
        {
            Plane mirrorPlane = wC.windingCurvePlane;
            if (wC.edgeIndex == 0 || wC.edgeIndex == 2)
            {
                mirrorPlane.Rotate(RhinoMath.ToRadians(90), mirrorPlane.YAxis);
            }
            else if (wC.edgeIndex == 1 || wC.edgeIndex == 3)
            {
                mirrorPlane.Rotate(RhinoMath.ToRadians(90), mirrorPlane.YAxis);
            }

            Transform mirrTr = Transform.Mirror(mirrorPlane);
            polylineToCopy.Transform(mirrTr);
        }
        Curve[] seg = polylineToCopy.DuplicateSegments();
        Print(seg.Length.ToString());
        for (int i = 0; i < seg.Length; i++)
        {
            seg[i].Domain = new Interval(0, 1);
            corners.Add(seg[i].PointAt(0));
            if (i == (seg.Length - 1))
            {
                corners.Add(seg[i].PointAt(1));
            }
        }

        for (int i = 0; i < corners.Count; i++)
        {
            Plane pla = wC.attackAngle;
            pla.Origin = corners[i];
            behav.Add(pla);
        }

        List<Plane> transPls = GenerateTransitionPath(wC);
        behav.AddRange(transPls);
        if (wC.isVertical)
        {
            behav.Insert(0, transPls[0]);
        }
        return behav;
    }

    private static List<Plane> GenerateTransitionPath(WindingClass wC)
    {
        List<Plane> transitionPathPlanes = new List<Plane>();
        Plane transitionPlane = new Plane(wC.attackAngle);

        if (wC.edgeIndex == 0 || wC.edgeIndex == 2)
        {
            Transform trf = Transform.Translation(wC.windingCurvePlane.ZAxis*-150);
            transitionPlane.Transform(trf);
            transitionPathPlanes.Add(transitionPlane);
        }
        return transitionPathPlanes;
    }

    private static Plane GenerateTempPlane(WindingClass wC)
    {
        Plane tempPlane;
        if (wC.isVertical)
        {
            tempPlane = wC.basePlane;
            Transform downTrf = Transform.Translation(new Vector3d(tempPlane.ZAxis) * 90);
            tempPlane.Transform(downTrf);
            if (wC.edgeIndex == 0)
            {
                tempPlane.Rotate(RhinoMath.ToRadians(90), tempPlane.XAxis, tempPlane.Origin);
            }
            else
            {
                tempPlane.Rotate(RhinoMath.ToRadians(-90), tempPlane.XAxis, tempPlane.Origin);
            }
        }
        else
        {
            tempPlane = wC.basePlane;
            tempPlane.Rotate(RhinoMath.ToRadians(180), tempPlane.XAxis, tempPlane.Origin);
        }


        if (wC.edgeIndex == 2)
        {
            tempPlane.Rotate(RhinoMath.ToRadians(180), tempPlane.ZAxis);
            tempPlane.Rotate(RhinoMath.ToRadians(20), tempPlane.XAxis);
        }
        else if (wC.edgeIndex == 3)
        {
            tempPlane.Rotate(RhinoMath.ToRadians(90), tempPlane.ZAxis);
        }
        else if (wC.edgeIndex == 1)
        {
            tempPlane.Rotate(RhinoMath.ToRadians(-90), tempPlane.ZAxis);
            tempPlane.Rotate(RhinoMath.ToRadians(-5), tempPlane.XAxis);
        }
        else
        {
            tempPlane.Rotate(RhinoMath.ToRadians(20), tempPlane.XAxis);
        }

        return tempPlane;
    }

    // </Custom additional code>
}
