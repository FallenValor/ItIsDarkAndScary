/*****************************************************************************
// File Name : DarkScaryNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/3/2026
// Last Modified : 4/4/2026
//
// Brief Description : Abstract base class for all xNodes that make up a decision tree.
*****************************************************************************/
using IDAS.Decisions;
using System;
using UnityEngine;
using XNode;

public abstract class DarkScaryNode : Node 
{
    [SerializeField, TextArea] private string flavor;

    /// <summary>
    /// Queues any behaviour that should trigger when this node is reached.
    /// </summary>
    /// <param name="treeTraveler">The TreeTravelerService scrip that is traversing the DecisionTree.</param>
    public abstract void OnNodeEnter(DecisionTreeService treeTraveler);

    /// <summary>
    /// Queues any behaviour that should trigger when this node is left.
    /// </summary>
    /// <param name="treeTraveler">The TreeTravelerService scrip that is traversing the DecisionTree.</param>
    public virtual void OnNodeExit(DecisionTreeService treeTraveler) { }

    /// <summary>
    /// Gets all nodes that this node transitions to as an array.
    /// </summary>
    /// <return>The array of nodes that this node transitions to.</return>
    public abstract DarkScaryNode[] GetAllNextNodes();
}