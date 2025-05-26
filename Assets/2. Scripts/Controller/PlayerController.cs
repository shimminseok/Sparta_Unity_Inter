using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(PlayerAnimationHandler))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(StatManager))]
[RequireComponent(typeof(StatusEffectManager))]
public class PlayerController : SceneOnlySingleton<PlayerController>, IDamageable, IKnockbackable
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private LayerMask groundLayer;

    public InputHandler        InputHandler        { get; private set; }
    public PlayerMovement      PlayerMovement      { get; private set; }
    public StatManager         StatManager         { get; private set; }
    public Animator            Animator            { get; private set; }
    public StatusEffectManager StatusEffectManager { get; private set; }
    public Rigidbody           Rigidbody           { get; private set; }

    public bool IsGrounded { get; private set; }

    private IObjectExecutable lastExecutable;
    private IPlatform currentPlatform;
    private StateMachine<PlayerController> stateMachine;
    private IState<PlayerController>[] states;

    private Vector3 lastPlayformPos;
    private PlayerState currentState;
    private IInteractable currentInteractable;


    public bool  IsTouchingWall { get; private set; }
    public float RotationSpeed  => rotationSpeed;

    protected override void Awake()
    {
        base.Awake();
        InputHandler = GetComponent<InputHandler>();
        Rigidbody = GetComponent<Rigidbody>();
        PlayerMovement = GetComponent<PlayerMovement>();
        Animator = GetComponent<Animator>();
        StatManager = GetComponent<StatManager>();
        StatusEffectManager = GetComponent<StatusEffectManager>();
    }

    private void Start()
    {
        SetupState();
    }

    private void SetupState()
    {
        states = new IState<PlayerController>[Enum.GetValues(typeof(PlayerState)).Length];
        for (int i = 0; i < states.Length; i++)
        {
            states[i] = GetState((PlayerState)i);
        }

        stateMachine = new StateMachine<PlayerController>();
        stateMachine.Setup(this, states[(int)PlayerState.Idle]);
    }

    private IState<PlayerController> GetState(PlayerState state)
    {
        return state switch
        {
            PlayerState.Idle => new IdleState(),
            PlayerState.Move => new MoveState(),
            PlayerState.Jump => new JumpState(),
            _                => null
        };
    }

    private void ChangeState(PlayerState newState)
    {
        stateMachine.ChangeState(states[(int)newState]);
        currentState = newState;
    }

    private void TryStateTransition()
    {
        PlayerState? next = states[(int)currentState].CheckTransition(this);
        if (next.HasValue && next.Value != currentState)
        {
            ChangeState(next.Value);
        }
    }

    private void Update()
    {
        TryStateTransition();

        CheckGround();
        CheckFowardInteraction();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine?.FixedUpdate();
        // SyncWithPlatform();
    }

    public void IsWallAhead(bool isTouchingWall)
    {
        IsTouchingWall = isTouchingWall;
    }

    private void CheckFowardInteraction()
    {
        if (Physics.Raycast(transform.position, transform.forward * 0.3f, out RaycastHit forwardHit, 0.2f))
        {
            if (forwardHit.collider.TryGetComponent<IInteractable>(out IInteractable newInteractable))
            {
                if (currentInteractable != newInteractable)
                {
                    currentInteractable?.Exit(this);
                    currentInteractable = newInteractable;
                    currentInteractable.PrintUI();
                }

                if (!InputHandler.InteractRequested)
                    return;

                newInteractable.Execute(this);
                InputHandler.ResetInteractRequested();
            }
            else
            {
                ClearCurrentInteractable();
            }
        }
        else
        {
            ClearCurrentInteractable();
        }
    }

    private void CheckGround()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, out RaycastHit hit, 0.2f, groundLayer))
        {
            IsGrounded = true;
            //점프 발판 등 단발성 플랫폼
            if (hit.collider.TryGetComponent<IObjectExecutable>(out IObjectExecutable executable))
            {
                if (lastExecutable == executable)
                    return;

                executable.Execute(Rigidbody);
                lastExecutable = executable;
            }
            //움직이는 발판 등 지속성 플랫폼
            else if (hit.collider.TryGetComponent<IPlatform>(out IPlatform platform))
            {
                platform.OnUpdate();

                if (currentPlatform == platform)
                    return;

                currentPlatform?.Exit(this);
                platform.Execute(this);
                currentPlatform = platform;
            }
            else
            {
                currentPlatform?.Exit(this);
                currentPlatform = null;
                lastExecutable = null;
                lastPlayformPos = Vector3.zero;
            }
        }
        else
        {
            currentPlatform?.Exit(this);
            IsGrounded = false;
            lastExecutable = null;
            currentPlatform = null;
        }
    }

    private void ClearCurrentInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Exit(this);
            currentInteractable = null;
        }
    }

    public void TakeDamage(float damage)
    {
        StatManager.Consume(StatType.CurrentHp, damage);
        if (StatManager.GetValue(StatType.CurrentHp) <= 0)
        {
            //죽음
        }
    }

    public void ApplyKnockback(Vector3 force)
    {
        Rigidbody.AddForce(force, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), Vector3.down * 0.2f, Color.red);

        Debug.DrawRay(transform.position, transform.forward * 0.3f, Color.green);
    }
}