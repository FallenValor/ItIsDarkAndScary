/*****************************************************************************
// File Name : ApplicationManager.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : Manages initializing all manager systems.
*****************************************************************************/
using UnityEngine;

namespace IDAS
{
    public class ApplicationManager : MonoBehaviour
    {
        [SerializeField] private Manager[] managers;

        private Manager[] managerInstances;

        /// <summary>
        /// Initialize all managers on awake.
        /// </summary>
        private async Awaitable Awake()
        {
            managerInstances = new Manager[managers.Length];
            for (int i = 0; i < managers.Length; i++)
            {
                Manager inst = Instantiate(managers[i], transform);
                managerInstances[i] = inst;
                await inst.Initialize();
            }
        }

        /// <summary>
        /// De-Initialize all managers on destroy
        /// </summary>
        private async Awaitable OnDestroy()
        {
            managerInstances = new Manager[managers.Length];
            for (int i = 0; i < managers.Length; i++)
            {
                Manager inst = Instantiate(managers[i], transform);
                managerInstances[i] = inst;
                await inst.Deinitialize();
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