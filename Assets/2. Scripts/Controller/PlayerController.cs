using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(PlayerAnimationHandler))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private LayerMask groundLayer;
    private InputHandler inputHandler;
    private PlayerAnimationHandler playerAnimationHandler;
    private Rigidbody rigid;
    private bool isGrounded;

    private IObjectExecutable lastExecutable;
    private IPlatform currentPlatform;
    private Vector3 lastPlayformPos;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        rigid = GetComponent<Rigidbody>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
    }


    private void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.2f, groundLayer))
        {
            isGrounded = true;

            if (hit.collider.TryGetComponent<IObjectExecutable>(out var executable))
            {
                if (lastExecutable == executable) return;

                executable.Execute(rigid);
                lastExecutable = executable;
            }

            else if (hit.collider.TryGetComponent<IPlatform>(out var platform))
            {
                if (currentPlatform == platform) return;

                currentPlatform = platform;
                lastPlayformPos = currentPlatform.PlatformTransform.position;
            }
            else
            {
                currentPlatform = null;
                lastExecutable = null;
                lastPlayformPos = Vector3.zero;
            }
        }
        else
        {
            isGrounded = false;
            lastExecutable = null;
            if (transform.parent != null)
                transform.SetParent(null);
        }
    }

    private void FixedUpdate()
    {
        Jump();
        Movement();
        SyncWithPlatform();
    }

    private void Movement()
    {
        Vector2 moveInput = inputHandler.MoveInput;

        // Cinemachine VirtualCamera의 Transform 가져오기
        Transform camTransform = CameraController.Instance.MainCamera.transform;

        // 카메라 기준 방향 벡터
        Vector3 camForward = camTransform.forward;
        Vector3 camRight   = camTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // 입력 기반 이동 벡터
        Vector3 move = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        // Rigidbody로 이동
        Vector3 velocity = new Vector3(move.x * moveSpeed, rigid.velocity.y, move.z * moveSpeed);
        rigid.velocity = velocity;

        // 이동 방향 회전
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // 애니메이션 이동 속도
        Vector2 flatVelocity = new Vector2(velocity.x, velocity.z);
        playerAnimationHandler.SetMoveSpeed(flatVelocity.magnitude);
    }

    private void Jump()
    {
        if (!inputHandler.JumpRequested || !isGrounded)
            return;

        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        inputHandler.ResetJumpRequested();
    }

    private void SyncWithPlatform()
    {
        if (currentPlatform == null) return;

        Vector3 delta = currentPlatform.PlatformTransform.position - lastPlayformPos;
        rigid.MovePosition(rigid.position + delta);
        lastPlayformPos = currentPlatform.PlatformTransform.position;
    }

    private void ClearPlatform()
    {
        currentPlatform = null;
        lastPlayformPos = Vector3.zero;
    }
}