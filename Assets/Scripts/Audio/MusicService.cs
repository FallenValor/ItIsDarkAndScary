/*****************************************************************************
// File Name : MusicService.cs
// Author : Brandon Koederitz
// Creation Date : 4/20/2026
// Last Modified : 4/20/2026
//
// Brief Description : Handles playing and transitioning between music.
*****************************************************************************/
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace IDAS.Audio
{
    public class MusicService : Service
    {
        [SerializeField] private EventReference musicEvent;

        private EventInstance musicInstance;

        #region Instance Handling
        protected override void Initialize()
        {
            musicInstance = RuntimeManager.CreateInstance(musicEvent);
        }

        protected override void ServiceStart()
        {
            musicInstance.start();
        }

        public override void Deinitialize()
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        #endregion

        // Add parameter adjusting here.
    }
}
