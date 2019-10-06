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


    private void RunScript(List<System.Object> iWindingObjects, ref object oWindingObjects, ref object oPlanes)
    {
        // <Custom code>
        List<Plane> processedPlanes = new List<Plane>();
        List<WindingClass> windingObjects = new List<WindingClass>();
        for (var index = 0; index < iWindingObjects.Count; index++)
        {
            WindingClass wC = (WindingClass)iWindingObjects[index];
            wC.attackAngle = AdjustAttackAngle(wC);
            processedPlanes.Add(wC.attackAngle);
            windingObjects.Add(wC);
        }

        oWindingObjects = windingObjects;
        oPlanes = processedPlanes;


        // </Custom code>
    }

    // <Custom additional code>
    Plane AdjustAttackAngle(WindingClass wp)
    {

        Vector3d tangent = wp.edgeCurve.TangentAt(wp.edgeParam);

        Plane tempPln;
        wp.edgeCurve.FrameAt(wp.edgeParam, out tempPln);

        Vector3d perpVector = Vector3d.CrossProduct(tangent, tempPln.YAxis);

        Plane npln = new Plane(wp.basePlane.Origin, perpVector, tangent);


        if (wp.edgeIndex == 1)
        {
            npln.Rotate(RhinoMath.ToRadians(180), npln.ZAxis);
            if (wp.edgeParam > 0.5)
            {

                npln.Rotate(RhinoMath.ToRadians(-105), npln.YAxis);
                npln.Rotate(RhinoMath.ToRadians(-90), npln.ZAxis);
                npln.Rotate(RhinoMath.ToRadians(-70), npln.XAxis);
            }
            else
            {
                npln.Rotate(RhinoMath.ToRadians(-105), npln.YAxis);
                npln.Rotate(RhinoMath.ToRadians(-90), npln.ZAxis);
                npln.Rotate(RhinoMath.ToRadians(-70), npln.XAxis);
                npln.Rotate(RhinoMath.ToRadians(60), npln.ZAxis);
            }
        }

        else if (wp.edgeIndex == 3)
        {
            npln = new Plane(wp.basePlane);
            npln.Rotate(RhinoMath.ToRadians(180), npln.XAxis);

            npln.Rotate(wp.edgeParam > 0.5 ? RhinoMath.ToRadians(50) : RhinoMath.ToRadians(70), npln.ZAxis);
        }
        else if(wp.edgeIndex == 2)
        {
            npln.Rotate(RhinoMath.ToRadians(90), npln.ZAxis);
            npln.Rotate(RhinoMath.ToRadians(180), npln.YAxis);
            npln.Rotate(RhinoMath.ToRadians(-80), npln.XAxis);
            if (wp.edgeParam < 0.25)
            {
                npln.Rotate(RhinoMath.ToRadians(-60), npln.ZAxis);
            }
            else if (wp.edgeParam < 0.5)
            {
                npln.Rotate(RhinoMath.ToRadians(-25), npln.ZAxis);

            }
            else if (wp.edgeParam > 0.5)
            {
                npln.Rotate(RhinoMath.ToRadians(35), npln.ZAxis);
                npln.Rotate(RhinoMath.ToRadians(15), npln.XAxis);
            }
            //npln.Rotate(wp.edgeParam > 0.5 ? RhinoMath.ToRadians(65) : RhinoMath.ToRadians(-15), npln.ZAxis);
        }
        else if (wp.edgeIndex == 0)
        {
            npln.Rotate(RhinoMath.ToRadians(90), npln.ZAxis);
            npln.Rotate(RhinoMath.ToRadians(-90), npln.XAxis);
            npln.Rotate(wp.edgeParam > 0.5 ? RhinoMath.ToRadians(-25) : RhinoMath.ToRadians(25), npln.ZAxis);
        }

        // Check for vertical pins and reorient
        List<int> verticalPinsIds = new List<int>()
        {
            2,6,10,14,18,22,26,30,34,38   
        };
        if (wp.edgeIndex == 0)
        {
            bool isInList = verticalPinsIds.IndexOf(wp.pinIndex) != -1;
            if (isInList)
            {
                //npln.Rotate(RhinoMath.ToRadians(-110), npln.XAxis);
                //npln.Rotate(RhinoMath.ToRadians(180), npln.ZAxis);
                //if (wp.edgeParam < 0.3)
                //{
                //    npln.Rotate(RhinoMath.ToRadians(-45), npln.ZAxis);
                //    npln.Rotate(RhinoMath.ToRadians(-40), npln.YAxis);


                //}
                //else if (wp.edgeParam > 0.7)
                //{
                //    npln.Rotate(RhinoMath.ToRadians(45), npln.ZAxis);
                //    npln.Rotate(RhinoMath.ToRadians(-25), npln.XAxis);
                //    npln.Rotate(RhinoMath.ToRadians(35), npln.YAxis);
                //    npln.Rotate(RhinoMath.ToRadians(45), npln.ZAxis);
                //}
                //else
                //{
                //    npln.Rotate(RhinoMath.ToRadians(45), npln.ZAxis);
                //    npln.Rotate(RhinoMath.ToRadians(90), npln.YAxis);
                //    //npln.Rotate(RhinoMath.ToRadians(30), npln.XAxis);
                //    npln.Rotate(RhinoMath.ToRadians(45), npln.ZAxis);



                //}
                wp.isVertical = true;
            }
        }
        else if (wp.edgeIndex == 2)
        {
            bool isInList = verticalPinsIds.IndexOf(wp.pinIndex) != -1;
            if (isInList)
            {
                //npln.Rotate(RhinoMath.ToRadians(270), npln.YAxis);
                //npln.Rotate(RhinoMath.ToRadians(-90), npln.ZAxis);
                //if (wp.edgeParam < 0.3)
                //{
                //    npln.Rotate(RhinoMath.ToRadians(90), npln.YAxis);
                //    npln.Rotate(RhinoMath.ToRadians(-20), npln.ZAxis);
                //    npln.Rotate(RhinoMath.ToRadians(-40), npln.XAxis);



                //}
                //else if (wp.edgeParam > 0.7)
                //{
                //    npln.Rotate(RhinoMath.ToRadians(40), npln.ZAxis);
                //    npln.Rotate(RhinoMath.ToRadians(25), npln.YAxis);
                //    npln.Rotate(RhinoMath.ToRadians(-25), npln.XAxis);



                //}
                //else
                //{
                //    npln.Rotate(RhinoMath.ToRadians(-70), npln.XAxis);
                //    npln.Rotate(RhinoMath.ToRadians(45), npln.ZAxis);


                //}
                wp.isVertical = true;

            }
        }

        return npln;
    }
    // </Custom additional code>
}
