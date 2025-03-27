using AllExtensions;
using Module7;
using UI.CanvasManger;
using UnityEngine;

namespace UI.Segments
{
    public class DamageModuleInfoSegment : ModuleInfoSegment
    {
        public GameObject ExamModeInfo;
        public GameObject TutorialModeInfo;

        public override void Initialize()
        {
            base.Initialize();
            CanvasManagerBase.OnSegmentsInitialization.AddListener(SetModeInfo);
        }
        private void SetModeInfo()
        {
            //if (DamageSceneStarter.IsTraining)
            //{
            //    ExamModeInfo.DeactivateObject();
            //    TutorialModeInfo.ActivateObject();
            //}
            //else
            //{
                TutorialModeInfo.DeactivateObject();
                ExamModeInfo.ActivateObject();
            //}
        }
    }
}
