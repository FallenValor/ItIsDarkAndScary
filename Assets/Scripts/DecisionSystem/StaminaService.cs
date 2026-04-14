/*****************************************************************************
// File Name : StaminaService.cs
// Author : Brandon Koederitz
// Creation Date : 4/14/2026
// Last Modified : 4/14/2026
//
// Brief Description : Controls the player's stamina that they can expend on certain decisions.
*****************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IDAS.Decisions
{
    public class StaminaService : DecisionService
    {
        #region CONSTS
        private const string STAMINA_KEY = "Stamina";
        #endregion

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
                PersistentData.SaveData(STAMINA_KEY, stamina);
            }
        }
        #endregion

        protected override void GameStart()
        {
            try
            { 
                Stamina = PersistentData.RetrieveData<int>(STAMINA_KEY);
            }
            catch (KeyNotFoundException)
            {
                ResetStamina();
            }
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
