using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Module7
{
    [RequireComponent(typeof(DamageHelper))]
    public class DamageManager : MonoBehaviour
    {
        [Header("Damages List")]
        [Space]
        public List<AircraftDamage> AircraftDamageList = new();
        public List<AdditionalDamage> AdditionalDamageList = new();

        [Header("Damage Chances Settings")]
        [Space]
        [Range(0f, 1f)] public float AircraftDamageChance = 0.5f;
        [Range(0f, 1f)] public float MarkerChance = 0.5f;

        [Header("DamageManager Setup")]
        [Space]
        public GameObject SummaryTransitionObject;
        [HideInInspector]
        public UnityEvent OnSummaryEnable;
        public void InitializeDamage()
        {
            ResetDamage();
            SetupIssueLineRenderer();
            GenerateDamage();
        }
        private void ResetDamage()
        {
            if (AircraftDamageList.Count == 0) return;

            foreach (AircraftDamage damage in AircraftDamageList)
            {
                damage.ResetMarker();
                damage.ResetDamage();
                damage.DisableIssue();
            }
        }
        private void SetupIssueLineRenderer()
        {
            foreach (AircraftDamage damage in AircraftDamageList)
            {
                damage.SetupLineRenderer();
            }
        }
        private void GenerateDamage()
        {
            if (AircraftDamageChance == 0f) return;

            HashSet<DamageArea> damagedAreas = new();
            List<AircraftDamage> damagedAreasToAddNumber = new();
            IEnumerable<DamageArea[]> allDamageAreaGroups = GetAllDamageAreaGroups();

            foreach (var areaGroup in allDamageAreaGroups)
            {
                foreach (DamageArea area in areaGroup)
                {
                    if (damagedAreas.Contains(area)) continue;

                    AircraftDamage damage = GetRandomDamageOption(area);

                    if (damage is null) continue;

                    if (IsMarkerToEnable())
                        ApplyMarker(damage);

                    ApplyDamage(damage);
                    damagedAreas.Add(area);
                    damagedAreasToAddNumber.Add(damage);
                }
            }

            SetNumberToMarkers(damagedAreasToAddNumber);
        }
        private void SetNumberToMarkers(List<AircraftDamage> damagedAreasToAddNumber)
        {
            HashSet<int> usedNumbers = new();

            foreach (AircraftDamage damage in damagedAreasToAddNumber)
            {
                int value = GenerateUniqueRandomNumber(usedNumbers);
                damage.ApplyNumber(value);
            }
        }

        private int GenerateUniqueRandomNumber(HashSet<int> usedNumbers)
        {
            if (usedNumbers.Count >= 99)
                Debug.LogError("Nie ma wiêcej unikalnych liczb do wykorzystania.", gameObject);


            int number;
            do
            {
                number = Random.Range(1, 100);
            } while (usedNumbers.Contains(number));

            usedNumbers.Add(number);
            return number;
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
        private AircraftDamage GetRandomDamageOption(DamageArea area)
        {
            List<AircraftDamage> availableDamage = AircraftDamageList.Where(option => option.DamageArea == area).ToList();

            if (availableDamage.Count == 0) return null;

            int randomIndex = Random.Range(0, availableDamage.Count);

            return (Random.value > AircraftDamageChance) ? null : availableDamage[randomIndex];
        }
        private bool IsMarkerToEnable()
        {
            return Random.value <= MarkerChance;
        }
        private void ApplyDamage(AircraftDamage damage)
        {
            if (damage == null)
            {
                Debug.LogError("Damage application problem. This damage is null.", damage.gameObject);
                return;
            }

            damage.ApplyDamage();
            Debug.Log($"Applied damage to {damage.DamageArea} for object: {damage.gameObject.name}", damage.gameObject);
        }
        private void ApplyMarker(AircraftDamage damage)
        {
            if (damage == null)
            {
                Debug.LogError("Damage application problem. This damage is null.", damage.gameObject);
                return;
            }

            damage.ApplyMarker();
            Debug.Log($"Applied marker to object: {damage.gameObject.name}", damage.gameObject);
        }

        [ContextMenu("SetSummary")]
        public void PrepareSummary()
        {
            ScoreData score = new();
            score.ScenarioDescription = "Scenariusz inspekcji poszycia - podsumowanie";
            score.DetailsHeader = "Lista wygenerowanych uszkodzeñ";

            foreach (AircraftDamage damage in AircraftDamageList)
            {
                if (!damage.DamageIsActive) continue;

                if (damage.IsReported && damage.IssueIsActive)
                {
                    score.CriticalErrors += $"[B³¹d krytyczny] Zg³oszone uszkodzenie samolotu przez {GetDamageTypeText(damage.DamageType)} na podobszarze {GetDamageAreaText(damage.DamageArea)} dla obszaru {GetDamageAreaGroupText(damage.DamageArea)} zosta³o zg³oszone \n";
                }
                else if (!damage.IsReported && !damage.IssueIsActive)
                {
                    score.CriticalErrors += $"[B³¹d krytyczny] Uszkodzenie samolotu przez {GetDamageTypeText(damage.DamageType)} na podobszarze {GetDamageAreaText(damage.DamageArea)}dla obszaru {GetDamageAreaGroupText(damage.DamageArea)} nie zosta³o zg³oszone \n";
                }

                score.DetailsDescription += $"Wyst¹pi³o uszkodzene samolotu przez {GetDamageTypeText(damage.DamageType)} na podobszarze {GetDamageAreaText(damage.DamageArea)} dla obszaru {GetDamageAreaGroupText(damage.DamageArea)}. \n";
            }
            AirportData.LastScoreData = score;

            SummaryTransitionObject.ActivateObject();

            OnSummaryEnable.Invoke();
        }
        private string GetDamageTypeText(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Hail:
                    return "grad";
                case DamageType.Bird:
                    return "uderzenie ptaka";
                case DamageType.Lightning:
                    return "uderzenie b³yskawicy";
                default:
                    return "nieznanego sprawcê";
            }
        }
        private string GetDamageAreaText(DamageArea damageArea)
        {
            switch (damageArea)
            {
                case DamageArea.Nose:
                    return "nosa";
                case DamageArea.LeftFrontSide:
                    return "przedniego lewego boku";
                case DamageArea.RightFrontSide:
                    return "przedniego prawego boku";
                case DamageArea.LeftWingFrontEdge:
                    return "przedniej krawêdzi lewego skrzyd³a";
                case DamageArea.RightWingFrontEdge:
                    return "przedniej krawêdzi prawego skrzyd³a";
                case DamageArea.LeftWingTip:
                    return "lewego wingletu";
                case DamageArea.RightWingTip:
                    return "prawego wingletu";
                case DamageArea.LeftTrailingEdge:
                    return "krawêdzi sp³ywu lewego skrzyd³a";
                case DamageArea.RightTrailingEdge:
                    return "krawêdzi sp³ywu prawego skrzyd³a";
                case DamageArea.LeftEngine:
                    return "lewego silnika";
                case DamageArea.RightEngine:
                    return "prawego silnika";
                case DamageArea.LeftMainLandingGearStrut:
                    return "lewego g³ównego podwozia";
                case DamageArea.RightMainLandingGearStrut:
                    return "prawego g³ównego podwozia";
                case DamageArea.FrontLandingGearStrut:
                    return "przedniego podwozia";
                case DamageArea.LeftBackSide:
                    return "tylnego lewego boku";
                case DamageArea.RightBackSide:
                    return "tylnego prawego boku";
                case DamageArea.Stabilizers:
                    return "stabilizatorów";
                default:
                    return "nieznanym";
            }
        }
        public string GetDamageAreaGroupText(DamageArea damageArea)
        {
            if (DamageAreaGroups.LandingGear.Contains(damageArea))
                return "podwozia";
            if (DamageAreaGroups.WingAreas.Contains(damageArea))
                return "skrzyde³";
            if (DamageAreaGroups.EngineAreas.Contains(damageArea))
                return "silnika";
            if (DamageAreaGroups.FrontAreas.Contains(damageArea))
                return "przedniego";
            if (DamageAreaGroups.BackAreas.Contains(damageArea))
                return "tylnego";

            return "nieznanego";
        }
    }
}
