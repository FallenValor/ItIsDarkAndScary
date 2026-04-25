/*****************************************************************************
// File Name : FMODEvents.cs
// Author : Brandon Koederitz
// Creation Date : 4/20/2026
// Last Modified : 4/20/2026
//
// Brief Description : Stores relevant FMOD events for one shot or instance sounds.
*****************************************************************************/
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

namespace IDAS.Audio
{
    public class FMODEvents : Service
    {
        [SerializeField] private EventKey[] sounds;

        private readonly Dictionary<string, EventReference> soundDict = new Dictionary<string, EventReference>();

        [System.Serializable]
        private struct EventKey
        {
            [SerializeField] internal string name;
            [SerializeField] internal EventReference sound;
        }

        protected override void Initialize()
        {
            
            foreach(var key in sounds)
            {
                soundDict.Add(key.name, key.sound);
            }
        }

        public EventReference GetEvent(string name)
        {
            if (soundDict.ContainsKey(name))
            {
                return soundDict[name];
            }
            return default;
        }
    }
}
