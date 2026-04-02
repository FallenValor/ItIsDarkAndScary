/*****************************************************************************
// File Name : DecisionNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : xNode node that represents a specific decision point in the decision tree.
*****************************************************************************/
using UnityEngine;
using XNode;

namespace IDAS.Decisions.Tree
{
    [CreateAssetMenu(fileName = "DecisionNode", menuName = "Decisions/DecisionNode")]
    public class DecisionNode : Node
    {
        [SerializeField, Output(dynamicPortList = true)] private Choice[] choices;

        /// <summary>
        /// Handles getting a specific value from a port.
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public override object GetValue(NodePort port)
        {
            return base.GetValue(port);
        }
    }

}