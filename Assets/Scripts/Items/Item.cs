/*****************************************************************************
// File Name : ItemObject.cs
// Author : Brandon Koederitz
// Creation Date : 4/8/2026
// Last Modified : 4/8/2026
//
// Brief Description : Script for prefab objects that represent items in the scene.
*****************************************************************************/
using UnityEngine;

namespace IDAS.Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemID item;
        [SerializeField] private Item itemPrefab;

        #region Properties
        private ItemID ID => item;
        private Item Prefab => itemPrefab;
        #endregion


    }
}
