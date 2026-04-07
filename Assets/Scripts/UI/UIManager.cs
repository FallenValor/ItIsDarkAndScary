/*****************************************************************************
// File Name : UIManager.cs
// Author : Brandon Koederitz
// Creation Date : 4/6/2026
// Last Modified : 4/6/2026
//
// Brief Description : Manages UI centered scripts and instantiates the main canvas.
*****************************************************************************/
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IDAS.UI
{
    public class UIManager : Manager
    {
        [SerializeField] private Canvas canvasPrefab;
        [SerializeField] private EventSystem eventSystemPrefab;

        public Canvas Canvas { get; private set; }

        /// <summary>
        /// Initializes 
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Awaitable Initialize(CancellationToken ct)
        {
            // Spawn the canvas prefab before initialization.
            Instantiate(eventSystemPrefab);
            Canvas = Instantiate(canvasPrefab);
            await base.Initialize(ct);
        }
    }
}
