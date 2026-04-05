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
using UnityEngine.Splines;

namespace IDAS.Decisions
{
    public class PlayerControllerService : DecisionService
    {
        [SerializeField] private CinemachineSplineDolly playerPrefabCamera;
        [SerializeField] private float playerTravelSpeed;

        private readonly Dictionary<DarkScaryNode, NodePoint> nodePoints = new Dictionary<DarkScaryNode, NodePoint>();

        private CinemachineSplineDolly player;
        private DarkScaryNode currentNode;

        /// <summary>
        ///  Initializes the player prefab and dictionary of node points.
        /// </summary>
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
            DarkScaryNode startNode = DecisionManager.DecisionTree.GetStartNode();
            NodePoint startPoint = nodePoints[startNode];

            // Spawn the player at the starting node.
            player = Instantiate(playerPrefabCamera, startPoint.transform.position, startPoint.transform.rotation);
            currentNode = startNode;

            // Debug
            //MoveToPoint(0);
        }

        /// <summary>
        /// Moves the player from their current node to another node along the node's defined spline paths.
        /// </summary>
        /// <param name="nodeIndex">The index of the subsequent node to move to.</param>
        /// <returns></returns>
        public async Awaitable MoveToPoint(int nodeIndex)
        {
            // Get the spline to move along.
            NodePoint point = nodePoints[currentNode];
            SplineContainer spline = point.Splines[nodeIndex];

            //Update the player.
            player.CameraPosition = 0;
            player.Spline = spline;
            float splineLength = spline.CalculateLength();

            // Continually move the player along the spline.
            while(player.CameraPosition < splineLength)
            {
                player.CameraPosition += playerTravelSpeed * Time.deltaTime;

                await Awaitable.NextFrameAsync();
            }
        }
    }
}
