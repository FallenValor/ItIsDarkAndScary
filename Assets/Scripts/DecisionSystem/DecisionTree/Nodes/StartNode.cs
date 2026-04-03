/*****************************************************************************
// File Name : StartNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : Defines the starting node of a decision tree.
*****************************************************************************/
using IDAS.Decisions;
using IDAS.Decisions.Tree;
using UnityEngine;
using XNode;

namespace IDAS
{
    [CreateAssetMenu(fileName = "StartNode", menuName = "Decision Tree/Start Node")]
    public class StartNode : DecisionNodeBase
    {
        public override void NodeAction(TreeTravelService treeTraveler)
        {
            throw new System.NotImplementedException();
        }
    }
}
