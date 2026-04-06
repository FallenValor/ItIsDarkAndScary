/***********************************************************************
// File Name : PlayerControllerService.cs
// Author : Brandon Koederitz
// Creation Date : 4/4/2026
// Last Modified : 4/4/2026
//
// Brief Description : Controls moving the player along cinemachine dolly tracks to move them through the world
*****************************************************************************/
using System.Collections.Generic;
using System.Threading;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

namespace IDAS.Decisions
{
    public class PlayerControllerService : DecisionService
    {
        #region CONSTS
        private const float REQUIRED_END_DIST = 0.01f;
        #endregion

        [SerializeField] private CinemachineSplineDolly playerPrefabCamera;
        [SerializeField] private float playerTravelSpeed;

        private readonly Dictionary<DarkScaryNode, NodePoint> nodePoints = new Dictionary<DarkScaryNode, NodePoint>();

        private SequencerService sequencer;

        private CinemachineSplineDolly player;

        /// <summary>
        ///  Initializes the player prefab and dictionary of node points.
        /// </summary>
        protected override void Initialize()
        {
            Manager.GetService<DecisionTreeService>().DecisionEvent += QueueMoveToPoint;

            sequencer = Manager.GetService<SequencerService>();

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
            startPoint.CCam.Prioritize();

            // Debug
            //MoveToPoint(0);
        }

        /// <summary>
        /// Unsubscribe events.
        /// </summary>
        public override void Deinitialize()
        {
            Manager.GetService<DecisionTreeService>().DecisionEvent -= QueueMoveToPoint;
        }

        /// <summary>
        /// Queues a movement from one node to another using the SequencerService.
        /// </summary>
        /// <param name="currentNode">The current node that the player is starting at.</param>
        /// <param name="nodeIndex">The index of the next node from the current one.</param>
        /// <param name="targetNode">The target node.</param>
        public void QueueMoveToPoint(DarkScaryNode currentNode, int nodeIndex, DarkScaryNode targetNode)
        {
            async Awaitable MoveToPointWrapper(CancellationToken ct)
            {
                await MoveToPoint(nodePoints[currentNode], nodeIndex, nodePoints[targetNode], ct);
            }
            // Queue the MoveToPoint call with the SequencerService.
            sequencer.QueueAction(MoveToPointWrapper);
        }

        /// <summary>
        /// Moves the player from their current node to another node along the node's defined spline paths.
        /// </summary>
        /// <param name="splineIndex">The index of the subsequent node/spline to move to.</param>
        /// <returns></returns>
        public async Awaitable MoveToPoint(NodePoint startPoint, int splineIndex, NodePoint endNode, CancellationToken ct)
        {
            // Get the spline to move along.
            SplineContainer spline = startPoint.Splines[splineIndex];
            if (spline == null)
            {
                return;
            }

            //Update the player.
            player.CameraPosition = 0;
            player.Spline = spline;
            player.VirtualCamera.Prioritize();
            float splineLength = spline.CalculateLength();

            // Continually move the player along the spline.
            while(player.CameraPosition < splineLength - REQUIRED_END_DIST)
            {
                ct.ThrowIfCancellationRequested();
                player.CameraPosition += playerTravelSpeed * Time.deltaTime;

                
                await Awaitable.NextFrameAsync();
            }

            endNode.CCam.Prioritize();
            Debug.Log("Hit end of track");
        }
    }
}
