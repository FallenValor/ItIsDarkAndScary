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
using System.Threading;
using UnityEngine;

namespace IDAS.Decisions
{
    public class DecisionTreeService : DecisionService
    {
        private DecisionTree DecisionTree => DecisionManager.DecisionTree;

        private DarkScaryNode currentNode;
        private DecisionNodeBase currentDecision;

        private SequencerService sequencer;

        #region Events
        public event Action<DarkScaryNode, int, DarkScaryNode> DecisionEvent;
        public event Action<DarkScaryNode> ReachNodeEvent;
        public event Action<DecisionNodeBase> ReachDecisionEvent;
        #endregion

        /// <summary>
        /// Initializes/Deinitializes input references.
        /// </summary>
        /// <returns></returns>
        protected override void Initialize()
        {
            Manager.GetService<InputService>().DecisionInputEvent += OnDecisionInput;
            sequencer = Manager.GetService<SequencerService>();
            // Set the current decision to the starting decision.
            SetCurrentNode(DecisionTree.GetStartNode());
        }
        public override void Deinitialize()
        {
            Manager.GetService<InputService>().DecisionInputEvent -= OnDecisionInput;
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

                DarkScaryNode nextNode = currentDecision.GetDecisionNode(decision);
                // Reduce stamina based on cost.

                // Broadcast that a decision has been made.
                DecisionEvent?.Invoke(currentNode, decision, nextNode);

                ResetCurrentNode();

                // Queue a SetCurrentNode call in the SequencerService.
                Awaitable SetNodeWrapper(CancellationToken ct)
                {
                    ct.ThrowIfCancellationRequested();
                    SetCurrentNode(nextNode);
                    return Awaitable.NextFrameAsync();
                }
                Debug.Log("Set Queued");
                sequencer.QueueAction(SetNodeWrapper);

                // Clear the current decision.
                currentDecision = null; 
            }
        }

        /// <summary>
        /// Resets the current node.
        /// </summary>
        private void ResetCurrentNode()
        {
            // Clean up the current node.
            if (currentNode != null)
            {
                currentNode.OnNodeExit(this);
            }
            currentNode = null;
        }

        /// <summary>
        /// Sets the current node that the player is at in the decision tree.
        /// </summary>
        /// <param name="node"></param>
        private void SetCurrentNode(DarkScaryNode node)
        {
            // Clean up the current node.
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