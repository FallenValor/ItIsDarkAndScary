/*****************************************************************************
// File Name : HealthService.cs
// Author : Brandon Koederitz
// Creation Date : 4/14/2026
// Last Modified : 4/14/2026
//
// Brief Description : Control's the players health as a lose condition.
*****************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IDAS.Decisions
{
    public class HealthService : DecisionService
    {
        #region CONSTS
        private const string HEALTH_KEY = "Health";
        #endregion

        [SerializeField] private int maxHealth = 3;
        [SerializeField, Tooltip("The amount of health the player loses when the timer expires.")] 
        private int damageOnTimerFail = 1;

        private int health;
        private bool isDead;

        public event Action<int, int> HealthChangedEvent;
        public static event Action LoseGameEvent;

        #region Properties
        public bool IsDead => isDead;
        public int Health
        {
            get { return health; }
            set 
            {
                int oldHealth = health;
                health = Mathf.Clamp(value, 0, maxHealth);
                int change = health - oldHealth;
                HealthChangedEvent?.Invoke(change, health);
                Debug.Log("Health is now " + health);
                PersistentData.SaveData(HEALTH_KEY, health);

                if (health <= 0)
                {
                    isDead = true;
                    // Lose Condition.
                    Debug.Log("Player lost");
                    LoseGameEvent?.Invoke();
                }
            }
        }
        #endregion

        /// <summary>
        /// Setup event for losing health when timer expires.
        /// </summary>
        protected override void Initialize()
        {
            DecisionManager.GetService<TimerService>().TimerCompleteEvent += OnTimerComplete;
        }
        public override void Deinitialize()
        {
            DecisionManager.GetService<TimerService>().TimerCompleteEvent -= OnTimerComplete;
        }

        /// <summary>
        /// Initializes health.
        /// </summary>
        protected override void GameStart()
        {
            try
            {
                Health = PersistentData.RetrieveData<int>(HEALTH_KEY);
            }
            catch (KeyNotFoundException)
            {
                ResetHealth();
            }
        }

        /// <summary>
        /// Resets the player back to base health.
        /// </summary>
        public void ResetHealth()
        {
            Health = maxHealth;
        }

        /// <summary>
        /// Reduces the player's health when the timer expires.
        /// </summary>
        private void OnTimerComplete()
        {
            Health -= damageOnTimerFail;
        }
    }
}
