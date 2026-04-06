/*****************************************************************************
// File Name : ApplicationManager.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : Manages initializing all manager systems.
*****************************************************************************/
using System.Threading;
using UnityEngine;

namespace IDAS
{
    public class ApplicationManager : MonoBehaviour
    {
        [SerializeField] private Manager[] managers;

        private Manager[] managerInstances;
        private CancellationTokenSource cts;

        /// <summary>
        /// Initialize all managers on awake.
        /// </summary>
        private async Awaitable Awake()
        {
            managerInstances = new Manager[managers.Length];
            cts = new CancellationTokenSource();
            for (int i = 0; i < managers.Length; i++)
            {
                Manager inst = Instantiate(managers[i], transform);
                managerInstances[i] = inst;
                await inst.Initialize(cts.Token);
            }
        }

        /// <summary>
        /// De-Initialize all managers on destroy
        /// </summary>
        private async Awaitable OnDestroy()
        {
            managerInstances = new Manager[managers.Length];
            cts.Cancel();
            for (int i = 0; i < managers.Length; i++)
            {
                await managerInstances[i].Deinitialize();
            }
        }

        /// <summary>
        /// Gets a specific manager instance by type.
        /// </summary>
        /// <typeparam name="T">The type of the manager instance to get.</typeparam>
        /// <returns>The found manager.</returns>
        public T GetManager<T>() where T : Manager
        {
            foreach (var manager in managerInstances)
            {
                if (manager is T typedInst)
                {
                    return typedInst;
                }
            }
            return null;
        }
    }

}