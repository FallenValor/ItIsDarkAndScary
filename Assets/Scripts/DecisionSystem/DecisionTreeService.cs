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
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace IDAS.Decisions
{
    public class DecisionTreeService : DecisionService
    {
        private DecisionTree DecisionTree => DecisionManager.DecisionTree;

        private DarkScaryNode currentNode;
        private DecisionNode currentDecision;

        private DarkScaryNode previousNode;

        private SequencerService sequencer;
        private TimerService timer;

        #region Events
        public event Action<DarkScaryNode, int, DarkScaryNode> MovementEvent;
        public event Action<DecisionNode, NodePoint> ReachDecisionEvent;
        #endregion

        /// <summary>
        /// Initializes/Deinitializes input references.
        /// </summary>
        /// <returns></returns>
        protected override void Initialize()
        {
            Manager.GetService<InputService>().DecisionInputEvent += OnDecisionInput;
            sequencer = Manager.GetService<SequencerService>();

            timer = Manager.GetService<TimerService>();
            timer.TimerCompleteEvent += MakeRandomDecision;

            // Set the current decision to the starting decision.
            SetCurrentNode(DecisionTree.GetStartNode());
        }
        public override void Deinitialize()
        {
            Manager.GetService<InputService>().DecisionInputEvent -= OnDecisionInput;
            timer.TimerCompleteEvent -= MakeRandomDecision;
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
            Debug.Log($"Current node is now {currentNode.name}");
            if (currentNode != null)
            {
                currentNode.OnNodeEnter(this);
            }
        }

        /// <summary>
        /// Moves the player to a next node in the decision tree.
        /// </summary>
        /// <param name="nextNode">The node to move to.</param>
        /// <param name="decisionIndex">The index of the decision made.</param>
        public void MoveToNode(DarkScaryNode nextNode, int decisionIndex)
        {
            // Broadcast that a decision has been made.
            MovementEvent?.Invoke(currentNode, decisionIndex, nextNode);

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
                currentDecision.Choices[decision].IsValid(DecisionManager))
            {
                // Debug.
                Debug.Log($"You chose {currentDecision.Choices[decision].Name}");
                MakeDecision(decision);
            }
        }

        /// <summary>
        /// Forces the player to make a random decision.
        /// </summary>
        private void MakeRandomDecision()
        {
            int randomDecisionIndex = GetRandomDecisionIndex(currentDecision);
            MakeDecision(randomDecisionIndex);
        }

        /// <summary>
        /// Gets a random valid decision index for a decision.
        /// </summary>
        /// <returns></returns>
        private int GetRandomDecisionIndex(DecisionNode decision)
        {
            List<int> validIndicies = new List<int>();
            for(int i = 0; i < decision.Choices.Length; i++)
            {
                if (decision.Choices[i].IsValid(DecisionManager) && 
                    decision.Choices[i].IsRandomSelectable() && 
                    decision.GetDecisionNode(i).RandomSelectable)
                {
                    validIndicies.Add(i);
                }
            }
            return validIndicies[UnityEngine.Random.Range(0, validIndicies.Count)];
        }

        /// <summary>
        /// Progresses the player through a decided choice.
        /// </summary>
        /// <param name="decision"></param>
        private void MakeDecision(int decision)
        {
            if (currentDecision != null &&
                decision < currentDecision.Choices.Length &&
                currentDecision.Choices[decision].IsValid(DecisionManager))
            { 
                DarkScaryNode nextNode = currentDecision.GetDecisionNode(decision);

                currentDecision.Choices[decision].OnChosen(DecisionManager);

                timer.StopTimer();

                MoveToNode(nextNode, decision);

                // Clear the current decision.
                currentDecision = null;
            }
        }

        /// <summary>
        /// Queues a decision for the player to make.
        /// </summary>
        /// <param name="decisionNode">The decision node that the player is making a decision at.</param>
        public void QueueDecision(DecisionNode decisionNode)
        {
            currentDecision = decisionNode;
            ReachDecisionEvent?.Invoke(decisionNode, DecisionManager.GetPoint(decisionNode));

            // Start the timer.
            timer.StartTimer();

            // Debug
            for(int i = 0; i < decisionNode.Choices.Length; i++)
            {
                if (decisionNode.Choices[i].IsValid(DecisionManager))
                {
                    Debug.Log($"{i}. {decisionNode.Choices[i].Name}");
                }
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