/****************************************************************************
// File Name : EndNode.cs
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
    [CreateAssetMenu(fileName = "EndNode", menuName = "Scriptable Objects/End Node")]
    public class EndNode : DarkScaryNode
    {
        [SerializeField, Input] private Choice inputChoice;

        /// <summary>
        /// When an end node is reached, end the current decision tree.
        /// </summary>
        /// <param name="treeTraveler"></param>
        public override void OnNodeEnter(TreeService treeTraveler)
        {
            // TODO: add tree ending implementation.
        }
    }
}
