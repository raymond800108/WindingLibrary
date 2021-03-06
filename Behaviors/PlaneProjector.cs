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
using WindingLibrary;

// </Custom "using" statements>


#region padding (this ensures the line number of this file match with those in the code editor of the C# Script component
















#endregion

public partial class PlaneProjector : GH_ScriptInstance
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


    private void RunScript(DataTree<Plane> iWrap, DataTree<Plane> iAllPlanes, DataTree<Plane> iTravelPlanes, ref object oProjectedPlanes, ref object oDebug, ref object oDebugB)
    {
        // <Custom code>
        
        DataTree<int> iWrapCounts = new DataTree<int>();
        for (int i = 0; i < iWrap.BranchCount; i++)
        {
            GH_Path pth = new GH_Path(i);
            iWrapCounts.Add(iWrap.Branch(i).Count, pth);
        }

        DataTree<int> iTravelCounts = new DataTree<int>();
        for (int i = 0; i < iTravelPlanes.BranchCount; i++)
        {
            GH_Path pth = new GH_Path(i);
            iTravelCounts.Add(iTravelPlanes.Branch(i).Count, pth);
        }

        DataTree<int> iSelectionIndexes = new DataTree<int>();
        for (int i = 0; i < iTravelCounts.BranchCount; i++)
        {
            GH_Path pth = new GH_Path(i);
            for (int y = 0; y < iTravelCounts.Branch(i)[0]; y++)
            {
                iSelectionIndexes.Add(iWrapCounts.Branch(i)[0]+y, pth);
            }
        }

        DataTree<Plane> projectedPlanes = new DataTree<Plane>();
        for (int i = 0; i < iAllPlanes.BranchCount; i++)
        {
            GH_Path pth = new GH_Path(i);
            for (int y = 0; y < iSelectionIndexes.Branch(i).Count; y++)
            {
                Plane tempPlane = iAllPlanes.Branch(i)[iSelectionIndexes.Branch(i)[y]];
                Plane tempPlane2 = iTravelPlanes.Branch(i)[y];
                tempPlane2.Origin = tempPlane.Origin;
                projectedPlanes.Add(tempPlane2, pth);
                iAllPlanes.Branch(i)[iSelectionIndexes.Branch(i)[y]] = tempPlane2;
            }
        }

        oProjectedPlanes = iAllPlanes;
        oDebug = iWrapCounts;
        oDebugB = projectedPlanes;
        // </Custom code>
    }

    // <Custom additional code>
  
    // </Custom additional code>
}
