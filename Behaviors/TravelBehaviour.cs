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


    private void RunScript(List<System.Object> wp, double VecAmp, Curve axis, List<Curve> crvs, bool isBackSyntax, ref object A, ref object B, ref object C)
    {
        // <Custom code>
     
        List<WindingClass> windingObjects = new List<WindingClass>();
        DataTree<Plane> Pth = new DataTree<Plane>();
        DataTree<Plane> Move = new DataTree<Plane>();
        GH_Path pth = new GH_Path(0);
        Print(isBackSyntax.ToString());
        for (var index = 0; index < wp.Count-1; index++)
        {
            pth = new GH_Path(index);
            WindingClass wC = (WindingClass)wp[index];
            wC.path = Path(wC, (WindingClass) wp[index + 1], crvs[index], axis, VecAmp, isBackSyntax);
            Move.AddRange(wC.path, pth);
            Pth.AddRange(wC.rect, pth);
            Pth.AddRange(wC.path, pth);
            windingObjects.Add(wC);
        }

        // Double check this not sure if pth count is valid branch...
        WindingClass lastItem = (WindingClass) wp[wp.Count - 1];
        Pth.AddRange(lastItem.rect, pth);
        windingObjects.Add((WindingClass)windingObjects.Last());

        A = windingObjects;
        B = Move;
        C = Pth;

        //a = wp #class 
        //b = Move #just inbetween planes
        //c = Pth #path including wrapping planes 

        // </Custom code>
    }

    // <Custom additional code>
    List<Plane> Path(WindingClass wC, WindingClass nextWC, Curve curve, Curve axis, double VecAmp, bool isBackSyntax)
    {
        List<Plane> path = new List<Plane>();

        Point3d[] geoDiv;
        int divisionCount = 20;
        //curve.DivideByLength(130, true, out geoDiv);
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
            Point3d origin = axis.PointAt(param);

          
            // Offset plane from surface
            Vector3d V = rpln.ZAxis;
            //rpln.Origin = geoDiv[i];
            V.Unitize();
            Vector3d vec = V * VecAmp;
            rpln.Origin -= vec;

            // if is back syntax add vector that orients the planes towards the middle
            if (isBackSyntax)
            {

                Vector3d toMid = nextWC.pln.Origin - midPoint;
                Vector3d toMidNoZ = new Vector3d(toMid.X, toMid.Y, 0);

                Vector3d toMidCurrent = wC.pln.Origin - midPoint;
                Vector3d toMidNoZCurrent = new Vector3d(toMidCurrent.X, toMidCurrent.Y, 0);


                Vector3d localToMid = rpln.Origin - midPoint;
                Vector3d localToMidNoZ = new Vector3d(localToMid.X, localToMid.Y, 0);

                if (toMidNoZ.Length >= 1000)
                {
                    if (toMidNoZCurrent.Length >= 1000 && wC.frameIndex != nextWC.frameIndex)
                    {
                        localToMidNoZ += Vector3d.ZAxis*-400;
                        rpln.Origin -= localToMidNoZ * 0.75;
                    }
                    else if(wC.frameIndex != nextWC.frameIndex)
                    {
                        rpln.Origin -= localToMidNoZ * 0.5;
                    }
                }
                else
                {
                    rpln.Origin -= toMidNoZ * 0.3;
                }
                
            }

            Plane xyPlane = Plane.WorldXY;
            xyPlane.Origin = rpln.Origin;
            xyPlane.Rotate(RhinoMath.ToRadians(180), xyPlane.XAxis, xyPlane.Origin);
            xyPlane.Rotate(RhinoMath.ToRadians(-15), xyPlane.YAxis, xyPlane.Origin);

            // Rotate plane so robot is pulling fiber, reduces friction
            if (nextWC.pln.Origin.Y > wC.pln.Origin.Y)
            {
                Transform tf3 = Transform.Rotation(RhinoMath.ToRadians(-45), xyPlane.XAxis, xyPlane.Origin);
                xyPlane.Transform(tf3);


            }
            else
            {
                Transform tf3 = Transform.Rotation(RhinoMath.ToRadians(45), xyPlane.XAxis, xyPlane.Origin);
                xyPlane.Transform(tf3);

            }

            //xyPlane.Rotate(a, xyPlane.ZAxis, xyPlane.Origin);
            if (i < 4 || i > divisionCount-4)
            {

                //Plane firstPlanes = new Plane(wC.pln);
                //firstPlanes.Origin = xyPlane.Origin;
                //firstPlanes.Rotate(RhinoMath.ToRadians(180), firstPlanes.XAxis, firstPlanes.Origin);
                //path.Add(firstPlanes);
                //path.Add(xyPlane);

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
