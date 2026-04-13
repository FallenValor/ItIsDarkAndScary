/***********************************************************************
// File Name : PlayerControllerService.cs
// Author : Brandon Koederitz
// Creation Date : 4/4/2026
// Last Modified : 4/4/2026
//
// Brief Description : Controls moving the player along cinemachine dolly tracks to move them through the world
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private CinemachineSplineDolly splineDollyPrefab;
        [SerializeField] private float playerTravelSpeed;

        private SequencerService sequencer;

        private CinemachineSplineDolly splineDolly;
        private PlayerController player;

        #region Properties
        public PlayerController Player => player;
        #endregion

        /// <summary>
        ///  Initializes the player prefab and dictionary of node points.
        /// </summary>
        protected override void Initialize()
        {
            DecisionTreeService dts = Manager.GetService<DecisionTreeService>();
            if (dts != null)
            {
                dts.MovementEvent += QueueMoveToPoint;
            }

            sequencer = Manager.GetService<SequencerService>();

            // Get the starting point.
            DarkScaryNode startNode = DecisionManager.DecisionTree.GetStartNode();
            NodePoint startPoint = DecisionManager.GetPoint(startNode);

            // Spawn the player at the starting node.
            player = Instantiate(playerPrefab, startPoint.transform.position, startPoint.transform.rotation);

            // Spawn the dolly at the starting node.
            splineDolly = Instantiate(splineDollyPrefab, startPoint.transform.position, startPoint.transform.rotation);
            startPoint.CCam.Prioritize();

            // Debug
            //MoveToPoint(0);
        }

        /// <summary>
        /// Unsubscribe events.
        /// </summary>
        public override void Deinitialize()
        {
            DecisionTreeService dts = Manager.GetService<DecisionTreeService>();
            if (dts != null)
            {
                dts.MovementEvent -= QueueMoveToPoint;
            }
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
                await MoveToPointAsync(DecisionManager.GetPoint(currentNode), nodeIndex, DecisionManager.GetPoint(targetNode), ct);
            }
            // Queue the MoveToPoint call with the SequencerService.
            sequencer.QueueAction(MoveToPointWrapper);
        }

        /// <summary>
        /// Moves the player from their current node to another node along the node's defined spline paths.
        /// </summary>
        /// <param name="splineIndex">The index of the subsequent node/spline to move to.</param>
        /// <returns></returns>
        public async Awaitable MoveToPointAsync(NodePoint startPoint, int splineIndex, NodePoint endPoint, CancellationToken ct)
        {
            try
            {
                // Get the spline to move along.
                if (splineIndex > startPoint.Splines.Length || startPoint.Splines[splineIndex] == null)
                {
                    Debug.LogWarning($"No Spline found connecting nodes {startPoint} and {endPoint}.  " +
                        $"Using default Cinemachine interpolation.");
                    endPoint.CCam.Prioritize();
                    return;
                }
                SplineContainer spline = startPoint.Splines[splineIndex];

                //Update the player.
                splineDolly.CameraPosition = 0;
                splineDolly.Spline = spline;
                splineDolly.VirtualCamera.Prioritize();
                float splineLength = spline.CalculateLength();

                // Continually move the player along the spline.
                while (splineDolly.CameraPosition < splineLength - REQUIRED_END_DIST)
                {
                    ct.ThrowIfCancellationRequested();
                    splineDolly.CameraPosition += playerTravelSpeed * Time.deltaTime;


                    await Awaitable.NextFrameAsync();
                }

                endPoint.CCam.Prioritize();
                Debug.Log("Hit end of track");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
        }
    }
}
