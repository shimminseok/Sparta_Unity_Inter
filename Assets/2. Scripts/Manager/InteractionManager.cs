using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayerMask;

    [SerializeField] private float checkRate = 0.5f;
    private Camera mainCamera;


    private float timer;
    private bool isInteracting;
    private IHUDDisplayable currentIhudDisplayable;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (mainCamera is null || EventSystem.current.IsPointerOverGameObject()) return;


        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, int.MaxValue, interactionLayerMask))
        {
            timer += Time.deltaTime;
            if (timer >= checkRate)
            {
                if (hit.collider.TryGetComponent<IHUDDisplayable>(out IHUDDisplayable interactable) && !isInteracting && currentIhudDisplayable != interactable)
                {
                    UIObjectInfo.Instance.SetObjectInfo(interactable);
                    isInteracting = true;
                }
            }
        }
        else if (isInteracting)
        {
            isInteracting = false;
            currentIhudDisplayable = null;
            timer = 0;
            UIObjectInfo.Instance.Close();
        }
    }
}