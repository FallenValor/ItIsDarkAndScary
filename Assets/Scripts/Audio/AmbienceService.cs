/*****************************************************************************
// File Name : AmbienceService.cs
// Author : Brandon Koederitz
// Creation Date : 4/20/2026
// Last Modified : 4/20/2026
//
// Brief Description : Handles playing and modifying game ambience.
*****************************************************************************/
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace IDAS.Audio
{
    public class AmbienceService : Service
    {
        [SerializeField] private EventReference ambienceEvent;

        private EventInstance ambienceInstance;

        #region Instance Handling
        protected override void Initialize()
        {
            try
            {
                ambienceInstance = RuntimeManager.CreateInstance(ambienceEvent);
            }
            catch (EventNotFoundException)
            {
                Debug.LogWarning($"No FMOD Event for ambience has been set.");
            }
            
        }

        protected override void ServiceStart()
        {
            ambienceInstance.start();
        }

        public override void Deinitialize()
        {
            ambienceInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        #endregion

        // Add parameter adjusting here.
    }
}
