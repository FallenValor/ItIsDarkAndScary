/***********************************************************************
// File Name : PlayerController.cs
// Author : Brandon Koederitz
// Creation Date : 4/8/2026
// Last Modified : 4/8/2026
//
// Brief Description : Data script containing information on the player object that is spawned by the 
PlayerControllerService.
*****************************************************************************/
using UnityEngine;

namespace IDAS
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform[] itemSlots;

        public Transform GetItemSlot(int index)
        {
            if (index < 0 || index >= itemSlots.Length) { return null; }
            return itemSlots[index];
        }
    }
}
