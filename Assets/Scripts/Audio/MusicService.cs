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



        protected override void Initialize()
        {
            base.Initialize();
        }
    }
}
