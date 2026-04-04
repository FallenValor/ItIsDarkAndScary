/*****************************************************************************
// File Name : DecisionPoint.cs
// Author : Brandon Koederitz
// Creation Date : 4/4/2026
// Last Modified : 4/4/2026
//
// Brief Description : Script that describes a particular point in the level where a decision on the DecisionTree 
takes place.
*****************************************************************************/
using IDAS.Decisions.Tree;
using System.Threading.Tasks;
using UnityEngine;

namespace IDAS.Decisions
{
    public class NodePoint : MonoBehaviour
    {
        [SerializeField] private DecisionTree tree;
        [SerializeField] private string nodeName;

        public int nodeIndex { get; private set; }

        #region Properties
        public DecisionTree Tree => tree;
        #endregion

        /// <summary>
        /// Initializes this node point.
        /// </summary>
        /// <param name="service"></param>
        public Task Initialize(SequencerService service)
        {
            nodeIndex = tree.GetNodeIndex(nodeName);
            // If this is a NodePoint for a DecisionNode, initialize child ChoicePoints that define where UI should be.
            if (tree.nodes[nodeIndex] is DecisionNodeBase decision)
            {
                ChoicePoint[] choices = GetComponentsInChildren<ChoicePoint>();
                foreach (ChoicePoint choicePoint in choices)
                {
                    choicePoint.Initialize(decision);
                }
            }
            
            return Task.CompletedTask;
        }
    }
}
