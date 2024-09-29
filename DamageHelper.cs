using System.Collections.Generic;
using UnityEngine;

namespace Module7
{
    public class DamageHelper : MonoBehaviour
    {
        public DamageManager DamageManager;

        public void CheckDamage()
        {
            if (DamageManager == null || DamageManager.DamageList.Count == 0) return;

            List<DamageBase> damageList = DamageManager.DamageList;

            foreach (DamageBase damage in damageList)
            {
                if (damage == null)
                {
                    Debug.Log("Some damage on list is null.");
                    continue;
                }

                if (!damage.MarkerIsAvailable)
                {
                    Debug.Log($"Marker is not added to:{damage.gameObject.name}.", damage.gameObject);
                }
                if (damage is DecalDamage decalDamage && !decalDamage.IsAvailable)
                {
                    Debug.Log($"Decal damage is not added correctly to:{decalDamage.gameObject.name}.", decalDamage.gameObject);
                }
                else if (damage is TextureDamage textureDamage && !textureDamage.IsAvailable)
                {
                    Debug.Log($"Texture damage is not added correctly to:{textureDamage.gameObject.name}.", textureDamage.gameObject);
                }
                else if (damage is MeshDamage meshDamage && !meshDamage.IsAvailable)
                {
                    Debug.Log($"Mesh damage is not added correctly to:{meshDamage.gameObject.name}.", meshDamage.gameObject);
                }
            }
        }
    }
}
