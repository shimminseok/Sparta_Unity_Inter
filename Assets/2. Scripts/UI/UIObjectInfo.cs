using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIObjectInfo : UIBase<UIObjectInfo>, IUIBase
{
    [SerializeField] private Image objectIcon;
    [SerializeField] private TextMeshProUGUI objectName;
    [SerializeField] private TextMeshProUGUI objectDescription;

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
        objectName.text = string.Empty;
        objectDescription.text = string.Empty;
    }


    public void SetObjectInfo(IInteractable interactable)
    {
        objectName.text = interactable.Name;
        objectDescription.text = interactable.Description;
        Open();
    }
}