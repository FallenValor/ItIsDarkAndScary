/*****************************************************************************
// File Name : LinkingOutNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/23/2026
// Last Modified : 4/23/2026
//
// Brief Description : Out side of a linking node that seamlessly connect two different parts of one \
decision tree for organization purposes.
*****************************************************************************/
using IDAS.Decisions;
using IDAS.Decisions.Tree;
using UnityEngine;
using XNode;
using static XNode.Node;

namespace IDAS
{
    [NodeTint("#a53284")]
    public class LinkingOutNode : DarkScaryNode
    {
        [SerializeField, Output(backingValue = ShowBackingValue.Never)] private Choice outChoice;

        public DarkScaryNode GetLinkedNode()
        {
            return GetConnectedNode(nameof(outChoice));
        }
        public override DarkScaryNode[] GetAllNextNodes()
        {
            return GetLinkedNode().GetAllNextNodes();
        }


        /// <summary>
        /// Gets an out value based on port name.
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public override object GetValue(NodePort port)
        {
            switch (port.fieldName)
            {
                case nameof(outChoice):
                    return outChoice;
                default:
                    return base.GetValue(port);
            }
        }

        public override void OnNodeEnter(DecisionTreeService treeTraveler) { } // Do nothing when this node is entered.
    }
}
