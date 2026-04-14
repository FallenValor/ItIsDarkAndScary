/*****************************************************************************
// File Name : HealthUIService.cs
// Author : Brandon Koederitz
// Creation Date : 4/14/2026
// Last Modified : 4/14/2026
//
// Brief Description : Displays the player's current health on the UI.
*****************************************************************************/
using IDAS.Decisions;
using IDAS.UI;
using UnityEngine;

namespace IDAS.UI
{
    public class HealthUIService : UIService
    {
        [SerializeField] private RectTransform healthFill;

        private HealthService healthService;

        /// <summary>
        /// Sets up even references to change the displayed health when health changes.
        /// </summary>
        protected override void Initialize()
        {
            healthService = AppManager.GetManager<DecisionManager>().GetService<HealthService>();
            healthService.HealthChangedEvent += UpdateHealthDisplay;
        }
        public override void Deinitialize()
        {
            healthService.HealthChangedEvent -= UpdateHealthDisplay;
        }

        /// <summary>
        /// Updates the health display object's scale based on the player's health percent.
        /// </summary>
        /// <param name="healthChange">The change in the player's health.</param>
        /// <param name="currentHealth">the player's current numeric health.</param>
        /// <param name="healthPercent">The current percentage of their max health the player is at.</param>
        private void UpdateHealthDisplay(int healthChange, int currentHealth, float healthPercent)
        {
            healthFill.localScale = Vector3.one * healthPercent;
        }
    }
}
