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
        [SerializeField] private bool consumeItem;


        #region Accessors
        public string Name => name;
        public int Stamina => stamina;
        public ItemID Item => item;
        public bool ConsumeItem => consumeItem;
        #endregion

        /// <summary>
        /// Checks if this choice is valid to be made.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            // Add decision validity later when implemenmting stamina and items.
            return true;
        }
    }
}
