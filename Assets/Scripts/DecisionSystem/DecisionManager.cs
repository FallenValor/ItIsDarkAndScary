/*****************************************************************************
// File Name : DecisionManager.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : Manages the decision system and holds the main decision tree.
*****************************************************************************/
using IDAS.Decisions.Tree;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace IDAS.Decisions
{
    public class DecisionManager : Manager
    {
        [field: SerializeField] public DecisionTree DecisionTree { get; private set; }
        public Dictionary<DarkScaryNode, NodePoint> NodePoints { get; private set; } 
            = new Dictionary<DarkScaryNode, NodePoint>();

        /// <summary>
        /// Initialize the NodePoints dictionary, in addition to managers.
        /// </summary>
        /// <param name="applicationManager">The application manager that spawns this manager.</param>
        /// <param name="ct"> The CancellationToken for this routine.</param>
        /// <returns>Async</returns>
        public override async Awaitable Initialize(ApplicationManager applicationManager, CancellationToken ct)
        {
            // Initialize all node points.
            NodePoint[] points = FindObjectsByType<NodePoint>(FindObjectsSortMode.InstanceID)
                .Where(x => !x.IsIgnored).ToArray();

            // Initialize the node point dictionary.
            for (int i = 0; i < points.Length; i++)
            {
                NodePoints.Add(points[i].Node, points[i]);
            }
            await base.Initialize(applicationManager, ct);
        }
    }

}