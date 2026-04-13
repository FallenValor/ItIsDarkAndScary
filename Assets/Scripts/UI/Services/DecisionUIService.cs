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
        [SerializeField] private DecisionDisplay displayPrefab;

        private DecisionTreeService decisionService;
        private DecisionManager decisionManager;

        private readonly Queue<DecisionDisplay> displayPool = new Queue<DecisionDisplay>();

        /// <summary>
        /// Setup event subscriptions
        /// </summary>
        protected override void Initialize()
        {
            decisionService = AppManager.GetManager<DecisionManager>().GetService<DecisionTreeService>();
            decisionService.ReachDecisionEvent += VisualizeDecision;
        }
        public override void Deinitialize()
        {
            decisionService.ReachDecisionEvent -= VisualizeDecision;
        }

        /// <summary>
        /// Visualizes all choices for a decision on 
        /// </summary>
        /// <param name="node">The node that needs to have it's choices visualized.</param>
        /// <param name="point">The point to get the ChoicePoints from.</param>
        private void VisualizeDecision(DecisionNode node, NodePoint point)
        {
            
        }

        #region Display Object Pooling
        private DecisionDisplay GetDisplay()
        {
            DecisionDisplay display = displayPool.Count > 0 ? displayPool.Dequeue() : 
                Instantiate(displayPrefab, transform);
            display.gameObject.SetActive(true);
            return display;
        }

        private void ReturnDisplay(DecisionDisplay display)
        {
            displayPool.Enqueue(display);
            display.gameObject.SetActive(false);
        }
        #endregion
    }
}
