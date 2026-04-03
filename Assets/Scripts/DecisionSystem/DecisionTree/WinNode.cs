/****************************************************************************
// File Name : WinNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/2/2026
// Last Modified : 4/2/2026
//
// Brief Description : Defines the node that ends the decision tree.
*****************************************************************************/
using IDAS.Decisions.Tree;
using UnityEngine;
using XNode;

namespace IDAS
{
    [CreateAssetMenu(fileName = "WinNode", menuName = "Scriptable Objects/Win Node")]
    public class WinNode : Node
    {
        [SerializeField, Input] private Choice inputChoice;
    }
}
