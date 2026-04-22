/*****************************************************************************
// File Name : DecisionAudioRelay.cs
// Author : Brandon Koederitz
// Creation Date : 4/21/2026
// Last Modified : 4/21/2026
//
// Brief Description : Plays different sounds when the player makes decisions.
*****************************************************************************/
using IDAS.Decisions;
using IDAS.Decisions.Tree;
using UnityEngine;

namespace IDAS.Audio
{
    public class DecisionAudioRelay : AudioRelay
    {
        [SerializeField] private string staminaChoiceSound;
        [SerializeField] private string normalChoiceSound;
        private void Awake()
        {
            DecisionTreeService dts = GetComponent<DecisionTreeService>();
            dts.MakeDecisionEvent += PlayDecisionSound;
        }

        private void OnDestroy()
        {
            DecisionTreeService dts = GetComponent<DecisionTreeService>();
            dts.MakeDecisionEvent -= PlayDecisionSound;
        }

        private void PlayDecisionSound(DarkScaryNode currentNode, int decisionIndex, DarkScaryNode nextNode)
        {
            if (currentNode is DecisionNode dNode)
            {
                Choice choice = dNode.Choices[decisionIndex];
                if (choice.Stamina > 0)
                {
                    PlayOneShot(staminaChoiceSound);
                }
                else
                {
                    PlayOneShot(normalChoiceSound);
                }
            }
            
        }
    }
}
