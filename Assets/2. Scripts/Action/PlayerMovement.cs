using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float requireJumpStamina;

    private PlayerController owner;

    public bool CanJump => owner.InputHandler.JumpRequested && owner.IsGrounded && owner.StatManager.GetValue(StatType.CurrentStamina) >= requireJumpStamina;

    private void Awake()
    {
        owner = GetComponent<PlayerController>();
    }

    public void Movement()
    {
        Vector2 moveInput = owner.InputHandler.MoveInput;

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
        float   moveSpeed      = owner.StatManager.GetValue(StatType.MoveSpeed);
        Vector3 targetVelocity = new Vector3(move.x * moveSpeed, owner.Rigidbody.velocity.y, move.z * moveSpeed);
        Vector3 deltaPosition  = new Vector3(targetVelocity.x, 0f, targetVelocity.z) * Time.fixedDeltaTime;
        Debug.Log(deltaPosition);
        owner.Rigidbody.MovePosition(owner.transform.localPosition + deltaPosition);

        // 이동 방향 회전
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            owner.Rigidbody.MoveRotation(Quaternion.Slerp(transform.localRotation, targetRotation, owner.RotationSpeed * Time.fixedDeltaTime));
        }
    }

    public void ClimbWall()
    {
        if (owner.StatManager.GetValue(StatType.CurrentStamina) <= 0f)
        {
            owner.IsWallAhead(false);
            return;
        }

        Vector2 moveInput      = owner.InputHandler.MoveInput;
        Vector3 climbDirection = transform.up * moveInput.y + transform.right * moveInput.x;
        climbDirection.y = Mathf.Clamp(climbDirection.y, -1f, 1f); // 위아래만 가능하게 조절해도 됨
        climbDirection.Normalize();

        float   moveSpeed     = owner.StatManager.GetValue(StatType.MoveSpeed);
        Vector3 deltaPosition = climbDirection * moveSpeed * Time.fixedDeltaTime;

        owner.Rigidbody.MovePosition(owner.Rigidbody.position + deltaPosition);
        owner.StatManager.Consume(StatType.CurrentStamina, 1f);
    }

    public void Jump()
    {
        if (!CanJump)
        {
            owner.InputHandler.ResetJumpRequested();
            return;
        }

        owner.Rigidbody.velocity = new Vector3(owner.Rigidbody.velocity.x, 0f, owner.Rigidbody.velocity.z);
        owner.Rigidbody.AddForce(Vector3.up * owner.StatManager.GetValue(StatType.JumpPower), ForceMode.Impulse);
        owner.StatManager.Consume(StatType.CurrentStamina, requireJumpStamina);
    }
}