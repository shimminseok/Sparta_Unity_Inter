using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectJumpHandler))]
public class TempJumpObject : MonoBehaviour, IHUDDisplayable, IObjectExecutable
{
    [SerializeField] private string objectName;
    [SerializeField] private string objectDescription;
    public string Name        => objectName;
    public string Description => objectDescription;

    ObjectJumpHandler objectJumpHandler;

    private void Awake()
    {
        objectJumpHandler = GetComponent<ObjectJumpHandler>();
    }

    public void Execute(Rigidbody playerRb)
    {
        objectJumpHandler.Jump(playerRb);
    }
}