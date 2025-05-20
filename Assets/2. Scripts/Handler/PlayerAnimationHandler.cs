using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour
{
    private readonly int isJumpingHash = Animator.StringToHash("Jump");
    private readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
    }

    public void SetJump()
    {
        animator.SetTrigger(isJumpingHash);
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        animator.SetFloat(moveSpeedHash, moveSpeed);
    }
}