using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rhino;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;


namespace WindingLibrary
{
    public class WindingClass
    {
        public int index;
        public int pinIndex;
        public bool isVertical = false;

        public Plane basePlane;
        public Plane attackAngle;
        public Plane windingCurvePlane;

        public int edgeIndex;
        public double edgeParam;
        public NurbsCurve edgeCurve;

        public Curve iso;
        public Surface srf;

        public List<Plane> windingPath = new List<Plane>();
        public List<Plane> travelPath = new List<Plane>();
        public List<Plane> transitionPath = new List<Plane>();

        // Deprecated?
        public int frameside;
        public double ft;
        


        public WindingClass(Polyline poly, int _index, Surface _srf, bool isBackSyntax)
        {
            index = _index;
            srf = _srf;
            Frame(poly[index], isBackSyntax);
        }

        void Frame(Point3d pt, bool isBackSyntax)
        {

        //  Find Plane Based on Surface
            double u1;
            double v1;
            srf.ClosestPoint(pt, out u1, out v1);
            srf.FrameAt(u1, v1, out basePlane);
            basePlane.Origin = pt;

        //  Extract Edges
            List<Curve> surfaceEdges = new List<Curve>();
            Brep srfBrep = srf.ToBrep();
            surfaceEdges.AddRange(srfBrep.Curves3D);

        //  Find which Edge point is on
            double minDistance = double.MaxValue;
            double tFinal = 0;
            for (var i = 0; i < surfaceEdges.Count; i++)
            {
                Curve crv = surfaceEdges[i];
                crv.Domain = new Interval(0, 1);
                double t;
                crv.ClosestPoint(pt, out t);
                double distance = pt.DistanceTo(crv.PointAt(t));
                if (distance < minDistance)
                {
                    edgeIndex = i;
                    minDistance = distance;
                    tFinal = t;
                }
            }

            //  Determine whether Frame lies on corner condition

            if (edgeIndex == 1 && tFinal > 0.99)
            {
                edgeIndex = 2;
            }
            else if (edgeIndex == 1 && tFinal < 0.01)
            {
                edgeIndex = 0;
            }
            else if (edgeIndex == 3 && tFinal > 0.99)
            {
                edgeIndex = 0;
            }
            else if (edgeIndex == 3 && tFinal < 0.01)
            {
                edgeIndex = 2;
            }
            
            
          

        //  Save location of plane on edge, and edge geometry
            surfaceEdges[edgeIndex].Domain = new Interval(0, 1);
            surfaceEdges[edgeIndex].ClosestPoint(basePlane.Origin, out edgeParam);
            edgeCurve = surfaceEdges[edgeIndex].ToNurbsCurve();

        //  Create IsoCurve for Debugging 
            if (edgeIndex == 0 || edgeIndex == 2)
            {
                iso = srf.IsoCurve(1, u1);
            }
            else
            {
                iso = srf.IsoCurve(0, v1);
            }


         // Orient Base Plane to IsoCurve
            //double param;
            //iso.ClosestPoint(pt, out param);
            //double paramAdd = 0.01 + param;
            //// find closest vector of iso curve
            //Point3d iso_pt1 = iso.PointAt(paramAdd);
            //Vector3d vectPts = pt - iso_pt1;
            ////Determine the angle between plane and vector for correction
            //double angles = Vector3d.VectorAngle(basePlane.YAxis, vectPts);
            //basePlane.Rotate(angles, basePlane.ZAxis);
            //// Flip it if its pointing out
            //if (edgeIndex == 3 || edgeIndex == 0)
            //{
            //    basePlane.Rotate(RhinoMath.ToRadians(180), basePlane.ZAxis);
            //}

        }
    }
}
