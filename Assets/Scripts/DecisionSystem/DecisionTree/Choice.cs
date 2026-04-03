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
        
    }
}
