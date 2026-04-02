/*****************************************************************************
// File Name : Service.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : Base class for service systems that handle specific functionality.
*****************************************************************************/
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
        public virtual async Awaitable Initialize(Manager manager)
        {
            parentManager = manager;
        }

        /// <summary>
        /// Initializes the service.
        /// </summary>
        /// <returns></returns>
        public virtual async Awaitable Deinitialize()
        {
        }
    }
}