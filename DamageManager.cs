using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Module7
{
    public class DamageManager : MonoBehaviour
    {
        [Header("Damages List")]
        [Space]
        public List<DamageBase> DamageList = new();

        [Header("Damage Chances Settings")]
        [Space]
        [Range(0f, 1f)] public float DamageChance = 0.5f;

        [ContextMenu("Generate Damage")]
        public void GenerateDamage()
        {
            if (DamageChance == 0f) return;

            HashSet<DamageArea> damagedAreas = new();

            foreach (var areaGroup in GetAllDamageAreaGroups())
            {
                foreach (DamageArea area in areaGroup)
                {
                    if (damagedAreas.Contains(area)) continue;

                    DamageBase damage = GetRandomDamageOption(area);

                    if (damage is null) continue;

                    ApplyDamage(damage);
                    damagedAreas.Add(area);
                }
            }
        }
        private IEnumerable<DamageArea[]> GetAllDamageAreaGroups()
        {
            return new List<DamageArea[]>
            {
                DamageAreaGroups.LandingGear,
                DamageAreaGroups.WingAreas,
                DamageAreaGroups.EngineAreas,
                DamageAreaGroups.FrontAreas,
                DamageAreaGroups.BackAreas
            };
        }

        private DamageBase GetRandomDamageOption(DamageArea area)
        {
            List<DamageBase> availableDamage = DamageList.Where(option => option.DamageArea == area).ToList();

            if (availableDamage.Count == 0) return null;

            int randomIndex = Random.Range(0, availableDamage.Count);
            DamageBase Damage = availableDamage[randomIndex];

            return (Random.value > DamageChance) ? null : availableDamage[randomIndex];
        }
        private void ApplyDamage(DamageBase damage)
        {
            if (damage == null)
            {
                Debug.LogError("Damage application problem. This damage is null.", damage.gameObject);
                return;
            }

            damage.ApplyDamage();

            Debug.Log($"Applied damage to {damage.DamageArea} for object: {damage.gameObject.name}", damage.gameObject);
        }
    }
}
