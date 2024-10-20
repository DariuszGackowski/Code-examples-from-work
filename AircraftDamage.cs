using TMPro;
using UnityEngine;

namespace Module7
{
    public enum DamageArea
    {
        // Nose and front
        Nose,
        LeftFrontSide,
        RightFrontSide,

        //Wing areas
        LeftWingFrontEdge,
        RightWingFrontEdge,
        LeftWingTip,
        RightWingTip,
        LeftTrailingEdge,
        RightTrailingEdge,

        //Engine
        LeftEngine,
        RightEngine,

        // Landing gear
        LeftMainLandingGearStrut,
        RightMainLandingGearStrut,
        FrontLandingGearStrut,

        //Stabilizers and Back
        LeftBackSide,
        RightBackSide,
        Stabilizers
    }

    public enum DamageType
    {
        Hail,
        Bird,
        Lightning
    }
    public class DamageAreaGroups
    {
        public static DamageArea[] LandingGear =
        {
            DamageArea.LeftMainLandingGearStrut,
            DamageArea.RightMainLandingGearStrut,
            DamageArea.FrontLandingGearStrut
        };

        public static DamageArea[] WingAreas =
        {
            DamageArea.LeftWingFrontEdge,
            DamageArea.RightWingFrontEdge,
            DamageArea.LeftWingTip,
            DamageArea.RightWingTip,
            DamageArea.LeftTrailingEdge,
            DamageArea.RightTrailingEdge
        };

        public static DamageArea[] EngineAreas =
        {
            DamageArea.LeftEngine,
            DamageArea.RightEngine
        };

        public static DamageArea[] FrontAreas =
        {
            DamageArea.Nose,
            DamageArea.LeftFrontSide,
            DamageArea.RightFrontSide
        };

        public static DamageArea[] BackAreas =
        {
            DamageArea.LeftBackSide,
            DamageArea.RightBackSide,
            DamageArea.Stabilizers
        };

        public static DamageArea[][] AllGroups =
        {
            LandingGear,
            WingAreas,
            EngineAreas,
            FrontAreas,
            BackAreas
        };
    }

    [System.Serializable]
    public abstract class AircraftDamage : DamageBase
    {
        [Header("Aircraft Damage Area")]
        public DamageArea DamageArea;
        public DamageType DamageType;
        public GameObject MarkerObject;
        public TextMeshProUGUI NumberText;
        [TextArea]
        public string MarkedCriticalErrorText;
        public bool IsReported => MarkerObject.activeSelf;
        public bool MarkerIsAvailable => MarkerObject != null;
        public bool NumberTextIsAvailable => NumberText != null;
        protected void SetMarkerObject()
        {
            MarkerObject.ActivateObject();
        }
        protected void UnsetMarkerObject()
        {
            MarkerObject.DeactivateObject();
        }
        protected void SetNumberObject(int value)
        {
            NumberText.SetText(value.ToString());
        }
        protected void UnsetNumberObject()
        {
            NumberText.SetText(0.ToString());
        }
        public void ApplyNumber(int value)
        {
            if (!NumberTextIsAvailable)
            {
                Debug.LogError("Applying number in text is not possible.", gameObject);
                return;
            }

            SetNumberObject(value);
        }
        public void ResetNumber()
        {
            if (!NumberTextIsAvailable)
            {
                Debug.LogError("Reset number in text is not possible.", gameObject);
                return;
            }

            UnsetMarkerObject();
        }
        public void ApplyMarker()
        {
            if (!MarkerIsAvailable)
            {
                Debug.LogError("Applying marker is not possible.", gameObject);
                return;
            }

            SetMarkerObject();
        }
        public void ResetMarker()
        {
            if (!MarkerIsAvailable)
            {
                Debug.LogError("Reset marker is not possible.", gameObject);
                return;
            }

            UnsetMarkerObject();
        }
    }
}
