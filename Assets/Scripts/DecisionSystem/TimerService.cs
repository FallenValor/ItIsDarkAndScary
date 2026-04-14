/*****************************************************************************
// File Name : TimerService.cs
// Author : Brandon Koederitz
// Creation Date : 4/6/2026
// Last Modified : 4/6/2026
//
// Brief Description : Manages adding a time limit to player decisions.
*****************************************************************************/
using System;
using System.Threading;
using UnityEngine;

namespace IDAS.Decisions
{
    public class TimerService : DecisionService
    {
        [SerializeField] private float decisionTime;

        private CancellationTokenSource cts;
        private bool isRunning;

        #region Events
        public event Action TimerStartEvent;
        public event Action<float, float> TimerUpdateEvent;
        public event Action TimerCompleteEvent;
        public event Action TimerStopEvent;
        #endregion

        /// <summary>
        /// Setup event for timer hiding when the player loses.
        /// </summary>
        protected override void Initialize()
        {
            HealthService.LoseGameEvent += StopTimer;
        }

        /// <summary>
        /// Starts the timer counting down.
        /// </summary>
        /// <param name="time"></param>
        public void StartTimer()
        {
            StartTimer(decisionTime);
        }
        public void StartTimer(float time)
        {
            if (isRunning)
            {
                StopTimer();
            }
            cts = new CancellationTokenSource();
            isRunning = true;
            TimerStartEvent?.Invoke();
            TimerAsync(time, cts.Token);
        }

        /// <summary>
        /// Cancel the timer on Deinitialize
        /// </summary>
        public override void Deinitialize()
        {
            HealthService.LoseGameEvent -= StopTimer;
            if (cts != null)
            {
                cts.Cancel();
            }
        } 

        /// <summary>
        /// Actually stops the timer, skipping event calls.
        /// </summary>
        public void StopTimer()
        {
            if (!isRunning) { return; }
            TimerStopEvent?.Invoke();
            cts.Cancel();
        }

        /// <summary>
        /// Cleans up the timer and returns the TimerService to a static state.
        /// </summary>
        private void CleanUpTimer()
        {
            isRunning = false;
            cts = null;
        }

        /// <summary>
        /// Asynchronously runs the timer function.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="ct"></param>
        private async void TimerAsync(float time, CancellationToken ct)
        {
            float timer = time;
            try
            {
                while (timer > 0)
                {
                    if (ct.IsCancellationRequested)
                    {
                        CleanUpTimer();
                        return;
                    }

                    timer -= Time.deltaTime;
                    TimerUpdateEvent?.Invoke(timer, timer / time);

                    await Awaitable.NextFrameAsync(ct);
                }
                // Timer Complete.
                CleanUpTimer();
                TimerCompleteEvent?.Invoke();
            }
            catch (Exception e)
            {
                if (e is not OperationCanceledException)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}
