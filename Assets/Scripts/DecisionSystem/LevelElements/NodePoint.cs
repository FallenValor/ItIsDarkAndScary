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
using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

namespace IDAS.Decisions
{
    [RequireComponent(typeof(SplineContainer))]
    [RequireComponent(typeof(CinemachineSplineSmoother))]
    public class NodePoint : MonoBehaviour
    {
        [SerializeField] private DecisionTree tree;
        [SerializeField] private DarkScaryNode node;
        [SerializeField] private string oldNodeName;

        #region Properties
        public DecisionTree Tree => tree;
        public DarkScaryNode Node => node;
        #endregion


        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] protected SplineContainer splineC;

        [ContextMenu("Get Component References")]
        private void Reset()
        {
            splineC = GetComponent<SplineContainer>();
        }
        #endregion
    }
}
