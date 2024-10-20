using UnityEngine;
using UnityEngine.Events;

namespace Module7
{
    public static class DamageExtensions
    {
        public static void SetTransform(this Transform transform, Transform transformToSet, bool isScaleToSet)
        {
            transform.position = transformToSet.position;
            transform.rotation = transformToSet.rotation;

            if (isScaleToSet)
                transform.localScale = transformToSet.localScale;
        }
        public static void DeactivateObject(this GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
        public static void ActivateObject(this GameObject gameObject)
        {
            gameObject.SetActive(true);
        }
        public static void SetMaterial(this MeshRenderer meshRenderer, Material material)
        {
            meshRenderer.material = material;
        }
        public static void SetBoolProperty(this Material material, string propertyName, bool boolValue)
        {
            material.SetFloat(propertyName, boolValue ? 1f : 0f);
        }
        public static void AddListener(this UnityEvent unityEvent, UnityAction unityAction, bool removeListenerBeforeAddNew)
        {
            if (removeListenerBeforeAddNew)
                unityEvent.RemoveListener(unityAction);

            unityEvent.AddListener(unityAction);
        }
    }
}
