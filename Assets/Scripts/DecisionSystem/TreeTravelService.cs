/*****************************************************************************
// File Name : TreeTravelService.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : Controls the sequence of moving through decision points on the tree.
*****************************************************************************/
using IDAS.Decisions.Tree;
using System;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace IDAS.Decisions
{
    public class TreeTravelService : DecisionService
    {
        private DecisionTree DecisionTree => DecisionManager.DecisionTree;

        private DecisionNodeBase currentDecision;

        /// <summary>
        /// Initializes/Deinitializes input references.
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public override Task Initialize()
        {
            try
            {
                Manager.GetService<InputService>().DecisionInputEvent += OnDecisionInput;
                // Set the current decision to the starting decision.
                currentDecision = DecisionTree.GetStartNode();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            return Task.CompletedTask;
        }
        public override Task Deinitialize()
        {
            Manager.GetService<InputService>().DecisionInputEvent -= OnDecisionInput;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Progresses through the tree based on player input.
        /// </summary>
        /// <param name="decision"></param>
        private void OnDecisionInput(int decision)
        {
            Choice test = currentDecision.GetInputValue<Choice>("inputChoice");
            Debug.Log(test);
            if (decision < currentDecision.Choices.Length)
            {
                Node nextNode = currentDecision.GetDecisionNode(decision);
                
            }
        }
    }
}