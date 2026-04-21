/*****************************************************************************
// File Name : HealthFlashService.cs
// Author : Brandon Koederitz
// Creation Date : 4/20/2026
// Last Modified : 4/20/2026
//
// Brief Description : Causes a damage flash effect to play when the player loses health.
*****************************************************************************/
using IDAS.Decisions;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace IDAS.VFX
{
    public class HealthFlashService : Service
    {
        [SerializeField] private VolumeProfile volumeProfile;
        [SerializeField] private Color hurtFlashColor = Color.red;
        [SerializeField] private float hurtFlashTime;
        [SerializeField, Range(0, 1)] private float flashIntensity;
        [SerializeField] private AnimationCurve hurtFlashCurve;

        private HealthService healthService;
        private Vignette vignetteSettings;
        private Coroutine flashCoroutine;

        protected override void Initialize()
        {
            healthService = AppManager.GetManager<DecisionManager>().GetService<HealthService>();
            healthService.HealthChangedEvent += DamageFlash;

            volumeProfile.TryGet<Vignette>(out vignetteSettings);
        }
        public override void Deinitialize()
        {
            healthService.HealthChangedEvent -= DamageFlash;
        }

        private void DamageFlash(int change, int health, float normalizedHealth)
        {
            if (change < 0)
            {
                if (flashCoroutine != null)
                {
                    StopCoroutine(flashCoroutine);
                    flashCoroutine = null;
                }
                flashCoroutine = StartCoroutine(DamageFlashRoutine(hurtFlashTime, flashIntensity, hurtFlashColor, hurtFlashCurve));
            }
        }

        private IEnumerator DamageFlashRoutine(float flashTime, float intensity, Color flashColor, AnimationCurve curve)
        {
            float timer = 0;
            Color startColor = vignetteSettings.color.value;
            float startIntensity = vignetteSettings.intensity.value;

            while (timer < flashTime)
            {
                float normalizedTime = timer / flashTime;
                vignetteSettings.color.value = Color.Lerp(startColor, flashColor, curve.Evaluate(normalizedTime));
                vignetteSettings.intensity.value = Mathf.Lerp(startIntensity, intensity, curve.Evaluate(normalizedTime));   

                timer += Time.deltaTime;
                yield return null;
            }
            vignetteSettings.color.value = startColor;
            vignetteSettings.intensity.value = startIntensity;
            flashCoroutine = null;
        }
    }
}
