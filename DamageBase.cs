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
    public abstract class DamageBase : MonoBehaviour
    {
        [Header("Base Damage Area")]
        public DamageArea DamageArea;
        public GameObject MarkerObject;
        public bool IsReported;

        public bool MarkerIsAvailable => MarkerObject != null;
        public abstract void ApplyDamage();
    }
}
