/*****************************************************************************
// File Name : WinUIService.cs
// Author : Brandon Koederitz
// Creation Date : 4/16/2026
// Last Modified : 4/16/2026
//
// Brief Description : Displays UI for when the player completes the game.
*****************************************************************************/
using IDAS.Decisions;
using UnityEngine;

namespace IDAS.UI
{
    public class WinUIService : UIService
    {
        [SerializeField] private GameObject winText;
        protected override void Initialize()
        {
            LevelService.WinGameEvent += DisplayWinText;
        }

        public override void Deinitialize()
        {
            LevelService.WinGameEvent -= DisplayWinText;
        }

        private void DisplayWinText()
        {
            winText.SetActive(true);
        }
    }
}
