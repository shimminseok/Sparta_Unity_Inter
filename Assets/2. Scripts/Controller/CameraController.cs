using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Serialization;

public class CameraController : Singleton<CameraController>
{
    // [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float distance;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float maxDistance;
    [SerializeField] private float minDistance;
    [SerializeField] private Transform playerBody;

    [SerializeField] private CinemachineVirtualCamera mainCam;

    CinemachineFramingTransposer transposer;
    CinemachineCameraOffset cameraOffset;

    float xRotation = 0f;
    float yRotation = 0f;


    public CinemachineVirtualCamera MainCamera => mainCam;


    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        xRotation = transform.eulerAngles.x;
        yRotation = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        // 마우스 오른쪽 버튼이 눌렸을 때만 회전 처리
        HandleRotation();
        HandleZoom();
    }

    // Camera script

    void HandleRotation()
    {
        if (!Input.GetMouseButton(1))
            return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mouseX) > Mathf.Epsilon || Mathf.Abs(mouseY) > Mathf.Epsilon)
        {
            mouseX *= mouseSensitivity * Time.deltaTime;
            mouseY *= mouseSensitivity * Time.deltaTime;

            yRotation += mouseX;
            xRotation += mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            mainCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }

    void HandleZoom()
    {
        float zoomAmount = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(zoomAmount) > Mathf.Epsilon)
        {
            distance -= zoomAmount * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }
    }
}