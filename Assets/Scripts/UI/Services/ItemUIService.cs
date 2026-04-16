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
        /// <param name="itemData">The array of item ID's that the player is holding.</param>
        private void UpdateItemIcons(ItemData[] itemData)
        {
            Debug.Log("Updating items");
            for(int i = 0; i < itemData.Length && i < iconImages.Length; i++)
            {
                // Show no icon for Item.none
                if (itemData[i] == null || itemData[i].id == ItemID.None)
                {
                    iconImages[i].gameObject.SetActive(false);
                    continue;
                }
                else
                {
                    iconImages[i].gameObject.SetActive(true);
                    Sprite itemSprite = itemData[i].icon != null ? itemData[i].icon : defaultIcon;
                    iconImages[i].sprite = itemSprite;
                }
            }
        }

    }
}
