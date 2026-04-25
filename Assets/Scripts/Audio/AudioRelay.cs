/*****************************************************************************
// File Name : AudioRelay.cs
// Author : Brandon Koederitz
// Creation Date : 4/20/2026
// Last Modified : 4/20/2026
//
// Brief Description : Script that exposes audio playing functions to UnityEvents.
*****************************************************************************/
using UnityEngine;

namespace IDAS.Audio
{
    public class AudioRelay : MonoBehaviour
    {
        #region Instance Handling
        public static AudioManager Manager { private get; set; }
        private static SoundPlayerService soundPlayer;

        /// <summary>
        /// Initializes a newly createad AudioManager.
        /// </summary>
        /// <param name="manager">The manager to initialize.</param>
        /// <returns>Returns false if initialization failed.</returns>
        public static bool InitializeManager(AudioManager manager)
        {
            if (Manager != null && Manager != manager)
            {
                return false;
            }
            else
            {
                Manager = manager;
                soundPlayer = Manager.GetService<SoundPlayerService>();
                return true;
            }
        }

        public static void DeinitializeManager(AudioManager manager)
        {
            if (Manager == manager)
            {
                soundPlayer = null;
                Manager = null;
            }
        }
        #endregion


        public void PlayOneShot(string soundName)
        {
            if (soundPlayer != null)
            {
                soundPlayer.PlayOneShot(soundName);
            }
        }

        public void PlayOneShotSpatial(string soundName)
        {
            if (soundPlayer != null)
            {
                soundPlayer.PlayOneShot(soundName, transform.position);
            }
        }

        public void StartSound(string soundName)
        {
            if (soundPlayer != null)
            {
                soundPlayer.StartSound(soundName);
            }
        }

        public void StopSound(string soundName)
        {
            if (soundPlayer != null)
            {
                soundPlayer.StopSound(soundName);
            }
        }
        
        // Add ambience/music parameters
    }
}
