/*****************************************************************************
// File Name : EndingUIService.cs
// Author : Brandon Koederitz
// Creation Date : 4/15/2026
// Last Modified : 4/15/2026
//
// Brief Description : Displays UI for when the player dies game.
*****************************************************************************/
using IDAS.Decisions;
using System.Collections;
using TMPro;
using UnityEngine;

namespace IDAS.UI
{
    public class DeathUIService : UIService
    {
        [SerializeField] private TMP_Text deathText;
        [SerializeField] private float animationDuration;
        [SerializeField] private float minFontSize;
        [SerializeField] private float maxFontSize;
        [SerializeField] private AnimationCurve opacityCurve;
        [SerializeField] private AnimationCurve fontSizeCurve;

        protected override void Initialize()
        {
            HealthService.LoseGameEvent += DisplayDeathText;
        }

        public override void Deinitialize()
        {
            HealthService.LoseGameEvent -= DisplayDeathText;
        }

        private void DisplayDeathText()
        {
            StartCoroutine(DeathTextAnimation());
        }

        private IEnumerator DeathTextAnimation()
        {
            deathText.gameObject.SetActive(true);
            deathText.color = SetAlpha(deathText.color, 0);
            deathText.fontSize = minFontSize;
            float timer = 0f;
            while(timer < animationDuration)
            {
                float normalizedTime = timer / animationDuration;

                deathText.color = SetAlpha(deathText.color, opacityCurve.Evaluate(normalizedTime));
                deathText.fontSize = Mathf.Lerp(minFontSize, maxFontSize, fontSizeCurve.Evaluate(normalizedTime));

                timer += Time.deltaTime;
                yield return null;
            }
        }

        private Color SetAlpha(Color col, float alpha)
        {
            col.a = alpha;
            return col;
        }
    }
}
