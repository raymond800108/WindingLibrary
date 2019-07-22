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

public partial class WindingAttackAngle : GH_ScriptInstance
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


    private void RunScript(List<System.Object> wp, List<double> ang, Point3d AxisP1, Point3d AxisP2, ref object oWindingPoints, ref object planes)
    {
        // <Custom code>
        List<Plane> processedPlanes = new List<Plane>();
        List<WindingClass> windingObjects = new List<WindingClass>();
        for (var index = 0; index < wp.Count; index++)
        {
            WindingClass wC = (WindingClass) wp[index];
            wC.orientation = Orientation(wC);
            processedPlanes.Add(wC.orientation);
            windingObjects.Add(wC);
        }

        oWindingPoints = windingObjects;
        planes = processedPlanes;


        // </Custom code>
    }

    // <Custom additional code>
    Plane Orientation(WindingClass wp)
    {
        List<Curve> surfaceEdges = new List<Curve>();
        Brep closestBrep = wp.srf.ToBrep();
        surfaceEdges.AddRange(closestBrep.Curves3D);

        NurbsCurve edge = surfaceEdges[wp.frameIndex].ToNurbsCurve();

        NurbsCurve rebuilt = edge.Rebuild(250, 5, false);
        rebuilt.Domain = new Interval(0, 1);
        double param;
        rebuilt.ClosestPoint(wp.pln.Origin, out param);

        Vector3d tangent = rebuilt.TangentAt(param);

        Plane tempPln;
        rebuilt.FrameAt(param, out tempPln);

        Vector3d perpVector = Vector3d.CrossProduct(tangent, tempPln.YAxis);

        Plane npln = new Plane(wp.pln.Origin, perpVector, tangent);

        //Plane npln = new Plane(wp.pln);
        //npln.Rotate(RhinoMath.ToRadians(180), npln.YAxis);
        //npln.Rotate(RhinoMath.ToRadians(180), npln.ZAxis);


        if (wp.frameIndex == 1)
        {
            if (param > 0.5)
            {
                npln.Rotate(RhinoMath.ToRadians(110), npln.ZAxis);
                npln.Rotate(RhinoMath.ToRadians(25), npln.XAxis);
            }
            else
            {
                npln.Rotate(RhinoMath.ToRadians(100), npln.ZAxis);
                npln.Rotate(RhinoMath.ToRadians(15), npln.XAxis);
            }
        }
        // Adjust these angles on edge 3! Very problematic at the moment
        else if (wp.frameIndex == 3)
        {
            npln.Rotate(RhinoMath.ToRadians(15), npln.XAxis); // Rotate out
            if (param > 0.5)
            {
                npln.Rotate(RhinoMath.ToRadians(15), npln.ZAxis);
                npln.Rotate(RhinoMath.ToRadians(5), npln.XAxis);


            }
            else
            {
                npln.Rotate(RhinoMath.ToRadians(70), npln.ZAxis);
                npln.Rotate(RhinoMath.ToRadians(55), npln.XAxis);


            }

        }
        else if(wp.frameIndex == 2)
        {
            npln.Rotate(RhinoMath.ToRadians(-90), npln.ZAxis);
            npln.Rotate(RhinoMath.ToRadians(-60), npln.XAxis);
            if (param > 0.5)
            {
                npln.Rotate(RhinoMath.ToRadians(25), npln.ZAxis);

            }
            else
            {
                npln.Rotate(RhinoMath.ToRadians(-25), npln.ZAxis);

            }

        }
        else if (wp.frameIndex == 0)
        {
            npln.Rotate(RhinoMath.ToRadians(90), npln.ZAxis);
            npln.Rotate(RhinoMath.ToRadians(90), npln.XAxis);
            if (param > 0.5)
            {
                npln.Rotate(RhinoMath.ToRadians(-25), npln.ZAxis);

            }
            else
            {
                npln.Rotate(RhinoMath.ToRadians(25), npln.ZAxis);

            }
        }

            return npln;
    }
    // </Custom additional code>
}
