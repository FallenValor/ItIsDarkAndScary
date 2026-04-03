/*****************************************************************************
// File Name : DarkScaryNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/3/2026
// Last Modified : 4/3/2026
//
// Brief Description : Abstract base class for all xNodes that make up a decision tree.
*****************************************************************************/
using IDAS.Decisions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public abstract class DarkScaryNode : Node 
{
    /// <summary>
    /// Queues any behaviour that should trigger when this node is reached.
    /// </summary>
    /// <param name="treeTraveler"></param>
    public abstract void NodeAction(TreeTravelService treeTraveler);
}