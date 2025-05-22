using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableWallObject : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionName;

    public string InteractDecription => interactionName;

    public void PrintUI()
    {
        UIHUD.Instance.SetInteractionText(interactionName);
    }

    public void Execute(PlayerController player)
    {
        player.IsWallAhead(!player.IsTouchingWall);
    }

    public void Exit(PlayerController player)
    {
        player.IsWallAhead(false);
        UIHUD.Instance.ResetInteractionText();
    }
}