/*****************************************************************************
// File Name : TimerService.cs
// Author : Brandon Koederitz
// Creation Date : 4/6/2026
// Last Modified : 4/6/2026
//
// Brief Description : Manages adding a time limit to player decisions.
*****************************************************************************/
using System;
using UnityEngine;

namespace IDAS.Decisions
{
    public class TimerService : DecisionService
    {
        #region Events
        public event Action<float, float> TimerUpdateEvent;
        public event Action TimerCompleteEvent;
        #endregion

        public void StartTimer(float time)
        {

        }

        public void StopTimer()
        {

        }
    }
}
