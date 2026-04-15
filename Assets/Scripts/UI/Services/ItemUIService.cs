/*****************************************************************************
// File Name : ItemUIService.cs
// Author : Brandon Koederitz
// Creation Date : 4/15/2026
// Last Modified : 4/15/2026
//
// Brief Description : Displays player held items on the UI.
*****************************************************************************/
using IDAS.Decisions;
using UnityEngine;
using UnityEngine.UI;
using IDAS.Items;

namespace IDAS.UI
{
    public class ItemUIService : UIService
    {
        [SerializeField] private Sprite defaultIcon;
        [SerializeField, Tooltip("Item sprite icons should be arranged according to the ItemID's index -1.")] 
        private Sprite[] itemIcons;
        [SerializeField] private Image[] iconImages;

        private ItemService itemService;

        /// <summary>
        /// Sets up event subsccriptions to the ItemService.
        /// </summary>
        protected override void Initialize()
        {
            itemService = AppManager.GetManager<DecisionManager>().GetService<ItemService>();
            itemService.ItemsChangedEvent += UpdateItemIcons;
        }
        public override void Deinitialize()
        {
            itemService.ItemsChangedEvent -= UpdateItemIcons;
        }

        /// <summary>
        /// Updates the icons shown on the UI.
        /// </summary>
        /// <param name="itemIDs">The array of item ID's that the player is holding.</param>
        private void UpdateItemIcons(ItemID[] itemIDs)
        {
            Debug.Log("Updating items");
            for(int i = 0; i < itemIDs.Length && i < iconImages.Length; i++)
            {
                // Show no icon for Item.none
                if (itemIDs[i] == ItemID.None)
                {
                    iconImages[i].gameObject.SetActive(false);
                    continue;
                }
                else
                {
                    iconImages[i].gameObject.SetActive(true);
                    int iconIndex = (int)itemIDs[i] - 1;
                    Sprite itemSprite = iconIndex < itemIcons.Length ?
                        itemIcons[iconIndex] : defaultIcon;
                    iconImages[i].sprite = itemSprite;
                }
            }
        }

    }
}
