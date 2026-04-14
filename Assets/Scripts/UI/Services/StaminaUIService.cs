/*****************************************************************************
// File Name : StaminaUIService.cs
// Author : Brandon Koederitz
// Creation Date : 4/14/2026
// Last Modified : 4/14/2026
//
// Brief Description : Controls visualizing the decision timer on the canvas.
*****************************************************************************/
using IDAS.Decisions;
using UnityEngine;

namespace IDAS.UI
{
    public class StaminaUIService : UIService
    {
        [SerializeField] private GameObject[] staminaFills;

        private StaminaService staminaService;
        private int currentStaminaNum;

        /// <summary>
        /// Set up event subscriptions to update the stamina UI.
        /// </summary>
        protected override void Initialize()
        {
            staminaService = AppManager.GetManager<DecisionManager>().GetService<StaminaService>();
            staminaService.StaminaUpdateEvent += UpdateStaminaUI;
        }
        public override void Deinitialize()
        {
            staminaService.StaminaUpdateEvent -= UpdateStaminaUI;
        }

        /// <summary>
        /// Updates the number of shown stamina icons based on the player's current stamina.
        /// </summary>
        /// <param name="staminaChange">The change in the player's stamina from this update.</param>
        /// <param name="stamina">The player's current stamina.</param>
        private void UpdateStaminaUI(int staminaChange, int stamina)
        {
            if (staminaChange == 0) { return; }
            bool enabled = staminaChange > 0;
            // Disable or Enable the corresponding stamina fills.
            for(int i = currentStaminaNum; i != currentStaminaNum + staminaChange; i += (enabled ? 1 : -1))
            {
                staminaFills[i].SetActive(enabled);
            }

            currentStaminaNum += staminaChange - 1;
        }
    }
}
