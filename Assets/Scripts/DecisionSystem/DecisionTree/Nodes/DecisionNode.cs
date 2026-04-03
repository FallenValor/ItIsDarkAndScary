/*****************************************************************************
// File Name : DecisionNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : xNode node that represents a specific decision point in the decision tree.
*****************************************************************************/
using IDAS.Decisions.Tree;
using UnityEngine;
using XNode;

namespace IDAS
{
    [CreateAssetMenu(fileName = "DecisionNode", menuName = "Decision Tree/DecisionNode")]
    public class DecisionNode : DecisionNodeBase
    {
        [SerializeField, Input] private Choice inputChoice;
    }

}