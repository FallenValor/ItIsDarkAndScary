/*****************************************************************************
// File Name : Service.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : Base class for service systems that handle specific functionality.
*****************************************************************************/
using System;
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
        public virtual Task InitializeAsync(Manager manager)
        {
            try
            {
                this.parentManager = manager;
                Initialize();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            return Task.CompletedTask;
        }

        protected virtual void Initialize() { }

        /// <summary>
        /// Initializes the service.
        /// </summary>
        /// <returns></returns>
        public virtual Task DeinitializeAsync()
        {
            try
            {
                Initialize();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            return Task.CompletedTask;
        }
        public virtual void Deinitialize() { }
    }
}