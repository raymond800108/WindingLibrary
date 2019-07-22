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
        public Plane pln;
        public int frameIndex;
        public Plane orientation;
        public List<Plane> rect = new List<Plane>();
        public List<Plane> path = new List<Plane>();
        public int frameside;
        public double ft;
        public Surface srf;
        public Curve _iso;

        public WindingClass(Polyline poly, int _index, Surface _srf)
        {
            index = _index;
            srf = _srf;
            Frame(poly[index]);
        }

        void Frame(Point3d pt)
        {

            // Find which surface is the closest
            Point3d closestPoint;

            double u1;
            double v1;
            srf.ClosestPoint(pt, out u1, out v1);

            double u2;
            double v2;
            srf.ClosestPoint(pt, out u2, out v2);

            Point3d closestPt1 = srf.PointAt(u1, v1);
            Point3d closestPt2 = srf.PointAt(u2, v2);

            if (pt.DistanceTo(closestPt1) < pt.DistanceTo(closestPt2))
            {
                closestPoint = closestPt1;
                srf.FrameAt(u1, v1, out pln);
            }
            else
            {
                closestPoint = closestPt2;
                srf.FrameAt(u2, v2, out pln);
            }

            pln.Origin = pt;

            // Another closest point method (why?)
            double u3;
            double v3;
            srf.ClosestPoint(pt, out u3, out v3);

            // Extract Edges
            List<Curve> surfaceEdges = new List<Curve>();
            Brep closestBrep = srf.ToBrep();
            surfaceEdges.AddRange(closestBrep.Curves3D);

            // If closest edge is Y axis then IsoCurve(1, u3) else (0, v3)
            int edgeIndex = 0;
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

            if (edgeIndex == 2 && tFinal > 0.95)
            {
                frameIndex = 3;

            }
            else if (edgeIndex == 2 && tFinal < 0.05)
            {
                frameIndex = 1;
            }else if (edgeIndex == 0 && tFinal > 0.95)
            {
                frameIndex = 1;

            }
            else if (edgeIndex == 0 && tFinal < 0.05)
            {
                frameIndex = 3;

            }
            else
            {
                frameIndex = edgeIndex;

            }
            Curve iso;
            if (edgeIndex == 0 || edgeIndex == 2)
            {
                iso = srf.IsoCurve(1, u3);
            }
            else
            {
                iso = srf.IsoCurve(0, v3);
            }

            ;
            _iso = iso;


            double param;
            iso.ClosestPoint(pt, out param);
            double paramAdd = 0.01 + param;

            // find closest vector of iso curve
            Point3d iso_pt1 = iso.PointAt(paramAdd);
            Vector3d vectPts = pt - iso_pt1;
            //Determine the angle between plane and vector for correction
            double angles = Vector3d.VectorAngle(pln.YAxis, vectPts);
            pln.Rotate(angles, pln.ZAxis);

            // Check if y-axis is pointing in or out (Not working, use edge instead)

            // Flip it if its pointing out
            if (frameIndex == 3 || frameIndex == 0)
            {
                pln.Rotate(RhinoMath.ToRadians(180), pln.ZAxis);
            }

        }
    }
}
