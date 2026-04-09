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
using IDAS.Items;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

namespace IDAS.Decisions
{
    public class NodePoint : MonoBehaviour
    {
        // User Data
        [SerializeField] private DecisionTree tree;
        [SerializeField] private DarkScaryNode node;
        [SerializeField] private Item associatedItem;

        [Header("Components")]
        [SerializeField] private CinemachineCamera cCam;

        // Internal data.
        [SerializeField] private string oldNodeName;
        [SerializeField] private bool isDuplicate;
        [SerializeField] private SplineContainer[] splines;
        [SerializeField] private NodePoint[] nextPoints;
        [SerializeField] private SplinePointPair[] nextSplines;



        #region Properties
        public DecisionTree Tree => tree;
        public DarkScaryNode Node => node;
        public Item AssociatedItem => associatedItem;
        public CinemachineCamera CCam => cCam;
        public SplineContainer[] Splines => splines;
        public NodePoint[] NextPoints => nextPoints;
        public SplinePointPair[] NextSplines => nextSplines;
        public bool IsDuplicate => isDuplicate;
        public bool IsIgnored => isDuplicate;

        public bool HasSplines => node == null ? false : node.HasOut;
        public ItemID Item => node is ItemNode i ? i.ID : ItemID.None;
        #endregion

        #region Nested
        [System.Serializable]
        public struct SplinePointPair
        {
            [field: SerializeField] public SplineContainer spline { get; private set; }
            [field: SerializeField] public NodePoint point { get; private set; }


        }

        #endregion


        private void Reset()
        {
            cCam = GetComponentInChildren<CinemachineCamera>();
        }
    }
}
