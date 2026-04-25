/*****************************************************************************
// File Name : TimerAudioRelay.cs
// Author : Brandon Koederitz
// Creation Date : 4/21/2026
// Last Modified : 4/21/2026
//
// Brief Description : Plays sounds for the timer.
*****************************************************************************/
using IDAS.Decisions;
using UnityEngine;

namespace IDAS.Audio
{
    public class TimerAudioRelay : AudioRelay
    {
        [SerializeField] private string timerSound;
        private void Awake()
        {
            TimerService ts = GetComponent<TimerService>();
            ts.TimerStartEvent += StartTimerSound;
            ts.TimerCancelEvent += StopTimerSound;
            ts.TimerCompleteEvent += StopTimerSound;
        }

        private void OnDestroy()
        {
            TimerService ts = GetComponent<TimerService>();
            ts.TimerStartEvent -= StartTimerSound;
            ts.TimerCancelEvent -= StopTimerSound;
            ts.TimerCompleteEvent -= StopTimerSound;
        }

        private void StartTimerSound()
        {
            StartSound(timerSound);
        }

        private void StopTimerSound()
        {
            StopSound(timerSound);
        }
    }
}
