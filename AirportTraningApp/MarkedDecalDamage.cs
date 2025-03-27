using AllExtensions;
using Module7;
using TMPro;
using UnityEngine;

public class MarkedDecalDamage : DecalDamage, IDamageMarkedText
{
    [Header("Marked Decal Damage Area")]
    public GameObject MarkerObject;
    public TextMeshProUGUI NumberText;
    public bool IsReported => MarkerIsAvailable && MarkerObject.activeSelf;
    public bool MarkerIsAvailable => MarkerObject != null;
    public bool NumberTextIsAvailable => NumberText != null;

    [SerializeField] private string _issueOverMarkedInfoID;
    public new bool IsAvailable => MarkerIsAvailable && NumberTextIsAvailable && base.IsAvailable;

    public string IssueOverMarkedInfoID 
    {    
        get => _issueOverMarkedInfoID; 
        set => _issueOverMarkedInfoID = value; 
    }

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