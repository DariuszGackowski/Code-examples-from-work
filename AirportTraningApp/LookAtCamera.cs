using UnityEngine;

namespace Module7
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _cameraTransform;
        public Vector3 Offset;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            Vector3 directionToCamera = _cameraTransform.position - transform.position;
            directionToCamera.y = 0;

            directionToCamera += Offset;

            transform.rotation = Quaternion.LookRotation(directionToCamera);
        }
    }
}
