using UnityEngine;

namespace IDAS
{
    public class ObjectDestroyer : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
