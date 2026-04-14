/*****************************************************************************
// File Name : HealthService.cs
// Author : Brandon Koederitz
// Creation Date : 4/14/2026
// Last Modified : 4/14/2026
//
// Brief Description : Control's the players health as a lose condition.
*****************************************************************************/
using System;
using UnityEngine;

namespace IDAS.Decisions
{
    public class HealthService : DecisionService
    {
        [SerializeField] private int maxHealth = 3;

        private int health;
        private bool isDead;

        public event Action<int, int> HealthChangedEvent;
        public static event Action LoseGameEvent;

        #region Properties
        public bool IsDead => isDead;
        #endregion

        /// <summary>
        /// Initializes health.
        /// </summary>
        protected override void Initialize()
        {
            health = maxHealth;
        }

        /// <summary>
        /// Makes the player take damage.
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(int damage)
        {
            health -= damage;
            HealthChangedEvent?.Invoke(damage, health);
            if (health <= 0)
            {
                isDead = true;
                // Lose Condition.
                Debug.Log("Player lost");
                LoseGameEvent?.Invoke();
            }
        }
    }
}
