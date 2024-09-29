using UnityEngine;

namespace Module7
{
    public static class DamageExtensions
    {
        public static void DeactivateObject(this GameObject mesh)
        {
            mesh.SetActive(false);
        }
        public static void ActivateObject(this GameObject mesh)
        {
            mesh.SetActive(true);
        }
        public static void SetMaterial(this MeshRenderer meshRenderer, Material material)
        {
            meshRenderer.material = material;
        }
    }
}
