/*****************************************************************************
// File Name : Choice.cs
// Author : Brandon Koederitz
// Creation Date : 4/2/2026
// Last Modified : 4/2/2026
//
// Brief Description : Class that represents a specific choice within a decision tree.
*****************************************************************************/
using UnityEngine;

namespace IDAS.Decisions.Tree
{
    [System.Serializable]
    public class Choice
    {
        [SerializeField] private string name;
        [SerializeField] private int stamina;
        [SerializeField] private Item item;


        #region Accessors
        public int Stamina => stamina;
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
