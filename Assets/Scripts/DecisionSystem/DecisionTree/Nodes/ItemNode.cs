/*****************************************************************************
// File Name : RelayNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/7/2026
// Last Modified : 4/7/2026
//
// Brief Description : Node that immediately moves the player to the next node.  Allows for looping.
*****************************************************************************/
using IDAS.Decisions;
using IDAS.Items;
using UnityEngine;

namespace IDAS
{
    public class ItemNode : RelayNode
    {
        [Header("Item Settings")]
        [SerializeField] private float preItemDelay;
        [SerializeField] private ItemID item;


        #region Properties
        // Relay nodes cannot be randomly selected.
        public override bool RandomSelectable => true;
        public ItemID ID => item;
        #endregion


        /// <summary>
        /// Immediately move to the next node when this node is reached, and also give the player an item.
        /// </summary>
        /// <param name="treeTraveler"></param>
        public override void OnNodeEnter(DecisionTreeService treeTraveler)
        {
            // Queue a delay with the sequencer.
            QueueDelay(treeTraveler, preItemDelay);

            // Item nodes also give the player an item when passed.
            ItemService itemService = treeTraveler.DecisionManager.GetService<ItemService>();
            if (itemService != null)
            {
                itemService.GainItemSequenced(item, this);
            }

            base.OnNodeEnter(treeTraveler);
        }
    }
}
