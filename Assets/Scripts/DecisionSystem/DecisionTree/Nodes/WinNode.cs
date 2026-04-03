/****************************************************************************
// File Name : WinNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/2/2026
// Last Modified : 4/2/2026
//
// Brief Description : Defines the node that ends the decision tree.
*****************************************************************************/
using IDAS.Decisions;
using IDAS.Decisions.Tree;
using UnityEngine;
using XNode;

namespace IDAS
{
    [CreateAssetMenu(fileName = "WinNode", menuName = "Scriptable Objects/Win Node")]
    public class WinNode : DarkScaryNode
    {
        [SerializeField, Input] private Choice inputChoice;

        public override void NodeAction(TreeTravelService treeTraveler)
        {
            throw new System.NotImplementedException();
        }
    }
}
