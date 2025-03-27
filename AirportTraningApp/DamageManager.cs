using CustomLocalisation;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.CanvasManger;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Module7
{
    [RequireComponent(typeof(DamageHelper))]
    public class DamageManager : MonoBehaviour
    {
        public enum PlaneType
        {
            Airbus320,
            CRJ900,
            E170,
            B738, 
            B737F
        }

        [Header("Damage Segment List")]
        [Space]
        public List<DamageSegment> CurrentDamageSegmentList = new List<DamageSegment>();
        public List<DamageSegment> AddedDamageSegmentList = new List<DamageSegment>();

        [Header("Damage Chances Settings In Percents")]
        [Space]
        [Range(0f, 100f)] public int MainDamageChance = 50;
        [Range(0f, 100f)] public int FODChance = 50;
        [Range(0f, 100f)] public int MarkerChance = 50;

        private const string _scenarioDescShortID = "module7_name";
        private const string _criticalErrorsDescriptionID = "module7_errors";
        private const string _detailsHeaderID = "module7_detailsHeader";
        private const string _scenarioDescriptionID = "module7_scenarioDescription";
        private void Awake() => Setup();
        private void Setup()
        {
            CanvasManagerBase.OnInitializationScene.AddListener(InitializeDamage);
        }
        private void InitializeDamage()
        {
            AddedDamageSegmentList.Clear();
            CurrentDamageSegmentList.Clear();

            CurrentDamageSegmentList = DamageSceneStarter.ChosenPlane.DamageSegmentList.ToList();

            ResetDamageSegemnts();
            ResetDamage();
            GenerateDamage();
        }
        private void ResetDamageSegemnts()
        {
            foreach (DamageSegment damageSegment in CurrentDamageSegmentList)
            {
                damageSegment.ResetDamageSegment();
            }
        }
        private void ResetDamage()
        {
            if (CurrentDamageSegmentList.Count == 0) return;
            CurrentDamageSegmentList.SelectMany(damageSegment => damageSegment.BaseDamages).ToList().ForEach(baseDamage =>
            {
                baseDamage.ResetDamage();

                if (baseDamage is MarkedDecalDamage markedDecalDamage)
                {
                    markedDecalDamage.ResetMarker();
                }
            });
        }
        private void GenerateDamage()
        {
            GenerateMainDamage();
            GenerateAdditionalDamage();
        }
        private void GenerateAdditionalDamage()
        {
            //if (CurrentDamageSegmentList.Count == 0 || FODChance == 0f) return;

            //List<DamageSegment> chosenDamageSegmentList = CurrentDamageSegmentList.Where(damageSegment => damageSegment.IsFOD).ToList();

            //if (chosenDamageSegmentList.Count == 0) return;

            //int damageCountToGenerate = (int)Mathf.Round(Mathf.CeilToInt(chosenDamageSegmentList.Count * (FODChance / 100f)));
            //List<DamageSegment> randomChosenSegments = chosenDamageSegmentList.OrderBy(_ => Random.value).Take(damageCountToGenerate).ToList();
            //foreach (DamageSegment damageSegment in randomChosenSegments)
            //{
            //    BaseDamage damage = damageSegment.BaseDamages.FirstOrDefault();
            //    if (damage is null) continue;

            //    ApplyDamage(damage);
            //    AddedDamageSegmentList.Add(damageSegment);
            //}
        }
        private void GenerateMainDamage()
        {
            //if (MainDamageChance == 0f || CurrentDamageSegmentList.Count == 0) return;

            //List<DamageSegment> chosenDamageSegmentList = CurrentDamageSegmentList.Where(damageSegment => !damageSegment.IsFOD).ToList();

            //if (chosenDamageSegmentList.Count == 0) return;

            //int damageCountToGenerate = (int)Mathf.Round(Mathf.CeilToInt(chosenDamageSegmentList.Count * (MainDamageChance / 100f)));

            //List<DamageSegment> randomChosenSegments = chosenDamageSegmentList.OrderBy(_ => Random.value).Take(damageCountToGenerate).ToList();

            //List<MarkedDamage> markerDamagedAreas = new List<MarkedDamage>();
            //foreach (DamageSegment damageSegment in randomChosenSegments)
            //{
            //    List<BaseDamage> availableDamage = damageSegment.BaseDamages.Where(damage => damage is not CustomDamage || damage is AdditionalDamage).ToList();
            //    BaseDamage damage = GetRandomDamage(availableDamage);

            //    if (damage is null) continue;

            //    ApplyDamage(damage);
            //    AddedDamageSegmentList.Add(damageSegment);

            //    if (damage is MarkedDamage)
            //    {
            //        markerDamagedAreas.Add(damage as MarkedDamage);
            //    }
            //}

            //ApplyMarkers(markerDamagedAreas);
            //SetMarkerNumbers(markerDamagedAreas);
        }
        private void SetMarkerNumbers(List<MarkedDecalDamage> markerDamagedAreas)
        {
            HashSet<int> usedNumbers = new HashSet<int>();

            foreach (MarkedDecalDamage damage in markerDamagedAreas)
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
        private BaseDamage GetRandomDamage(List<BaseDamage> availableDamage)
        {
            if (availableDamage.Count == 0) return null;

            int randomIndex = Random.Range(0, availableDamage.Count);

            return availableDamage[randomIndex];
        }
        private void ApplyMarkers(List<MarkedDecalDamage> markedDamages)
        {
            if (markedDamages.Count == 0 || MarkerChance == 0f) return;

            int countToMark = (int)Mathf.Round(Mathf.CeilToInt(markedDamages.Count * (MarkerChance / 100f)));
            List<MarkedDecalDamage> damagesToMark = markedDamages.OrderBy(_ => Random.value).Take(countToMark).ToList();

            foreach (MarkedDecalDamage damage in damagesToMark)
            {
                ApplyMarker(damage);
            }
        }
        private void ApplyDamage(BaseDamage damage)
        {
            if (damage == null)
            {
                Debug.LogError("Damage apply problem. This damage is null.", damage.gameObject);
                return;
            }

            damage.ApplyDamage();
            Debug.Log($"Applied damage for object: {damage.gameObject.name}", damage.gameObject);
        }
        private void ApplyMarker(MarkedDecalDamage damage)
        {
            if (damage == null)
            {
                Debug.LogError("Damage application problem. This damage is null.", damage.gameObject);
                return;
            }

            damage.ApplyMarker();
            Debug.Log($"Applied marker to object: {damage.gameObject.name}", damage.gameObject);
        }
        public void PrepareSummary()
        {
            ScoreData score = new()
            {
                ScenarioDescription = _scenarioDescriptionID,
                ScenarioDescShort = _scenarioDescShortID,
                DetailsHeader = _detailsHeaderID,
                CriticalErrorsDescription = _criticalErrorsDescriptionID,
                Date = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString()
            };

            MPPlayer player = FindObjectOfType<MPPlayer>();
            if(player != null)
            {
                score.WorkersNames = player.playerName.Value.ToString();
            }

            DamageSummary(ref score, CurrentDamageSegmentList);

            AirportData.LastScoreData = score;
        }
        private void DamageSummary(ref ScoreData score, List<DamageSegment> damageSegmentList)
        {
            foreach (DamageSegment damageSegment in damageSegmentList)
            {
                if (!damageSegment.IsAdded && damageSegment.IssueIsActive)
                {
                    score.DetailsDescription += $"{CustomLocalisationSettings.ScoreStopSign}{damageSegment.ClearErrorTextID}{CustomLocalisationSettings.ScoreStopSign}\n";
                }

                if (!damageSegment.IsAdded) continue;

                foreach (BaseDamage baseDamage in damageSegment.BaseDamages)
                {
                    if (!baseDamage.IsAdded) continue;

                    switch (baseDamage)
                    {
                        case AdditionalDamage additionalDamage when baseDamage is AdditionalDamage:                          
                            if (!damageSegment.IssueIsActive)
                            {
                                score.CriticalErrors += $"{CustomLocalisationSettings.ScoreStopSign}{additionalDamage.IssueNotMarkedInfoID}{CustomLocalisationSettings.ScoreStopSign}\n";
                            }
                            else
                            {
                                score.DetailsDescription += $"{CustomLocalisationSettings.ScoreStopSign}{additionalDamage.IssueMarkedInfoID}{CustomLocalisationSettings.ScoreStopSign}\n";
                            }
                            break;
                        case DisplacedDamage displacedDamage:
                            if (!damageSegment.IssueIsActive)
                            {
                                score.CriticalErrors += $"{CustomLocalisationSettings.ScoreStopSign}{displacedDamage.IssueNotMarkedInfoID}{CustomLocalisationSettings.ScoreStopSign}\n";
                            }
                            else
                            {
                                score.DetailsDescription += $"{CustomLocalisationSettings.ScoreStopSign}{displacedDamage.IssueMarkedInfoID}{CustomLocalisationSettings.ScoreStopSign}\n";
                            }
                            break;
                        case MarkedDecalDamage markedDecalDamage:
                            if (markedDecalDamage.IsReported && damageSegment.IssueIsActive)
                            {
                                score.DetailsDescription += $"{CustomLocalisationSettings.ScoreStopSign}{markedDecalDamage.IssueOverMarkedInfoID}{CustomLocalisationSettings.ScoreStopSign}\n";
                            }
                            else if (!markedDecalDamage.IsReported && !damageSegment.IssueIsActive)
                            {
                                score.CriticalErrors += $"{CustomLocalisationSettings.ScoreStopSign}{markedDecalDamage.IssueNotMarkedInfoID}{CustomLocalisationSettings.ScoreStopSign}\n";
                            }
                            else if (!markedDecalDamage.IsReported && damageSegment.IssueIsActive)
                            {
                                score.DetailsDescription += $"{CustomLocalisationSettings.ScoreStopSign}{markedDecalDamage.IssueMarkedInfoID}{CustomLocalisationSettings.ScoreStopSign}\n";
                            }
                            break;

                        default:
                            Debug.Log("This is a different type of BaseDamage.", baseDamage.gameObject);
                            break;
                    }
                }
            }
        }
    }
}