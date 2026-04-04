/*****************************************************************************
// File Name : DecisionTreeService.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/4/2026
//
// Brief Description : Manages behaviour that interacts with the decision tree, such as making decisions and traveling 
through the tree.
*****************************************************************************/
using IDAS.Decisions;
using System.Threading.Tasks;
using UnityEngine;

namespace IDAS
{
    public class ChoicePoint : MonoBehaviour
    {
        [SerializeField] private string choiceName;

        private int choiceIndex;

        /// <summary>
        /// Initializes this node point.
        /// </summary>
        /// <param name="service"></param>
        public Task Initialize(DecisionNodeBase node)
        {
            choiceIndex = node.GetChoiceIndex(choiceName);
            return Task.CompletedTask;
        }
    }
}
