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

        private readonly Dictionary<string, EventInstance> runningInstances = new Dictionary<string, EventInstance>();

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
            Debug.Log("Played one shot " + soundName);
            RuntimeManager.PlayOneShot(events.GetEvent(soundName));
        }

        public void PlayOneShot(string soundName, Vector3 worldPos)
        {
            Debug.Log("Played one shot " + soundName);
            RuntimeManager.PlayOneShot(events.GetEvent(soundName), worldPos);
        }
        #endregion

        #region Continuous Sounds
        public void StartSound(string soundName)
        {
            try
            {
                EventInstance inst = RuntimeManager.CreateInstance(events.GetEvent(soundName));
                runningInstances.Add(soundName, inst);
                inst.start();
                Debug.Log("Started " + soundName);
            }
            catch (EventNotFoundException)
            {
                Debug.LogWarning($"No FMOD Event with the name {soundName} was found.");
            }
            
        }

        public void StopSound(string soundName)
        {
            Debug.Log("stopped " + soundName);
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
