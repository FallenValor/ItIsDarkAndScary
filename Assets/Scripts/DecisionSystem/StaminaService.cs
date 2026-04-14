/*****************************************************************************
// File Name : StaminaService.cs
// Author : Brandon Koederitz
// Creation Date : 4/14/2026
// Last Modified : 4/14/2026
//
// Brief Description : Controls the player's stamina that they can expend on certain decisions.
*****************************************************************************/
using System;
using UnityEngine;

namespace IDAS.Decisions
{
    public class StaminaService : DecisionService
    {
        [SerializeField] private int maxStamina;

        private int stamina;

        public event Action<int, int> StaminaUpdateEvent;

        #region Properties
        public int Stamina
        {
            get { return stamina; }
            set
            {
                int oldStamina = stamina;
                stamina = Mathf.Clamp(value, 0, maxStamina);
                int change = stamina - oldStamina;
                StaminaUpdateEvent?.Invoke(change, stamina);
                Debug.Log("Stamina is now " + stamina);
            }
        }
        #endregion

        protected override void GameStart()
        {
           ResetStamina();
        }

        /// <summary>
        /// Resets the player back to max stamina.
        /// </summary>
        public void ResetStamina()
        {
            Stamina = maxStamina;
        }
    }
}
