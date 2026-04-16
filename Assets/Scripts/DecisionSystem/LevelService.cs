/*****************************************************************************
// File Name : LevelService.cs
// Author : Brandon Koederitz
// Creation Date : 4/15/2026
// Last Modified : 4/15/2026
//
// Brief Description : Controls moving between levels when a tree is completed.
*****************************************************************************/
using NaughtyAttributes;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IDAS.Decisions
{
    public class LevelService : DecisionService
    {
        [SerializeField] private bool isVictory;
        [SerializeField, Scene, HideIf("isVictory")] private string nextScene;
        [SerializeField] private float sceneTransitionDelay;
        [SerializeField, Scene] private string mainMenuScene;


        private SequencerService sequencer;

        public static event Action WinGameEvent;

        /// <summary>
        /// Setup event references.
        /// </summary>
        protected override void Initialize()
        {
            sequencer = DecisionManager.GetService<SequencerService>();
            DecisionManager.GetService<DecisionTreeService>().TreeEndEvent += MoveToNextSceneDelayed;
            HealthService.LoseGameEvent += MoveToMainMenuDelayed;
        }
        public override void Deinitialize()
        {
            DecisionManager.GetService<DecisionTreeService>().TreeEndEvent -= MoveToNextSceneDelayed;
            HealthService.LoseGameEvent -= MoveToMainMenuDelayed;
        }

        /// <summary>
        /// Moves to the next scene specified.
        /// </summary>
        public void MoveToNextScene()
        {
            if (isVictory)
            {
                WinGameEvent?.Invoke();
                MoveToMainMenuDelayed();
            }
            else
            {
                SceneManager.LoadScene(nextScene);
            }
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
        private void MoveToMainMenuDelayed()
        {
            async Awaitable MoveToMainMenuWrapper(CancellationToken ct)
            {
                try
                {
                    await Awaitable.WaitForSecondsAsync(sceneTransitionDelay, ct);
                }
                catch (OperationCanceledException)
                {
                    Debug.LogWarning("Operation Cancelled");
                }
                MoveToMainMenu();
            }
            // Queue the MoveToPoint call with the SequencerService.
            sequencer.QueueAction(MoveToMainMenuWrapper);
        }

        /// <summary>
        /// Returns to the main menu after a specified death delay.
        /// </summary>
        private void MoveToNextSceneDelayed()
        {
            async Awaitable MoveToNextSceneWrapper(CancellationToken ct)
            {
                try
                {
                    await Awaitable.WaitForSecondsAsync(sceneTransitionDelay, ct);
                }
                catch (OperationCanceledException)
                {
                    Debug.LogWarning("Operation Cancelled");
                }
                MoveToNextScene();
            }
            // Queue the MoveToPoint call with the SequencerService.
            sequencer.QueueAction(MoveToNextSceneWrapper);
        }
    }
}
