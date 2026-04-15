/*****************************************************************************
// File Name : ItemObject.cs
// Author : Brandon Koederitz
// Creation Date : 4/8/2026
// Last Modified : 4/8/2026
//
// Brief Description : Script for prefab objects that represent items in the scene.
*****************************************************************************/
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace IDAS.Items
{
    [RequireComponent(typeof(Rigidbody))]
    public class Item : MonoBehaviour
    {
        [SerializeField] private float lerpTime;
        [SerializeField] private AnimationCurve lerpCurve;

        #region Components
        [SerializeField, ShowIfNull] private Rigidbody rb;

        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
        }
        #endregion

        /// <summary>
        /// Sets this item as a child of the equpped transform and makes it LERP to it's new position.
        /// </summary>
        /// <param name="holder"></param>
        public void SetEquippedTransform(Transform holder)
        {
            transform.SetParent(holder, true);

            StartCoroutine(LerpToCenter());
        }

        /// <summary>
        /// LERPS the item to it's equip slot at local pos 0,0
        /// </summary>
        /// <returns></returns>
        private IEnumerator LerpToCenter()
        {
            Vector3 startingPos = transform.localPosition;
            float timer = 0;
            while (timer < lerpTime)
            {
                float normalizedTime = timer / lerpTime;

                transform.localPosition = Vector3.Lerp(startingPos, Vector3.zero, lerpCurve.Evaluate(normalizedTime));

                timer += Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// Drops the item.
        /// </summary>
        public void DropItem()
        {
            transform.SetParent(null);
            rb.isKinematic = false;
        }

        /// <summary>
        /// Destroys the item on removal.
        /// </summary>
        public void RemoveItem()
        {
            Destroy(gameObject);
        }
    }
}
