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
        [SerializeField] private int choiceIndex;
        [SerializeField] private string choiceName;

        [SerializeField] private NodePoint parentNode;

        #region Properties
        public NodePoint ParentNode
        {
            get
            {
                if (parentNode == null)
                {
                    parentNode = GetComponentInParent<NodePoint>();
                }
                return parentNode;
            }
        }
        #endregion

    }
}
