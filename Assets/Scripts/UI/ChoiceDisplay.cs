/*****************************************************************************
// File Name : DecisionDisplay.cs
// Author : Brandon Koederitz
// Creation Date : 4/13/2026
// Last Modified : 4/13/2026
//
// Brief Description : Visualizes a set of choice names on the UI.
*****************************************************************************/
using IDAS.Decisions.Tree;
using IDAS.Items;
using System.Collections;
using TMPro;
using UnityEngine;

namespace IDAS.UI
{
    public class ChoiceDisplay : MonoBehaviour
    {
        #region CONSTS
        private const string STAMINA_TAG = "<sprite name=\"Stamina\">";
        #endregion

        [SerializeField] private TMP_Text textComp;

        private Transform targetTransform;
        private bool isFollowing;
        private string choiceString;

        #region Properties
        private string ChoiceString
        {
            get { return choiceString; }
            set 
            { 
                choiceString = value; 
                textComp.text = choiceString;
            }
        }
        #endregion

        /// <summary>
        /// Sets the transform that determines the position of this choice display on the canvas.
        /// </summary>
        /// <param name="target">The transform to use as the world position of this element.</param>
        public void SetTargetTransform(Transform target, Vector3 defaultPos)
        {
            targetTransform = target;
            if (target == null)
            {
                isFollowing = false;
                // Return to the default position.
                transform.position = defaultPos;
            }
            else if (!isFollowing)
            {
                isFollowing = true;
                StartCoroutine(FollowTransformRoutine());
            }
        }

        /// <summary>
        /// Adds a choice as text to the display.
        /// </summary>
        /// <param name="choice"></param>
        public void AddChoice(Choice choice, string binding)
        {
            // Add icons for stamina cost and item requirements.
            string icons = GetItemTag(choice.Item);
            for(int i = 0; i < choice.Stamina; i++)
            {
                icons += STAMINA_TAG;
            }


            string choiceString = $"{icons} {binding}. {choice.Name}";
            ChoiceString += choiceString + "\n";
        }

        /// <summary>
        /// Clears all data from this display.
        /// </summary>
        public void Clear()
        {
            isFollowing = false;
            ChoiceString = string.Empty;
        }

        private IEnumerator FollowTransformRoutine()
        {
            while(isFollowing)
            {
                transform.position = Camera.main.WorldToScreenPoint(targetTransform.position);
                yield return null;
            }
        }

        /// <summary>
        /// Gets the tag for an item icon based on the item's ID.
        /// </summary>
        /// <param name="item">The item ID to get the tag of.</param>
        /// <returns>The icon tag for that item.</returns>
        private string GetItemTag(ItemID item)
        {
            if (item == ItemID.None) { return string.Empty; }
            return $"<sprite index={(int)item}>";
        }
    }
}
