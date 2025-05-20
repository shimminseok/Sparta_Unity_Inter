using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController owner;

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
        float   moveSpeed      = owner.StatManager.GetFinalValue(StatType.MoveSpeed);
        Vector3 targetVelocity = new Vector3(move.x * moveSpeed, owner.Rigidbody.velocity.y, move.z * moveSpeed);
        Vector3 deltaPosition  = new Vector3(targetVelocity.x, 0f, targetVelocity.z) * Time.fixedDeltaTime;
        owner.Rigidbody.MovePosition(owner.Rigidbody.position + deltaPosition);

        // 이동 방향 회전
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            owner.Rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, owner.RotationSpeed * Time.fixedDeltaTime));
        }
    }

    public void Jump()
    {
        if (!owner.InputHandler.JumpRequested || !owner.IsGrounded)
            return;


        owner.Rigidbody.velocity = new Vector3(owner.Rigidbody.velocity.x, 0f, owner.Rigidbody.velocity.z);
        owner.Rigidbody.AddForce(Vector3.up * owner.StatManager.GetFinalValue(StatType.JumpPower), ForceMode.Impulse);
        owner.InputHandler.ResetJumpRequested();
    }
}