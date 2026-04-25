/*****************************************************************************
// File Name : DamageAudioRelay.cs
// Author : Brandon Koederitz
// Creation Date : 4/21/2026
// Last Modified : 4/21/2026
//
// Brief Description : Plays sounds for taking damage.
*****************************************************************************/
using IDAS.Decisions;
using UnityEngine;

namespace IDAS.Audio
{
    public class DamageAudioRelay : AudioRelay
    {
        [SerializeField] private string damageSound;

        private void Awake()
        {
            HealthService hs = GetComponent<HealthService>();
            hs.HealthChangedEvent += PlayDamageSound;
        }

        private void OnDestroy()
        {
            HealthService hs = GetComponent<HealthService>();
            hs.HealthChangedEvent -= PlayDamageSound;
        }

        private void PlayDamageSound(int change, int health, float normalizedHealth)
        {
            if(change < 0)
            {
                PlayOneShot(damageSound);
            }
        }
    }
}
