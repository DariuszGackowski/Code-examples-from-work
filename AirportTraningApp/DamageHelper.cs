using System.Collections.Generic;
using System.Linq;
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
            if (DamageManager == null || DamageManager.CurrentDamageSegmentList.Count == 0) return;

            List<BaseDamage> damageList = DamageManager.CurrentDamageSegmentList.SelectMany(damageSegment => damageSegment.BaseDamages).ToList();

            foreach (BaseDamage damage in damageList)
            {
                if (damage == null)
                {
                    Debug.Log("Some damage on list is null.");
                    continue;
                }
                damage.CheckDamageSetup();
            }
        }
    }
}