using System.Collections.Generic;
using UnityEngine;

namespace Module7
{
    public class DamageHelper : MonoBehaviour
    {
        private DamageManager _damageManager;

        private DamageManager DamageManager 
        {
            get
            {
                if (_damageManager == null)
                {
                    _damageManager = FindFirstObjectByType<DamageManager>();
                }
                return _damageManager;
            }
        }

        public void CheckDamage()
        {
            if (DamageManager == null || DamageManager.AircraftDamageList.Count == 0) return;

            List<AircraftDamage> damageList = DamageManager.AircraftDamageList;

            foreach (AircraftDamage damage in damageList)
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

                if (!damage.IssueIsAvailable)
                {
                    Debug.Log($"Issue is not added to:{damage.gameObject.name}.", damage.gameObject);
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
