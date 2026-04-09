/*****************************************************************************
// File Name : RelayNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/7/2026
// Last Modified : 4/7/2026
//
// Brief Description : Node that immediately moves the player to the next node.  Allows for looping.
*****************************************************************************/
using IDAS.Decisions;
using IDAS.Decisions.Tree;
using System.Threading;
using UnityEngine;
using XNode;

namespace IDAS
{
    public class RelayNode : DarkScaryNode
    {
        [SerializeField] private float delay;
        [SerializeField, Output(backingValue = ShowBackingValue.Never)] private Choice outputChoice;

        #region Properties
        // Relay nodes cannot be randomly selected.
        public override bool RandomSelectable => false;
        #endregion

        /// <summary>
        /// Gets an out value based on port name.
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public override object GetValue(NodePort port)
        {
            switch (port.fieldName)
            {
                case nameof(outputChoice):
                    return outputChoice;
                default:
                    return base.GetValue(port);
            }
        }

        /// <summary>
        /// The only next node is the node linked by outputChoice;
        /// </summary>
        /// <returns></returns>
        public override DarkScaryNode[] GetAllNextNodes()
        {
            return new DarkScaryNode[] { GetNextNode() };
        }

        /// <summary>
        /// Gets the next node connected to this node by outChoice.
        /// </summary>
        /// <returns></returns>
        public DarkScaryNode GetNextNode()
        {
            NodePort otherPort = GetPort(nameof(outputChoice)).Connection;
            if (otherPort != null)
            {
                return otherPort.node as DarkScaryNode;
            }
            return null;
        }

        /// <summary>
        /// Immediately move to the next node when this node is reached.
        /// </summary>
        /// <param name="treeTraveler"></param>
        public override void OnNodeEnter(DecisionTreeService treeTraveler)
        {
            QueueDelay(treeTraveler, delay);
            // Relay nodes always use a decision index of 0.
            treeTraveler.MoveToNode(GetNextNode(), 0);
        }

        /// <summary>
        /// Queues a delay with the sequencer service.
        /// </summary>
        /// <param name="treeService"></param>
        protected void QueueDelay(DecisionTreeService treeService, float delay)
        {
            async Awaitable DelayWrapper(CancellationToken ct)
            {
                await Awaitable.WaitForSecondsAsync(delay, ct);
            }

            // Queue a delay with the sequencer.
            SequencerService sequencer = treeService.DecisionManager.GetService<SequencerService>();
            if (sequencer != null)
            {
                sequencer.QueueAction(DelayWrapper);
            }
        }
    }
}
