/*****************************************************************************
// File Name : DamageNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/14/2026
// Last Modified : 4/14/2026
//
// Brief Description : Node that deals damage to the player to punish poor choices.
*****************************************************************************/
using IDAS.Decisions;
using UnityEngine;

namespace IDAS
{
    [NodeTint("#ae3723")]
    public class DamageNode : RelayNode
    {
        [Header("Damage")]
        [SerializeField] private int damage = 1;

        /// <summary>
        /// Signal the health service to take damage.
        /// </summary>
        /// <param name="treeTraveler"></param>
        public override void OnNodeEnter(DecisionTreeService treeTraveler)
        {
            HealthService healthService = treeTraveler.DecisionManager.GetService<HealthService>();
            if (healthService != null)
            {
                healthService.Health -= damage;
            }
            if (!healthService.IsDead)
            {
                base.OnNodeEnter(treeTraveler);
            }
        }
    }
}
