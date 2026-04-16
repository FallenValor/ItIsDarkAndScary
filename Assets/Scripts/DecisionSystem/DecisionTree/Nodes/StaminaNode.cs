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
    public class StaminaNode : RelayNode
    {
        [Header("Stamina")]
        [SerializeField] private int staminaChange;

        /// <summary>
        /// Signal the health service to take damage.
        /// </summary>
        /// <param name="treeTraveler"></param>
        public override void OnNodeEnter(DecisionTreeService treeTraveler)
        {
            StaminaService staminaService = treeTraveler.DecisionManager.GetService<StaminaService>();
            if(staminaService != null)
            {
                staminaService.Stamina += staminaChange;
            }
            base.OnNodeEnter(treeTraveler);
        }
    }
}
