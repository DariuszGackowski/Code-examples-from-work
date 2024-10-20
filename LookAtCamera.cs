using UnityEngine;

namespace Module7
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform cameraTransform;

        private void Awake()
        {
            cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            Vector3 directionToCamera = cameraTransform.position - transform.position;
            directionToCamera.y = 0;

            transform.rotation = Quaternion.LookRotation(directionToCamera);
        }
    }
}
