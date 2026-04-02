/*****************************************************************************
// File Name : Manager.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : Base class for manager systems that coordinate multiple services.
*****************************************************************************/
using UnityEngine;

namespace IDAS
{
    public class Manager : MonoBehaviour
    {
        [SerializeField] private Service[] services;

        private Service[] serviceInstances;

        /// <summary>
        /// Initializes all services within the manager.
        /// </summary>
        /// <returns></returns>
        public virtual async Awaitable Initialize()
        {
            serviceInstances = new Service[services.Length];
            for (int i = 0; i < services.Length; i++)
            {
                Service inst = Instantiate(services[i], transform);
                serviceInstances[i] = inst;
                await inst.Initialize(this);
            }
        }

        /// <summary>
        /// Initializes all services within the manager.
        /// </summary>
        /// <returns></returns>
        public virtual async Awaitable Deinitialize()
        {
            for (int i = 0; i < serviceInstances.Length; i++)
            {
                await serviceInstances[i].Deinitialize();
                Destroy(serviceInstances[i]);
            }
            serviceInstances = null;
        }

        /// <summary>
        /// Gets a specific service instance by type.
        /// </summary>
        /// <typeparam name="T">The type of the service instance to get.</typeparam>
        /// <returns>The found service.</returns>
        public T GetService<T>() where T : Service
        {
            foreach (var service in serviceInstances)
            {
                if (service is T typedInst)
                {
                    return typedInst;
                }
            }
            return null;
        }
    }
}
