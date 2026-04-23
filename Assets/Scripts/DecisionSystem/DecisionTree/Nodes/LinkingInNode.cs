/*****************************************************************************
// File Name : LinkingInNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/23/2026
// Last Modified : 4/23/2026
//
// Brief Description : In section of the linkg node that seamlessly connect two different parts of one 
decision tree for organization purposes.
*****************************************************************************/
using IDAS.Decisions;
using UnityEngine;

namespace IDAS
{
    [NodeTint("#a53284")]
    public class LinkingInNode : DarkScaryNode
    {
        [SerializeField] private LinkingOutNode node;

        /// <summary>
        /// Linking nodes always return the node that their respective out node links to as the next node.
        /// </summary>
        /// <returns></returns>
        public override DarkScaryNode GetNode()
        {
            return node.GetLinkedNode();
        }

        public override DarkScaryNode[] GetAllNextNodes()
        {
            return node.GetAllNextNodes();
        }

        public override void OnNodeEnter(DecisionTreeService treeTraveler) { } // This node should never be entered.
    }
}
