/*****************************************************************************
// File Name : DecisionManager.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : Manages the decision system and holds the main decision tree.
*****************************************************************************/
using UnityEngine;

public class DecisionManager : Manager
{
    [field: SerializeField] public DecisionTree DecisionTree { get; private set; }
}
