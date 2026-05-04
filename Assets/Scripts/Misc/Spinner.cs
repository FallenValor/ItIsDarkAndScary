using System.Collections;
using UnityEngine;

namespace IDAS
{
    public class Spinner : MonoBehaviour
    {
        [SerializeField] private float spinRate;
        public void StartSpin()
        {
            StartCoroutine(SpinRoutine());
        }

        private IEnumerator SpinRoutine()
        {
            while(true)
            {
                transform.Rotate(spinRate * Time.deltaTime * Vector3.up);
                yield return null;
            }
        }
    }
}
