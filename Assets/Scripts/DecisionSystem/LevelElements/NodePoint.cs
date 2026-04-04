/*****************************************************************************
// File Name : DecisionPoint.cs
// Author : Brandon Koederitz
// Creation Date : 4/4/2026
// Last Modified : 4/4/2026
//
// Brief Description : Script that describes a particular point in the level where a decision on the DecisionTree 
takes place.
*****************************************************************************/
using IDAS.Decisions.Tree;
using System.Threading.Tasks;
using UnityEngine;

namespace IDAS.Decisions
{
    public class NodePoint : MonoBehaviour
    {
        [SerializeField] private DecisionTree tree;
        [SerializeField] private DarkScaryNode node;
        [SerializeField] private string oldNodeName;

        #region Properties
        public DecisionTree Tree => tree;
        public DarkScaryNode Node => node;
        #endregion

    }
}
