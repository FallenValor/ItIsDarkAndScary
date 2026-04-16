/*****************************************************************************
// File Name : ItemData.cs
// Author : Brandon Koederitz
// Creation Date : 4/15/2026
// Last Modified : 4/15/2026
//
// Brief Description : Class for storing grouped data for an item.
*****************************************************************************/
using IDAS.Items;
using UnityEngine;

namespace IDAS.Items
{
    [System.Serializable]
    public class ItemData
    {
        [SerializeField] internal ItemID id;
        [SerializeField] internal Item prefab;
        [SerializeField] internal Sprite icon;
    }
}
