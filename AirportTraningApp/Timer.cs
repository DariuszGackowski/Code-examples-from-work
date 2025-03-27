using UnityEngine;
using UI.CanvasManger;

namespace XR.Watch
{
    public class Timer : MonoBehaviour
    {
        public static float CurrentTime;
        public static string CurrentTimeInMinutes => CountTimeToMinutes(_startTime - CurrentTime);
        private static float _startTime;
        private readonly float _endTime;
        private void Update() => CountTime();
        private void CountTime() 
        {
            if (!CanvasManagerBase.IsStarted) return; 

            CurrentTime -= Time.deltaTime;
            DisplayTime(CurrentTime);

            if (CurrentTime <= _endTime)
            {
                CanvasManagerBase.OnSceneExit.Invoke();
            }
        }
        public void Setup(float time) 
        {
            CurrentTime = time;
            _startTime = time;

            CanvasManagerBase.OnSceneReload.AddListener(ResetTime);
        }
        public void ResetTime() 
        {
            CurrentTime = _startTime;
        }
        private static string CountTimeToMinutes(float timeToDisplay) 
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            return string.Format(" {0:00}:{1:00}", minutes, seconds);
        }
        private void DisplayTime(float timeToDisplay)
        {
            string timeText = CountTimeToMinutes(timeToDisplay);

            CanvasManagerBase.OnTimeChanged.Invoke(timeText);
        }
    }
}