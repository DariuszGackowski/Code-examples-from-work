using UI.CanvasManger;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Segments
{
    public class ScrollViewSegment : MonoBehaviour
    {
        public ScrollRect ScrollRect;
        private Vector2 _startPosition = new(0f, 1f);

        private void Awake() => Initialize();
        private void Initialize()
        {
            CanvasManagerBase.OnSegmentsInitialization.AddListener(SetupScrollViewSegment);
            CanvasManagerBase.OnSceneReload.AddListener(ResetScrollPosition);
        }
        private void ResetScrollPosition()
        {
            UpdateScrollPosition(_startPosition);
        }
        private void SetupScrollViewSegment()
        {
            CanvasManagerBase.OnScrollPositionChanged.AddListener(delegate { UpdateScrollPosition(CanvasManagerBase.SegementCanvasScrollsPosition); });

            ScrollRect.onValueChanged.AddListener(delegate { GetScrollPosition(); CanvasManagerBase.OnScrollPositionChanged.Invoke(CanvasManagerBase.SegementCanvasScrollsPosition); });
        }
        private void GetScrollPosition()
        {
            CanvasManagerBase.SegementCanvasScrollsPosition = ScrollRect.normalizedPosition;
        }
        private void UpdateScrollPosition(Vector2 position)
        {
            ScrollRect.normalizedPosition = position;
        }
    }
}