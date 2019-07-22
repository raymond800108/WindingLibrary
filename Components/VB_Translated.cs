﻿using System;
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

public partial class VB_Translated : GH_ScriptInstance
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


    private void RunScript(List<Plane> N, List<double> E1, double E, double S, bool T, ref object A, ref object B, ref object C, ref object poly, ref object Debug)
    {
        // <Custom code>
        if (T)
        {
            _inc = E;
            _len = 0;
            _poly = new Polyline();
            _plane = N[0];
            _ex = RhinoMath.ToRadians(E1[0]);

        // Step 1: Get the length of the tool path
            foreach (Plane _pl in N)
            {
                _poly.Add(_pl.Origin);
            }

            _len = _poly.Length;
        }
        else
        {
            _inc += S / _len;
            if (_inc > 1)
            {
                _inc = 1;
            }else if (_inc < 0)
            {
                _inc = 0;
            }
        }

        // Step 2: Interpolate the plane based on where along the path we are
        Curve _crv = _poly.ToNurbsCurve();
        _pt = _crv.PointAtNormalizedLength(_inc);
        double _param = _poly.ClosestParameter(_pt);
        int _index = (int)Math.Floor(_param);
        Line _line = _poly.SegmentAt(_index);
        double _t = _line.ClosestParameter(_pt);
        Plane _P0 = N[_index];
        double _EX0 = E1[_index];

        Plane _P1;
        double _EX1;

        if (_index == _poly.SegmentCount)
        {
            _P1 = N[_index];
            _EX1 = E1[_index];
        }
        else
        {
            _P1 = N[_index + 1];
            _EX1 = E1[_index + 1];
        }

        Point3d _origin = _P0.Origin + _t * (_P1.Origin - _P0.Origin);
        Vector3d _xDir = _P0.XAxis + _t * (_P1.XAxis - _P0.XAxis);
        Vector3d _yDir = _P0.YAxis + _t * (_P1.YAxis - _P0.YAxis);
        _plane = new Plane(_origin, _xDir, _yDir);
        _ex = _EX0 + _t * minPI(_EX1 - _EX0);
        Print(minPI(_EX1 - _EX0).ToString());

        A = _plane;
        B = Math.Round(_inc * 100, 3);
        C = _ex;
        poly = _poly;

        // </Custom code>
    }

    // <Custom additional code>
    private static double _inc;
    private static double _len;
    private static Polyline _poly;
    private Point3d _pt;
    private Plane _plane;
    private double _ex;
    

    private double minPI(double val)
    {
        if (val >= 180.0)
        {
            val = val - 360;
        }else if (val <= -180.0)
        {
            val = val + 360;
        }

        return val;
    }
    // </Custom additional code>
}
