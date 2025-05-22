using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverObject : MonoBehaviour, IHUDDisplayable, IInteractable
{
    [SerializeField] private string objectName;
    [SerializeField] private string objectDescription;
    [SerializeField] private string interactionName;

    [SerializeField] private GameObject linkGameObject;

    public string Name               => objectName;
    public string Description        => objectDescription;
    public string InteractDecription => interactionName;


    public void PrintUI()
    {
        UIHUD.Instance.SetInteractionText(InteractDecription);
    }

    public void Execute(PlayerController player)
    {
        if (linkGameObject == null)
        {
            Debug.Log("연결된 오브젝트가 없습니다.");
        }

        if (linkGameObject.TryGetComponent<IActivatable>(out IActivatable activatable))
        {
            activatable.Activate();
        }
    }

    public void Exit(PlayerController player)
    {
        UIHUD.Instance.ResetInteractionText();
    }
}