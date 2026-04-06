/*****************************************************************************
// File Name : DarkScaryNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/4/2026
//
// Brief Description : base class for nodes that represent a player's decision.
*****************************************************************************/
using IDAS.Decisions;
using IDAS.Decisions.Tree;
using System;
using UnityEngine;
using System.Linq;
using XNode;

namespace IDAS
{
    public abstract class DecisionNodeBase : DarkScaryNode
    {
        #region CONSTS
        public const string CHOICE_PORT_NAME = "choices";
        #endregion

        [SerializeField, Output(dynamicPortList = true)] private Choice[] choices;

        #region Accessors
        public Choice[] Choices => choices;
        #endregion

        #region Get Value Implementation
        /// <summary>
        /// Handles getting a specific value from a port.
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public override object GetValue(NodePort port)
        {
            string[] names = port.fieldName.Split(null);
            // For dynamic port arrays.
            if (names.Length > 1)
            {
                switch (names[0])
                {
                    case "choices":
                        return GetChoiceByString(names[1]);
                }

            }
            // Handle normal ports
            else
            {

            }
            return null;
        }

        /// <summary>
        /// Gets the choice port from GetValue
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Choice GetChoiceByString(string indexStr)
        {
            int index = Int32.Parse(indexStr);
            return choices[index];
        }
        #endregion

        /// <summary>
        /// Convers the choices array into a node array.
        /// </summary>
        /// <returns></returns>
        public override DarkScaryNode[] GetAllNextNodes()
        {
            DarkScaryNode[] nodes = new DarkScaryNode[choices.Length];
            for(int i = 0; i < choices.Length; i++)
            {
                nodes[i] = GetDecisionNode(i);
            }
            return nodes;
        }

        /// <summary>
        /// Gets a node that this node transitions to by the index in the choices array.
        /// </summary>
        /// <param name="index">The index of the subsequent node to get.</param>
        /// <returns>The subsequent node.</returns>
        public DarkScaryNode GetDecisionNode(int index)
        {
            NodePort otherPort = GetPort(CHOICE_PORT_NAME + " " + index).Connection;
            if (otherPort != null)
            {
                return otherPort.node as DarkScaryNode;
            }
            return null;
        }

        /// <summary>
        /// Gets a decision 
        /// </summary>
        /// <param name="choiceName"></param>
        public int GetChoiceIndex(string choiceName)
        {
            return Array.FindIndex(choices, n => n.Name == choiceName);
        }

        /// <summary>
        /// Queues this node as a decision for the player to make when entered.
        /// </summary>
        /// <param name="treeTraveler">The TreeTravelerService scrip that is traversing the DecisionTree.</param>
        public override void OnNodeEnter(DecisionTreeService treeTraveler)
        {
            treeTraveler.QueueDecision(this);
        }
    }
}