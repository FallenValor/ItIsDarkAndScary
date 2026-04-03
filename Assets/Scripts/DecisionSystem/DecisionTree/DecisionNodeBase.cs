/*****************************************************************************
// File Name : DarkScaryNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : base class for custom xNodes
*****************************************************************************/
using IDAS.Decisions.Tree;
using UnityEngine;
using XNode;

namespace IDAS
{
    public abstract class DecisionNodeBase : Node
    {
        #region CONSTS
        public const string CHOICE_PORT_NAME = "choices";
        #endregion

        [SerializeField, Output(dynamicPortList = true)] private Choice[] choices;

        #region Accessors
        public Choice[] Choices => choices;
        #endregion

        /// <summary>
        /// Handles getting a specific value from a port.
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public override object GetValue(NodePort port)
        {
            return base.GetValue(port);
        }

        /// <summary>
        /// Gets a node that this node transitions to by the index in the choices array.
        /// </summary>
        /// <param name="index">The index of the subsequent node to get.</param>
        /// <returns>The subsequent node.</returns>
        public Node GetDecisionNode(int index)
        {
            NodePort otherPort = GetPort(DecisionNodeBase.CHOICE_PORT_NAME + " " + index).Connection;
            if (otherPort != null)
            {
                return otherPort.node;
            }
            return null;
        }
    }
}