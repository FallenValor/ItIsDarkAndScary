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
using UnityEngine.UI;

namespace IDAS.UI
{
    public class TimerUIService : UIService
    {
        [SerializeField] private GameObject timerParent;
        [SerializeField] private Image timerImage;

        private TimerService timer;
        private float currentSteepness;

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
            timerParent.SetActive(true);
        }
        public void HideTimer()
        {
            timerParent.SetActive(false);
        }

        /// <summary>
        /// Uodate's the fill on the timer image 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="normalizedTime"></param>
        private void OnUpdateTimer(float time, float normalizedTime)
        {
            timerImage.fillAmount = TimerSkewCurve(normalizedTime, currentSteepness);
        }

        /// <summary>
        /// Calculates the fill of the timer based on a curve scale.
        /// </summary>
        /// <remarks>Formula: steepness ^ (-time + logbase[steepness](1 - a)) + a.
        /// A formula: -1/(s-1)</remarks>
        /// <param name="normalziedTime">The normalized time of the timer.</param>
        /// <param name="steepness">The steepness of the timer curve.</param>
        /// <returns>A normalzied value represneting the fill of the timer.</returns>
        private static float TimerSkewCurve(float normalziedTime, float steepness)
        {
            float a = - 1 / (steepness - 1);
            return Mathf.Pow(steepness, -normalziedTime + Mathf.Log(1 - a, steepness)) + a;
        }
    }
}
