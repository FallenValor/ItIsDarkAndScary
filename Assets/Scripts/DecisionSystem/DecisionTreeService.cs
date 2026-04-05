/*****************************************************************************
// File Name : DecisionTreeService.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/4/2026
//
// Brief Description : Manages logic for traveling the decision tree.
*****************************************************************************/
using IDAS.Decisions.Tree;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace IDAS.Decisions
{
    public class DecisionTreeService : DecisionService
    {
        private DecisionTree DecisionTree => DecisionManager.DecisionTree;

        private DarkScaryNode currentNode;
        private DecisionNodeBase currentDecision;

        #region Events
        public event Action<global::DarkScaryNode> ReachNodeEvent;
        public event Action<DecisionNodeBase> ReachDecisionEvent;
        #endregion

        /// <summary>
        /// Initializes/Deinitializes input references.
        /// </summary>
        /// <returns></returns>
        protected override void Initialize()
        {
            Manager.GetService<InputService>().DecisionInputEvent += OnDecisionInput;
            // Set the current decision to the starting decision.
            SetCurrentNode(DecisionTree.GetStartNode());
        }
        public override void Deinitialize()
        {
            Manager.GetService<InputService>().DecisionInputEvent -= OnDecisionInput;
        }
        
        /// <summary>
        /// Sets the current node that the player is at in the decision tree.
        /// </summary>
        /// <param name="node"></param>
        private void SetCurrentNode(DarkScaryNode node)
        {
            if (currentNode != null)
            {
                currentNode.OnNodeExit(this);
            }
            currentNode = node;
            ReachNodeEvent?.Invoke(currentNode);
            Debug.Log($"Current node is now {currentNode.name}");
            if (currentNode != null)
            {
                currentNode.OnNodeEnter(this);
            }
        }

        #region Decisions
        /// <summary>
        /// Progresses through the tree through a currently queued decision based on player input.
        /// </summary>
        /// <param name="decision"></param>
        private void OnDecisionInput(int decision)
        {
            if (currentDecision != null && 
                decision < currentDecision.Choices.Length && 
                currentDecision.Choices[decision].IsValid())
            {
                // Debug.
                Debug.Log($"You chose {currentDecision.Choices[decision].Name}");

                global::DarkScaryNode nextNode = currentDecision.GetDecisionNode(decision);
                // Reduce stamina based on cost.
                SetCurrentNode(nextNode);

                // Clear the current decision.
                currentNode = null; 
            }
        }
        
        /// <summary>
        /// Queues a decision for the player to make.
        /// </summary>
        /// <param name="decisionNode">The decision node that the player is making a decision at.</param>
        public void QueueDecision(DecisionNodeBase decisionNode)
        {
            currentDecision = decisionNode;
            ReachDecisionEvent?.Invoke(decisionNode);

            // Debug
            for(int i = 0; i < decisionNode.Choices.Length; i++)
            {
                Debug.Log($"{i}. {decisionNode.Choices[i].Name}");
            }
        }
        #endregion

        /// <summary>
        /// Ends the current decision tree.
        /// </summary>
        public void EndTree()
        {
            // TODO: Tree End implementation.
            Debug.Log("Tree Ended.");
        }
    }
}