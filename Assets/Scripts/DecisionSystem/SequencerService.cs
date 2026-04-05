/*****************************************************************************
// File Name : SequencerService.cs
// Author : Brandon Koederitz
// Creation Date : 4/4/2026
// Last Modified : 4/4/2026
//
// Brief Description : Manages sequencing player actions and the progression of the game.
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace IDAS.Decisions
{
    public delegate Awaitable SequenceTask(CancellationToken ct);
    public class SequencerService : DecisionService
    {
        #region CONSTS
        private const float REQUEUE_WAIT_TIME = 0.5f;
        #endregion

        private readonly Queue<SequenceTask> taskQueue = new Queue<SequenceTask>();

        private CancellationTokenSource queueCts;

        /// <summary>
        /// Initialize all NodePoints in the current level.
        /// </summary>
        /// <returns></returns>
        protected override void Initialize()
        {
            queueCts = new CancellationTokenSource();
            QueueIterator(queueCts.Token);
        }

        /// <summary>
        /// Cancels the async function that loops through the queue.
        /// </summary>
        public override void Deinitialize()
        {
            queueCts.Cancel();
        }

        /// <summary>
        /// Continually asyncronously loops through the queue
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async void QueueIterator(CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    // Sleep until a task is queued.
                    while (taskQueue.Count == 0)
                    {
                        await Awaitable.WaitForSecondsAsync(REQUEUE_WAIT_TIME);
                    }

                    // Run the awaiatble in the queue.
                    await taskQueue.Dequeue()?.Invoke(ct);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
        }

        /// <summary>
        /// Enqueues an awaitable returning function as an action to sequence in the queue.
        /// </summary>
        /// <param name="toQueue"></param>
        public void QueueAction(SequenceTask toQueue)
        {
            taskQueue.Enqueue(toQueue);
        }
    }
}
