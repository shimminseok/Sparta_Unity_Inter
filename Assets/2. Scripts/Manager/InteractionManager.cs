using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractionManager : MonoBehaviour
{
    [FormerlySerializedAs("objectLayerMask")]
    [SerializeField] private LayerMask interactionLayerMask;

    [SerializeField] private float checkRate = 0.5f;
    private Camera mainCamera;


    private bool isInteracting;
    private IInteractable currentInteractable;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera is null) return;


        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, int.MaxValue, interactionLayerMask))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable) && !isInteracting && currentInteractable != interactable)
            {
                UIObjectInfo.Instance.SetObjectInfo(interactable);
                isInteracting = true;
            }
        }
        else if (isInteracting)
        {
            isInteracting = false;
            currentInteractable = null;
            UIObjectInfo.Instance.Close();
        }
    }
}