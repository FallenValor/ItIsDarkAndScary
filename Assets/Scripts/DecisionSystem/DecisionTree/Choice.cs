/*****************************************************************************
// File Name : Choice.cs
// Author : Brandon Koederitz
// Creation Date : 4/2/2026
// Last Modified : 4/2/2026
//
// Brief Description : Class that represents a specific choice within a decision tree.
*****************************************************************************/
using UnityEngine;
using IDAS.Items;

namespace IDAS.Decisions.Tree
{
    [System.Serializable]
    public class Choice
    {
        [SerializeField] private string name;
        [SerializeField] private int stamina;
        [SerializeField] private ItemID item;
        [SerializeField] private bool consume;


        #region Accessors
        public string Name => name;
        public int Stamina => stamina;
        public ItemID Item => item;
        public bool ConsumeItem => consume;
        #endregion

        /// <summary>
        /// Checks if this choice is valid to be made.
        /// </summary>
        /// <returns></returns>
        public bool IsValid(DecisionManager manager)
        {
            if (manager == null) { return false; }
            bool isInvalid = false;

            // Check for required stamina.

            // Check for required item.
            if (item != ItemID.None)
            {
                ItemService itemService = manager.GetService<ItemService>();
                // If item required and no service, then this decision isnt valid.
                if (itemService == null)
                {
                    isInvalid |= true;
                }
                else
                {
                    // If the player doesnt have the item, mark as invalid.
                    isInvalid |= !itemService.HasItem(item);
                }
            }

            return !isInvalid;
        }

        /// <summary>
        /// Choices are random selectable if they have no associated cost.
        /// </summary>
        /// <returns></returns>
        public bool IsRandomSelectable()
        {
            return item == ItemID.None && stamina <= 0;
        }

        /// <summary>
        /// Handles behavoir that happens when this choice is selected, such as stamina consumption;
        /// </summary>
        public void OnChosen(DecisionManager manager)
        {
            // Decrease Stamina.

            // Remove Item
            if (consume && item != ItemID.None)
            {
                ItemService itemService = manager.GetService<ItemService>();
                // If item required and no service, then this decision isnt valid.
                if (itemService != null)
                {
                    itemService.RemoveItem(item);
                }
            }
        }
    }
}
