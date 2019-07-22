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


    private void RunScript(List<System.Object> wp, Interval height, Interval width, Interval length, Surface minsrf, ref object oWindingPoints, ref object oWrap, ref object oNoWrap)
    {
        // <Custom code>
        DataTree<Plane> wrap = new DataTree<Plane>();
        DataTree<Plane> noWrap = new DataTree<Plane>();

        List<WindingClass> windingObjects = new List<WindingClass>();
        for (var index = 0; index < wp.Count; index++)
        {
            GH_Path pth = new GH_Path(index);
            WindingClass wC = (WindingClass)wp[index];
            Plane pln = wC.orientation;
            Point3d origin = pln.Origin;
            double u;
            double v;
            wC.srf.ClosestPoint(origin, out u, out v);
            Point3d srfPt = wC.srf.PointAt(u, v);
            double dist2srf = origin.DistanceTo(srfPt);

            // Again same code in both if and else... -_-
            if (wC.frameIndex == 1)
            {
                wC.rect = Rect(wC, width, height, length);
                //for (int i = 0; i < wC.rect.Count; i++)
                //{
                //    wC.rect[i].Rotate(RhinoMath.ToRadians(90), wC.rect[i].ZAxis, wC.rect[i].Origin);
                //}
                    
            }
            else
            {
                wC.rect = Rect(wC, width, height, length);
                
            }
            wrap.AddRange(wC.rect, pth);
            noWrap.Add(wC.orientation, pth);
            windingObjects.Add(wC);
        }

        oNoWrap = noWrap;
        oWrap = wrap;
        oWindingPoints = windingObjects;

        // </Custom code>
    }

    // <Custom additional code>
    List<Plane> Rect(WindingClass wC, Interval width, Interval height, Interval length)
    {
        List<Plane> behav = new List<Plane>();
        Plane pln = wC.orientation;

        if (wC.frameIndex == 0 || wC.frameIndex == 2)
        {
            pln.Rotate(RhinoMath.ToRadians(-180), pln.XAxis);
        }
        else
        {
            pln.Rotate(RhinoMath.ToRadians(180), pln.XAxis);
        }

        // Create Box
        Brep box = Brep.CreateFromBox(new Box(pln,
            new BoundingBox(width.Min, length.Min, height.Min, width.Max, length.Max, height.Max)));
        List<Point3d> corners = new List<Point3d>()
        {
            box.Vertices[5].Location,
            box.Vertices[6].Location,
            box.Vertices[7].Location,
            box.Vertices[0].Location,
            box.Vertices[1].Location,
            box.Vertices[2].Location,
            box.Vertices[3].Location,
            box.Vertices[4].Location,

        };
        //for (int i = 0; i < box.Vertices.Count; i++)
        //{
        //    corners.Add(box.Vertices[i].Location);
        //}


        // Back of frame motion value
        //int backHook = 20;
        //Point3d movedCorner2 = corners[2];
        //movedCorner2 += (corners[5] - corners[1]) * backHook;

        //Point3d movedCorner3 = corners[3];
        //movedCorner3 += (corners[5] - corners[1]) * backHook;

        //Point3d movedCorner4 = corners[4];

        //// LOL same code in if else comeon
        //if (wC.frameIndex == 0 || wC.frameIndex == 2)
        //{
        //    movedCorner2 += (corners[5] - corners[1]) * -1 * backHook;
        //    movedCorner4 += (corners[1] - corners[2]) * -2 * backHook;
        //}
        //else
        //{
        //    movedCorner2 += (corners[5] - corners[1]) * -1 * backHook;
        //    movedCorner4 += (corners[1] - corners[2]) * -2 * backHook;
        //}

        //List<Point3d> selC = new List<Point3d>()
        //{
        //    corners[4],
        //    corners[0],
        //    corners[3],
        //    movedCorner3,
        //    movedCorner2,
        //    corners[2],
        //    corners[1],
        //    movedCorner4
        //};
        PolylineCurve polyC = new PolylineCurve(corners);

        // Box
        for (int i = 0; i < corners.Count; i++)
        {
            Plane pla = wC.orientation;
            pla.Origin = corners[i];
            behav.Add(pla);
        }

        return behav;

    }
    // </Custom additional code>
}
