/*****************************************************************************
// File Name : DecisionTree.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : xNode graph used to build out a decision tree.
*****************************************************************************/
using UnityEngine;
using XNode;

namespace IDAS.Decisions.Tree
{
    [CreateAssetMenu(fileName = "DecisionTree", menuName = "Decisions Tree/Decision Tree")]
    public class DecisionTree : NodeGraph
    {
        /// <summary>
        /// Gets the starting node of the decision tree.
        /// </summary>
        /// <returns></returns>
        public StartNode GetStartNode()
        {
            foreach(var node in nodes)
            {
                if (node is StartNode sn)
                {
                    return sn;
                }
            }    
            return null;
        }
    }
}