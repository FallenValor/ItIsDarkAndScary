/*****************************************************************************
// File Name : LevelService.cs
// Author : Brandon Koederitz
// Creation Date : 4/15/2026
// Last Modified : 4/15/2026
//
// Brief Description : Controls moving between levels when a tree is completed.
*****************************************************************************/
using NaughtyAttributes;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IDAS.Decisions
{
    public class LevelService : DecisionService
    {
        [SerializeField] private float deathDelay;
        [SerializeField, Scene] private string nextScene;
        [SerializeField, Scene] private string mainMenuScene;

        private SequencerService sequencer;

        /// <summary>
        /// Setup event references.
        /// </summary>
        protected override void Initialize()
        {
            sequencer = DecisionManager.GetService<SequencerService>();
            DecisionManager.GetService<DecisionTreeService>().TreeEndEvent += MoveToNextScene;
            HealthService.LoseGameEvent += OnDeath;
        }
        public override void Deinitialize()
        {
            DecisionManager.GetService<DecisionTreeService>().TreeEndEvent -= MoveToNextScene;
            HealthService.LoseGameEvent -= OnDeath;
        }

        /// <summary>
        /// Moves to the next scene specified.
        /// </summary>
        public void MoveToNextScene()
        {
            SceneManager.LoadScene(nextScene);
        }

        /// <summary>
        /// Returns to the main menu.
        /// </summary>
        public void MoveToMainMenu()
        {
            SceneManager.LoadScene(mainMenuScene);
        }

        /// <summary>
        /// Returns to the main menu after a specified death delay.
        /// </summary>
        private void OnDeath()
        {
            async Awaitable MoveToMainMenuWrapper(CancellationToken ct)
            {
                await Awaitable.WaitForSecondsAsync(deathDelay, ct);
                MoveToMainMenu();
            }
            // Queue the MoveToPoint call with the SequencerService.
            sequencer.QueueAction(MoveToMainMenuWrapper);
        }
    }
}
