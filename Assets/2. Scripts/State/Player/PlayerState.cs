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
        else if (owner.InputHandler.JumpRequested)
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
        owner.PlayerMovement.Movement();
    }

    public void OnExit(PlayerController owenr)
    {
        owenr.Animator.SetBool(MoveHash, false);
    }

    public PlayerState? CheckTransition(PlayerController owner)
    {
        if (owner.InputHandler.MoveInput == Vector2.zero)
            return PlayerState.Idle;
        else if (owner.InputHandler.JumpRequested)
            return PlayerState.Jump;

        return null;
    }
}

public class JumpState : IState<PlayerController>
{
    private readonly int JumpHash = Animator.StringToHash("Jump");

    public void OnEnter(PlayerController owner)
    {
        // owner.Animator.SetTrigger(JumpHash);
    }

    public void OnUpdate(PlayerController owner)
    {
    }

    public void OnFixedUpdate(PlayerController owner)
    {
        owner.PlayerMovement.Jump();
    }

    public void OnExit(PlayerController owner)
    {
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