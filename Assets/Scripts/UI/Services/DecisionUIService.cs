/*****************************************************************************
// File Name : DecisionUIService.cs
// Author : Brandon Koederitz
// Creation Date : 4/6/2026
// Last Modified : 4/6/2026
//
// Brief Description : Controls visualizing player decisions on the canvas.
*****************************************************************************/
using IDAS.Decisions;
using System.Collections.Generic;
using UnityEngine;

namespace IDAS.UI
{
    public class DecisionUIService : UIService
    {
        [SerializeField] private ChoiceDisplay defaultDisplay;
        [SerializeField] private ChoiceDisplay displayPrefab;

        private DecisionTreeService decisionService;
        private DecisionManager decisionManager;

        private ChoiceDisplay[] currentDisplays;

        private readonly Queue<ChoiceDisplay> displayPool = new Queue<ChoiceDisplay>();

        /// <summary>
        /// Setup event subscriptions
        /// </summary>
        protected override void Initialize()
        {
            decisionManager = AppManager.GetManager<DecisionManager>();
            decisionService = decisionManager.GetService<DecisionTreeService>();
            decisionService.ReachDecisionEvent += VisualizeDecision;
            decisionService.MakeDecisionEvent += ClearDisplays;
        }

        public override void Deinitialize()
        {
            decisionService.ReachDecisionEvent -= VisualizeDecision;
            decisionService.MakeDecisionEvent -= ClearDisplays;
        }

        /// <summary>
        /// Visualizes all choices for a decision on 
        /// </summary>
        /// <param name="node">The node that needs to have it's choices visualized.</param>
        /// <param name="point">The point to get the ChoicePoints from.</param>
        private void VisualizeDecision(DecisionNode node, NodePoint point)
        {
            Dictionary<Transform, ChoiceDisplay> displayDict = new Dictionary<Transform, ChoiceDisplay>();
            List<ChoiceDisplay> displayList = new List<ChoiceDisplay>();

            //loop through each choice.
            for(int i = 0; i < node.Choices.Length; i++)
            {
                // Skip invalid choices.
                if (!node.Choices[i].IsValid(decisionManager)) { continue; }
                // Get the ChoiceDisplay to edit.
                Transform choicePoint = i < point.ChoicePoints.Length ? point.ChoicePoints[i] : null;
                ChoiceDisplay display;
                if (choicePoint == null)
                {
                    display = defaultDisplay;
                }
                else if (displayDict.ContainsKey(choicePoint))
                {
                    display = displayDict[choicePoint];
                }
                else
                {
                    // Get a new display to use.
                    display = GetDisplay();
                    display.SetTargetTransform(choicePoint);
                    displayDict.Add(choicePoint, display);
                    displayList.Add(display);
                }

                display.AddChoice(node.Choices[i], (i+1).ToString());

                currentDisplays = displayList.ToArray();
            }
        }

        /// <summary>
        /// Clears all existing displays when a choice has been made.
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="decisionIndex"></param>
        /// <param name="nextNode"></param>
        private void ClearDisplays(DarkScaryNode currentNode, int decisionIndex, DarkScaryNode nextNode)
        {
            if (currentDisplays == null) { return; }
            defaultDisplay.Clear();
            foreach(var display in currentDisplays)
            {
                display.Clear();
                ReturnDisplay(display);
            }
            currentDisplays = null;
        }

        #region Display Object Pooling
        private ChoiceDisplay GetDisplay()
        {
            ChoiceDisplay display = displayPool.Count > 0 ? displayPool.Dequeue() : 
                Instantiate(displayPrefab, transform);
            display.gameObject.SetActive(true);
            return display;
        }

        private void ReturnDisplay(ChoiceDisplay display)
        {
            displayPool.Enqueue(display);
            display.gameObject.SetActive(false);
        }
        #endregion
    }
}
