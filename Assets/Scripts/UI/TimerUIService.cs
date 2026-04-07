/*****************************************************************************
// File Name : TimerUIService.cs
// Author : Brandon Koederitz
// Creation Date : 4/6/2026
// Last Modified : 4/6/2026
//
// Brief Description : Controls visualizing the decision timer on the canvas.
*****************************************************************************/
using IDAS.Decisions;
using IDAS.UI;
using UnityEngine;

namespace IDAS.UI
{
    public class TimerUIService : UIService
    {
        private TimerService timer;

        /// <summary>
        /// Setup event references.
        /// </summary>
        protected override void Initialize()
        {
            timer = AppManager.GetManager<DecisionManager>().GetService<TimerService>();
            timer.TimerUpdateEvent += OnUpdateTimer;
            timer.TimerStartEvent += ShowTimer;
            timer.TimerCancelEvent += HideTimer;
            timer.TimerCompleteEvent += HideTimer;
        }
        
        /// <summary>
        /// Unsubscribe event references.
        /// </summary>
        public override void Deinitialize()
        {
            timer.TimerUpdateEvent -= OnUpdateTimer;
            timer.TimerStartEvent -= ShowTimer;
            timer.TimerCancelEvent -= HideTimer;
            timer.TimerCompleteEvent -= HideTimer;
        }


        public void ShowTimer()
        {

        }

        public void HideTimer()
        {

        }

        private void OnUpdateTimer(float time, float normalizedTime)
        {
            
        }
    }
}
