/*****************************************************************************
// File Name : DecisionUIService.cs
// Author : Brandon Koederitz
// Creation Date : 4/6/2026
// Last Modified : 4/6/2026
//
// Brief Description : Controls visualizing player decisions on the canvas.
*****************************************************************************/
using IDAS.Decisions;
using UnityEngine;

namespace IDAS.UI
{
    public class DecisionUIService : UIService
    {
        private DecisionTreeService decisionService;

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
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void VisualizeDecision(DecisionNode obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
