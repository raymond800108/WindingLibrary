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

public partial class WorkEnvelopeAttractor : GH_ScriptInstance
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


    private void RunScript(List<Plane> iPlanes, List<double> iAngles, Curve iAxis, Brep iWorkEnvelope, ref object oAttractedPlanes, ref object oProcessedAngles)
    {
        // <Custom code>
        List<Plane> pulledPlanes = new List<Plane>();
        List<double> processedAngles = new List<double>();
        List<Point3d> circles = new List<Point3d>();
        for (var index = 0; index < iPlanes.Count; index++)            
        {
            Plane pln = iPlanes[index];
            double angle = iAngles[index];
            if (iWorkEnvelope.IsPointInside(pln.Origin, 0.01, true))
            {
                pln.Origin = iWorkEnvelope.ClosestPoint(pln.Origin);
                Print(index.ToString());
                double newAngle;
                pln.Origin = adjustPlane(iAxis, pln, iWorkEnvelope, angle, out newAngle);
                angle = newAngle;
            }
            processedAngles.Add(angle);
            pulledPlanes.Add(pln);
        }

        oProcessedAngles = processedAngles;
        oAttractedPlanes = pulledPlanes;
        // </Custom code>
    }
     
    // <Custom additional code>
    Point3d adjustPlane(Curve axis, Plane plnToAdjust, Brep workEnvelope, double currentAngle, out double newAngle)
    {
        double param;
        axis.ClosestPoint(plnToAdjust.Origin, out param);
        Point3d pt = axis.PointAt(param);

        Circle circle = new Circle(new Plane(pt, new Vector3d(axis.PointAtEnd-axis.PointAtStart)), pt, pt.DistanceTo(plnToAdjust.Origin));

        Curve[] overlapCurves;
        Point3d[] intersectionPoints;
        Intersection.CurveBrep(circle.ToNurbsCurve(), workEnvelope, 0.01, out overlapCurves, out intersectionPoints);

        Vector3d vecA = new Vector3d(pt-plnToAdjust.Origin);
        Vector3d vecB = new Vector3d(pt-intersectionPoints[0]);

        newAngle = Vector3d.VectorAngle(vecA, vecB, new Plane(pt, new Vector3d(axis.PointAtEnd - axis.PointAtStart)));
        newAngle = RhinoMath.ToRadians(newAngle);
        newAngle = currentAngle + newAngle;

        return intersectionPoints[1];

    }   
    // </Custom additional code>
}
