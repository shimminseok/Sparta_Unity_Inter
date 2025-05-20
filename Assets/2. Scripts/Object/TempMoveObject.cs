using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectMoveHandler))]
public class TempMoveObject : MonoBehaviour, IInteractable, IPlatform
{
    [SerializeField] private string objectName;
    [SerializeField] private string objectDescription;

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
        Move();
    }

    public void Move()
    {
        moveHandler.StartMoving();
    }
}