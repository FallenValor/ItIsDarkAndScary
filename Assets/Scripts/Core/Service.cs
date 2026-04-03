/*****************************************************************************
// File Name : Service.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : Base class for service systems that handle specific functionality.
*****************************************************************************/
using System.Threading.Tasks;
using UnityEngine;

namespace IDAS
{
    public class Service : MonoBehaviour
    {
        private Manager parentManager;

        #region Properties
        protected virtual Manager Manager => parentManager;
        #endregion

        /// <summary>
        /// Initializes the service.
        /// </summary>
        /// <returns></returns>
        public virtual Task Initialize(Manager manager)
        {
            parentManager = manager;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Initializes the service.
        /// </summary>
        /// <returns></returns>
        public virtual Task Deinitialize()
        {
            return Task.CompletedTask;
        }
    }
}