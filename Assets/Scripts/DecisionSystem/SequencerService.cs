/*****************************************************************************
// File Name : SequencerService.cs
// Author : Brandon Koederitz
// Creation Date : 4/4/2026
// Last Modified : 4/4/2026
//
// Brief Description : Manages sequencing player actions and moving the player through the level.
*****************************************************************************/
using IDAS.Decisions.Tree;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace IDAS.Decisions
{
    public class SequencerService : DecisionService
    {
        [SerializeField] private GameObject playerPrefab;

        private Dictionary<DarkScaryNode, NodePoint> nodePoints;

        private GameObject player;

        /// <summary>
        /// Initialize all NodePoints in the current level.
        /// </summary>
        /// <returns></returns>
        protected override void Initialize()
        {
            // Initialize all node points.
            NodePoint[] points = FindObjectsByType<NodePoint>(FindObjectsSortMode.InstanceID);
            //foreach (NodePoint point in points)
            //{
            //    await point.Initialize(this);
            //}

            // Initialize the node point dictionary.
            for (int i = 0; i < points.Length; i++)
            {
                nodePoints.Add(points[i].Node, points[i]);
            }

            // Get the starting point.
            NodePoint startPoint = nodePoints[DecisionManager.DecisionTree.GetStartNode()];

            // Spawn the player at the starting node.
            player = Instantiate(playerPrefab, startPoint.transform.position, startPoint.transform.rotation);
        }


    }
}
