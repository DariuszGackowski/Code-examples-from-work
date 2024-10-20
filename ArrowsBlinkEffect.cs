using UnityEngine;

namespace Module7
{
    public class ArrowsBlinkEffect : MonoBehaviour
    {
        public Color StartColor;
        public Color EndColor;
        [Range(0, 10)]
        public float duration = 1f;
        public Material pathArrowsMaterial;
        private void Awake() => InitializeColor();

        private void Update() => UpdateColorOverTime();
        private void InitializeColor()
        {
            pathArrowsMaterial.SetColor("_BaseColor", StartColor);
        }
        private void UpdateColorOverTime()
        {
            Color newColor = Color.Lerp(StartColor, EndColor, Mathf.PingPong(Time.time * duration, 1f));

            pathArrowsMaterial.SetColor("_BaseColor", newColor);
        }
    }
}
