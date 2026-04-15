/*****************************************************************************
// File Name : ItemService.cs
// Author : Brandon Koederitz
// Creation Date : 4/8/2026
// Last Modified : 4/8/2026
//
// Brief Description : Manages player held items.
*****************************************************************************/
using IDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace IDAS.Decisions
{
    public class ItemService : DecisionService
    {
        #region CONSTS
        private const string ITEM_DATA_KEY = "Items";
        #endregion

        [SerializeField] private int maxItems;
        [SerializeField] private ItemData[] itemDatabase;

        private ItemInstanceData[] heldItems;

        private PlayerController player;
        private SequencerService sequencer;

        public event Action<ItemID[]> ItemsChangedEvent;

        #region Nested
        [System.Serializable]
        private class ItemData
        {
            [SerializeField] internal ItemID id;
            [SerializeField] internal Item prefab;

            internal ItemData(ItemID id, Item obj)
            {
                this.id = id;
                this.prefab = obj;
            }
        }

        [System.Serializable]
        private class ItemInstanceData
        {
            [SerializeField] internal ItemData data;
            [SerializeField] internal Item obj;

            #region Properties
            internal ItemID id => data.id;
            #endregion

            internal ItemInstanceData(ItemData data, Item obj)
            {
                this.data = data;
                this.obj = obj;
            }
        }
        #endregion


        /// <summary>
        /// Initializes all items and references to said items.
        /// </summary>
        protected override void Initialize()
        {
            PlayerControllerService pcs = DecisionManager.GetService<PlayerControllerService>();
            if (pcs != null)
            {
                player = pcs.Player;
            }
            else
            {
                Debug.LogWarning("ItemService is missing it's dependen service PlayerControllerService.");
            }

            sequencer = DecisionManager.GetService<SequencerService>();

            // Reorder ItemDatabase for easy ID indexing.
            ItemData[] database = itemDatabase;
            itemDatabase = new ItemData[Enum.GetValues(typeof(ItemID)).Length];
            for(int i = 0; i < database.Length; i++)
            {
                itemDatabase[i] = Array.Find(database, x => x.id == (ItemID)i);
            }
        }

        /// <summary>
        /// Set the players items in ServiceStart after initialization.
        /// </summary>
        protected override void ServiceStart()
        {
            // Retrieves from persistent data.  If no data, set to a new array.
            // Update Persistent Data.
            try
            {
                heldItems = InstantiateItems(PersistentData.RetrieveDataAsClass<ItemData[]>(ITEM_DATA_KEY));
            }
            catch (KeyNotFoundException)
            {
                heldItems = new ItemInstanceData[maxItems];
            }
            RegisterItemChange();
        }

        private void RegisterItemChange()
        {
            ItemsChangedEvent?.Invoke(heldItems.Select(x => x == null ? ItemID.None : x.id).ToArray());
            // Update Persistent Data.
            PersistentData.SaveData(ITEM_DATA_KEY, heldItems.Select(x => x == null ? null : x.data).ToArray());
        }

        /// <summary>
        /// Has this player gain a specific item, utilizing the sequencer to await delays.
        /// </summary>
        /// <param name="item">The item for the player to gain.</param>
        /// <param name="node">The node that the player gained the item from.</param>
        public void GainItemSequenced(ItemID item, ItemNode node)
        {
            if (sequencer != null)
            {
                Awaitable GainItemWrapper(CancellationToken ct)
                {
                    ct.ThrowIfCancellationRequested();
                    GainItem(item, node);
                    return Awaitable.NextFrameAsync(ct);
                }
                sequencer.QueueAction(GainItemWrapper);
            }
            else
            {
                GainItem(item, node);
            }
        }

        /// <summary>
        /// Has this player gain a specific item.
        /// </summary>
        /// <param name="item">The item for the player to gain.</param>
        /// <param name="node">The node that the player gained the item from.</param>
        public void GainItem(ItemID item, ItemNode node)
        {
            if (maxItems <= 0) { return; }

            Debug.Log("Gained " + item);
            // Drop the last item.
            if (heldItems[^1] != null)
            {
                heldItems[^1].obj.DropItem();
            }

            // Shift all items over 1 index.
            for(int i = 0; i < maxItems - 1; i++)
            {
                ItemInstanceData current = heldItems[i];
                heldItems[i + 1] = current;
                // Update the item's hand location.
                if (current != null && current.obj != null)
                {
                    current.obj.SetEquippedTransform(player.GetItemSlot(i + 1));
                }
            }

            // Get the associated item GameObject from the node.
            Item itemObj = null;
            NodePoint point = DecisionManager.GetPoint(node);
            if (node != null)
            {
                itemObj = point.AssociatedItem;
            }

            heldItems[0] = new ItemInstanceData(itemDatabase[(int)item], itemObj);
            if (heldItems[0].obj != null)
            {
                // Update the item's hand location.
                heldItems[0].obj.SetEquippedTransform(player.GetItemSlot(0));
            }

            RegisterItemChange();
        }

        /// <summary>
        /// Removes an item from the player's inventory.
        /// </summary>
        /// <param name="itemId"></param>
        public void RemoveItem(ItemID itemId)
        {
            //Debug.Log("Removed " + itemId);
            int index = Array.FindIndex(heldItems, x => x.id == itemId);
            ItemInstanceData data = heldItems[index];
            // Do cleanup on the removed item.
            data.obj.RemoveItem();
            heldItems[index] = null;

            RegisterItemChange();
        }

        /// <summary>
        /// Checks if the player is holding a specific item.
        /// </summary>
        /// <param name="itemId">The item to check for.</param>
        /// <returns>True if the player is holding the item, false if otherwise.</returns>
        public bool HasItem(ItemID itemId)
        {
            return heldItems.Any(x => x != null && x.id == itemId);
        }

        // If I have time after playtest, swap this to a database.
        #region Item Data Management
        /// <summary>
        /// Instantiates an array of item data from prefabs.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private ItemInstanceData[] InstantiateItems(ItemData[] items)
        {
            if (items == null) { return null; }
            ItemInstanceData[] instItems = new ItemInstanceData[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null) { continue; }
                Item spawnedItem = Instantiate(items[i].prefab);
                instItems[i] = new ItemInstanceData(items[i], spawnedItem);
                spawnedItem.SetEquippedTransform(player.GetItemSlot(i), true);
            }
            return instItems;
        }
        #endregion
    }
}
