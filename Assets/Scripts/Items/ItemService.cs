/*****************************************************************************
// File Name : ItemService.cs
// Author : Brandon Koederitz
// Creation Date : 4/8/2026
// Last Modified : 4/8/2026
//
// Brief Description : Manages player held items.
*****************************************************************************/
using UnityEngine;
using IDAS.Items;
using System.Collections.Generic;
using System;
using System.Linq;

namespace IDAS.Decisions
{
    public class ItemService : DecisionService
    {

        [SerializeField] private int maxItems;

        private ItemData[] heldItems;

        #region Nested
        [System.Serializable]
        private struct ItemData
        {
            [SerializeField] internal ItemID id;
            [SerializeField] internal Item obj;

            internal ItemData(ItemID id, Item obj)
            {
                this.id = id;
                this.obj = obj;
            }
        }
        #endregion

        /// <summary>
        /// Initializes all items and references to said items.
        /// </summary>
        protected override void Initialize()
        {
            heldItems = new ItemData[maxItems];
        }

        /// <summary>
        /// Has this player gain a specific item.
        /// </summary>
        /// <param name="item">The item for the player to gain.</param>
        /// <param name="node">The node that the player gained the item from.</param>
        public void GainItem(ItemID item, ItemNode node)
        {
            if (maxItems <= 0) { return; }
            // Shift all items over 1 index.
            for(int i = 0; i < maxItems - 1; i++)
            {
                heldItems[i + 1] = heldItems[i];
            }

            // Get the associated item GameObject from the node.
            Item itemObj = null;
            if (DecisionManager.NodePoints.ContainsKey(node))
            {
                DecisionManager.NodePoints[node]
            }
            

            heldItems[0] = new ItemData(item, itemObj);
        }

        /// <summary>
        /// Checks if the player is holding a specific item.
        /// </summary>
        /// <param name="itemId">The item to check for.</param>
        /// <returns>True if the player is holding the item, false if otherwise.</returns>
        public bool HasItem(ItemID itemId)
        {
            return heldItems.Any(x => x.id == itemId);
        }
    }
}
