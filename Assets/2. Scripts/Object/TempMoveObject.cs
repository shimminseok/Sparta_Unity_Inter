using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectMoveHandler))]
public class TempMoveObject : MonoBehaviour, IHUDDisplayable, IPlatform, IActivatable
{
    [SerializeField] private string objectName;
    [SerializeField] private string objectDescription;
    [SerializeField] private bool isStartMove;
    [SerializeField] private Transform playerRoot;

    private ObjectMoveHandler moveHandler;
    public string Name        => objectName;
    public string Description => objectDescription;

    private void Awake()
    {
        moveHandler = GetComponent<ObjectMoveHandler>();
    }


    private void Start()
    {
        if (isStartMove)
            Move();
    }


    public void Execute(PlayerController player)
    {
        player.transform.SetParent(playerRoot);
    }

    public void OnUpdate()
    {
    }

    public void Exit(PlayerController player)
    {
        player.transform.SetParent(null);
        player.transform.localScale = Vector3.one;
    }

    private void Move()
    {
        moveHandler.StartMoving();
    }

    public void Activate()
    {
        Move();
    }
}