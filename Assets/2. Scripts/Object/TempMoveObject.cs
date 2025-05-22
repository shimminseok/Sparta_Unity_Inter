using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectMoveHandler))]
public class TempMoveObject : MonoBehaviour, IHUDDisplayable, IPlatform, IActivatable
{
    [SerializeField] private string objectName;
    [SerializeField] private string objectDescription;
    [SerializeField] private bool isStartMove;

    private ObjectMoveHandler moveHandler;
    public string Name        => objectName;
    public string Description => objectDescription;

    public Transform PlatformTransform => transform;

    private void Awake()
    {
        moveHandler = GetComponent<ObjectMoveHandler>();
    }

    private void Start()
    {
        if (isStartMove)
            Move();
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