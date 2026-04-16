/*****************************************************************************
// File Name : FlavorTextUIService.cs
// Author : Brandon Koederitz
// Creation Date : 4/15/2026
// Last Modified : 4/15/2026
//
// Brief Description : Displays flavor text on a simple text bos on the UI.s
*****************************************************************************/
using IDAS.Decisions;
using TMPro;
using UnityEngine;

namespace IDAS.UI
{
    public class FlavorTextUIService : UIService
    {
        [SerializeField] private TMP_Text textDisplay;

        private DecisionTreeService decisionTreeService;

        /// <summary>
        /// Setup event subscriptions to display and clear text.
        /// </summary>
        protected override void Initialize()
        {
            decisionTreeService = AppManager.GetManager<DecisionManager>().GetService<DecisionTreeService>();
            decisionTreeService.OnEnterNode += DisplayFlavorText;
            decisionTreeService.OnExitNode += ClearFlavorText;
        }
        public override void Deinitialize()
        {
            decisionTreeService.OnEnterNode -= DisplayFlavorText;
            decisionTreeService.OnExitNode -= ClearFlavorText;
        }

        /// <summary>
        /// Display's a node's flavor text.
        /// </summary>
        /// <param name="obj">The node to display the flavor text of.</param>
        private void DisplayFlavorText(DarkScaryNode obj)
        {
            textDisplay.text = obj.Flavor;
        }

        /// <summary>
        /// Clears displayed flavor text.
        /// </summary>
        /// <param name="obj">Unused.</param>
        private void ClearFlavorText(DarkScaryNode obj)
        {
            textDisplay.text = string.Empty;
        }
    }
}
