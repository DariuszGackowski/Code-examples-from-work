using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Module7
{
    [System.Serializable]
    public abstract class DamageBase : MonoBehaviour
    {
        [Header("Base Damage Area")]
        public GameObject IssueCanvasObject;
        public LineRenderer LineRenderer;
        public Transform StartPoint;
        public Transform EndPoint;
        [TextArea]
        public string CriticalErrorText;
        [TextArea]
        public string DetailsDescriptionText;

        public bool IssueIsAvailable => IssueCanvasObject != null;
        public bool IssueIsActive => IssueCanvasObject.activeSelf;
        public bool LineRendererIsAvailable => LineRenderer != null;
        public bool LineRendererIsActive => LineRenderer.gameObject.activeSelf;
        public bool LineRendererPointsAreAvailable => StartPoint != null && EndPoint != null;
        public bool DamageIsActive => gameObject.activeSelf;
        public abstract void ApplyDamage();
        public abstract void ResetDamage();
        public abstract void HoverDamageEnter();
        public abstract void HoverDamageExit();
        protected void SetIssueObject()
        {
            IssueCanvasObject.ActivateObject();
        }
        protected void UnsetIssueObject()
        {
            IssueCanvasObject.DeactivateObject();
        }
        protected void SetLineRendererObject()
        {
            LineRenderer.gameObject.ActivateObject();
        }
        protected void UnsetLineRendererObject()
        {
            LineRenderer.gameObject.DeactivateObject();
        }
        private void EnableLineRenderer()
        {
            if (!LineRendererIsAvailable)
            {
                Debug.LogError("Enabling lineRenderer is not possible.", gameObject);
                return;
            }

            SetLineRendererObject();
        }
        public void DisableLineRenderer()
        {
            if (!LineRendererIsAvailable)
            {
                Debug.LogError("Disabling lineRenderer is not possible.", gameObject);
                return;
            }

            UnsetLineRendererObject();
        }
        private void EnableIssue()
        {
            if (!IssueIsAvailable)
            {
                Debug.LogError("Enabling issue is not possible.", gameObject);
                return;
            }

            SetIssueObject();
            SetLineRendererObject();
        }
        public void DisableIssue()
        {
            if (!IssueIsAvailable)
            {
                Debug.LogError("Disabling issue is not possible.", gameObject);
                return;
            }

            UnsetIssueObject();
            UnsetLineRendererObject();
        }
        public void ToggleIssue()
        {
            if (!IssueIsAvailable) return;

            if (IssueIsActive)
                DisableIssue();
            else
                EnableIssue();
        }
        public void SetupLineRenderer()
        {
            if (!LineRendererPointsAreAvailable)
            {
                Debug.LogError("Setup of lineRenderer points is not possible.", gameObject);
                return;
            }
            LineRenderer.SetPosition(0, StartPoint.position);
            LineRenderer.SetPosition(1, EndPoint.position);
        }
    }
}
