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

public partial class WindingClassConstructor : GH_ScriptInstance
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


    private void RunScript(Polyline iPolyline, Surface iSrf, DataTree<Point3d> iAnchors, bool isBackSyntax, ref object oIsoCurves, ref object oWindingObjects, ref object oPlanes)

    {
        // <Custom code>

        List<WindingClass> windingPoints = new List<WindingClass>();
        List<Plane> windingPlanes = new List<Plane>();
        List<Curve> isoCurves = new List<Curve>();
        
        for (int i = 0; i < iPolyline.Count; i++)
        {
            WindingClass tempWC = new WindingClass(iPolyline, i, iSrf, isBackSyntax);
            windingPoints.Add(tempWC);
            IdentifyPinIndexes(tempWC, iAnchors);
        }

        foreach (WindingClass wC in windingPoints)
        {
            windingPlanes.Add(wC.basePlane);
            isoCurves.Add(wC.iso);

            Print(wC.edgeIndex.ToString());
        }

        oWindingObjects = windingPoints;
        oPlanes = windingPlanes;
        oIsoCurves = isoCurves;

        // </Custom code>
    }

    // <Custom additional code>
    public void IdentifyPinIndexes(WindingClass wC, DataTree<Point3d> anchors)
    {
        // Find pin index 

        double minDistancePin = double.MaxValue;
        for (var i = 0; i < anchors.Branch(wC.edgeIndex).Count; i++)
        {
            Point3d pt = wC.basePlane.Origin;
            double distancePin = pt.DistanceTo(anchors.Branch(wC.edgeIndex)[i]);
            if (distancePin < minDistancePin)
            {
                minDistancePin = distancePin;
                wC.pinIndex = i;
            }
        }

    }
    // </Custom additional code>
}
