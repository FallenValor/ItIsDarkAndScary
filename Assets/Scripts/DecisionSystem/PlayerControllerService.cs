/***********************************************************************
// File Name : PlayerControllerService.cs
// Author : Brandon Koederitz
// Creation Date : 4/4/2026
// Last Modified : 4/4/2026
//
// Brief Description : Controls moving the player along cinemachine dolly tracks to move them through the world
*****************************************************************************/
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace IDAS.Decisions
{
    public class PlayerControllerService : DecisionService
    {
        [SerializeField] private CinemachineSplineDolly playerPrefabCamera;

        private Dictionary<DarkScaryNode, NodePoint> nodePoints;

        private CinemachineSplineDolly player;

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
            player = Instantiate(playerPrefabCamera, startPoint.transform.position, startPoint.transform.rotation);
        }


    }
}
