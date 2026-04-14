/*****************************************************************************
// File Name : PersistentData.cs
// Author : Brandon Koederitz
// Creation Date : 4/9/2026
// Last Modified : 4/9/2026
//
// Brief Description : Static class for storing data across scenes.
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace IDAS
{
    public static class PersistentData
    {
        private static readonly Dictionary<string, object> persistentDataDict = new Dictionary<string, object>();

        /// <summary>
        /// Saves data to the static dictionary.
        /// </summary>
        /// <param name="key">The key to save the data under.</param>
        /// <param name="obj">The object to save.</param>
        public static void SaveData(string key, object obj)
        {
            if (persistentDataDict.ContainsKey(key))
            {
                persistentDataDict[key] = obj;
            }
            else
            {
                persistentDataDict.Add(key, obj);
            }
        }

        /// <summary>
        /// Retrieves persistent data saved between scenes.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <param name="key">The key to retrieve.</param>
        /// <returns>The data object stored at the key.</returns>
        public static T RetrieveDataAsClass<T>(string key) where T : class
        {
            if (persistentDataDict.ContainsKey(key))
            {
                return persistentDataDict[key] as T;
            }
             throw new KeyNotFoundException("Could not find PersistentData saved under the key " + key);
        }

        /// <summary>
        /// Retrieves persistent data saved between scenes.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <param name="key">The key to retrieve.</param>
        /// <returns>The data object stored at the key.</returns>
        public static T RetrieveData<T>(string key)
        {
            if (persistentDataDict.ContainsKey(key))
            {
                return (T)persistentDataDict[key];
            }
            throw new KeyNotFoundException("Could not find PersistentData saved under the key " + key);
        }
    }
}
