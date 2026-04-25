/*****************************************************************************
// File Name : AudioManager.cs
// Author : Brandon Koederitz
// Creation Date : 4/20/2026
// Last Modified : 4/20/2026
//
// Brief Description : Manages playing Audio using FMOD
*****************************************************************************/
using System.Threading;
using UnityEngine;

namespace IDAS.Audio
{
    public class AudioManager : Manager
    {
        /// <summary>
        /// Setup AudioRelay static function references.
        /// </summary>
        /// <param name="applicationManager"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override Awaitable Initialize(ApplicationManager applicationManager, CancellationToken ct)
        {
            // Need to initialize services before initializing the manager.
            Awaitable awaitable = base.Initialize(applicationManager, ct);
            if (!AudioRelay.InitializeManager(this))
            {
                Destroy(gameObject);
                return null;
            }
            return awaitable;
        }

        public override Awaitable Deinitialize()
        {
            AudioRelay.DeinitializeManager(this);
            return base.Deinitialize();
        }
    }
}
