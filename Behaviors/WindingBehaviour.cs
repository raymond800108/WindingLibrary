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
        Plane polyBasePlane = new Plane(polylineBox.PointAt(0.5, 0.5, 0), new Vector3d(0, 0, -1)); //G

        for (var index = 0; index < iWindingObjects.Count; index++)
        {
            GH_Path pth = new GH_Path(index);
            WindingClass wC = (WindingClass)iWindingObjects[index];

            //wC.windingPath = CreateWindingPath(wC, iWidth, iHeight, iLength);
            int loopDir = CheckWindingSide(iWindingObjects, index);
            wC.windingPath = CreateWindingPath2(wC, iWindingPolyline, polyBasePlane, loopDir);
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

    List<Plane> CreateWindingPath2(WindingClass wC, Curve windingPolyline, Plane polyBasePlane, int loopDirection)
    {
        List<Plane> behav = new List<Plane>();
        List<Point3d> corners = new List<Point3d>();

        Curve polylineToCopy = windingPolyline.DuplicateCurve();
        Transform tr = Transform.PlaneToPlane(polyBasePlane, wC.attackAngle);
        polylineToCopy.Transform(tr);

        // mirror the polyline if loopDirection is counter clockwise - 1 
        if (loopDirection == 1)
        {
            Plane mirrorPlane = wC.basePlane;
            if (wC.edgeIndex == 0 || wC.edgeIndex == 2) // side edges
            {
                mirrorPlane.Rotate(RhinoMath.ToRadians(90), mirrorPlane.YAxis);
            }
            else if (wC.edgeIndex == 1 || wC.edgeIndex == 3)
            {
                mirrorPlane.Rotate(RhinoMath.ToRadians(90), mirrorPlane.XAxis);
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

        return behav;
    }

    // </Custom additional code>
}
