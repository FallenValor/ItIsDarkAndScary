/*****************************************************************************
// File Name : SoundPlayerService.cs
// Author : Brandon Koederitz
// Creation Date : 4/20/2026
// Last Modified : 4/20/2026
//
// Brief Description : Handles playing one shot or instance sounds.
*****************************************************************************/
using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

namespace IDAS.Audio
{
    public class SoundPlayerService : Service
    {
        private FMODEvents events;

        private Dictionary<string, EventInstance> runningInstances;

        /// <summary>
        /// SoundPlayerService always initializes after the FMODEvents.
        /// </summary>
        protected override void Initialize()
        {
            events = Manager.GetService<FMODEvents>();
        }

        #region One Shots
        public void PlayOneShot(string soundName)
        {
            RuntimeManager.PlayOneShot(events.GetEvent(soundName));
        }

        public void PlayOneShot(string soundName, Vector3 worldPos)
        {
            RuntimeManager.PlayOneShot(events.GetEvent(soundName), worldPos);
        }
        #endregion

        #region Continuous Sounds
        public void StartSound(string soundName)
        {
            EventInstance inst = RuntimeManager.CreateInstance(events.GetEvent(soundName));
            runningInstances.Add(soundName, inst);
            inst.start();
        }

        public void StopSound(string soundName)
        {
            if (runningInstances.ContainsKey(soundName))
            {
                EventInstance inst = runningInstances[soundName];
                inst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                runningInstances.Remove(soundName);
                inst.release();
            }
        }
        #endregion
    }
}
