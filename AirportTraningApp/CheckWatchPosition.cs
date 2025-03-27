using UnityEngine;

namespace Module7
{
    public class CheckWatchPosition : MonoBehaviour
    {
        [ContextMenu("CheckPosition")]
        public void CheckPosition() 
        {
            Debug.Log(transform.rotation);
        }
    }
}
