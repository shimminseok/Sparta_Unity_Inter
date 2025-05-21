using UnityEngine;

public enum PlayerState
{
    Idle,
    Move,
    Jump,
}

//FSM (유한상태머신) 
public class IdleState : IState<PlayerController>
{
    public void OnEnter(PlayerController owenr)
    {
        //대기상태
    }

    public void OnUpdate(PlayerController owner)
    {
    }

    public void OnFixedUpdate(PlayerController owner)
    {
    }

    public void OnExit(PlayerController owenr)
    {
    }

    public PlayerState? CheckTransition(PlayerController owner)
    {
        if (owner.InputHandler.MoveInput != Vector2.zero)
            return PlayerState.Move;
        else if (owner.PlayerMovement.CanJump)
            return PlayerState.Jump;

        return null;
    }
}


public class MoveState : IState<PlayerController>
{
    private readonly int MoveHash = Animator.StringToHash("IsMove");

    public void OnEnter(PlayerController owenr)
    {
        owenr.Animator.SetBool(MoveHash, true);
    }

    public void OnUpdate(PlayerController owner)
    {
    }

    public void OnFixedUpdate(PlayerController owner)
    {
        if (owner.IsTouchingWall)
        {
            owner.Rigidbody.useGravity = false;
            owner.PlayerMovement.ClimbWall();
        }
        else
        {
            owner.Rigidbody.useGravity = true;
            owner.PlayerMovement.Movement();
        }
    }

    public void OnExit(PlayerController owenr)
    {
        owenr.Animator.SetBool(MoveHash, false);
    }

    public PlayerState? CheckTransition(PlayerController owner)
    {
        if (owner.InputHandler.MoveInput == Vector2.zero)
            return PlayerState.Idle;
        else if (owner.PlayerMovement.CanJump)
            return PlayerState.Jump;

        return null;
    }
}

public class JumpState : IState<PlayerController>
{
    private readonly int JumpHash = Animator.StringToHash("Jump");

    public void OnEnter(PlayerController owner)
    {
        owner.PlayerMovement.Jump();
    }

    public void OnUpdate(PlayerController owner)
    {
    }

    public void OnFixedUpdate(PlayerController owner)
    {
    }

    public void OnExit(PlayerController owner)
    {
        owner.InputHandler.ResetJumpRequested();
    }

    public PlayerState? CheckTransition(PlayerController owner)
    {
        if (owner.InputHandler.MoveInput == Vector2.zero)
            return PlayerState.Idle;
        else
            return PlayerState.Move;


        return null;
    }
}