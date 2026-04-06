/*****************************************************************************
// File Name : SequencerService.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : Controls the sequence of moving through decision points on the tree.
*****************************************************************************/
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IDAS.Decisions
{
    public class InputService : DecisionService
    {
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private string decisionActionMap;

        private InputActionMap map;

        public bool Listening { get; set; }

        public event Action<int> DecisionInputEvent;

        /// <summary>
        /// Initializes all decision actions.
        /// </summary>
        /// <returns></returns>
        protected override void Initialize()
        {
            Listening = true;
            // Subscribes each action to the same function.
            inputActions.Enable();
            map = inputActions.FindActionMap(decisionActionMap);
            foreach (var action in map.actions)
            {
                action.performed += OnDecisionInput;
            }
        }

        /// <summary>
        /// Removes all event subscriptions.
        /// </summary>
        /// <returns></returns>
        public override void Deinitialize()
        {
            foreach (var action in map.actions)
            {
                action.performed -= OnDecisionInput;
            }
        }

        /// <summary>
        /// Manages recieving a decision based input.
        /// </summary>
        /// <param name="callbackContext"></param>
        private void OnDecisionInput(InputAction.CallbackContext callbackContext)
        {
            if (!Listening) { return; }
            // Get the index of the InputAction that was triggered and output the index of the action.
            int index = map.actions.IndexOf((item) => item == callbackContext.action);
            DecisionInputEvent?.Invoke(index);
        }
    }
}