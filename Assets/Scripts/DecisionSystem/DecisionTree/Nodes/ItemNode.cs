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
        [SerializeField] private ItemID item;


        #region Properties
        // Relay nodes cannot be randomly selected.
        public override bool RandomSelectable => true;
        #endregion


        /// <summary>
        /// Immediately move to the next node when this node is reached, and also give the player an item.
        /// </summary>
        /// <param name="treeTraveler"></param>
        public override void OnNodeEnter(DecisionTreeService treeTraveler)
        {
            base.OnNodeEnter(treeTraveler);
            // Item nodes also give the player an item when passed.
            ItemService itemService = treeTraveler.DecisionManager.GetService<ItemService>();
            if (itemService != null)
            {
                itemService.GainItem(item, this);
            }
        }
    }
}
