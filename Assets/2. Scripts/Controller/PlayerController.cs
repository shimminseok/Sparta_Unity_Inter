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
public class PlayerController : SceneOnlySingleton<PlayerController>
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private LayerMask groundLayer;

    public InputHandler        InputHandler        { get; private set; }
    public PlayerMovement      PlayerMovement      { get; private set; }
    public StatManager         StatManager         { get; private set; }
    public Animator            Animator            { get; private set; }
    public StatusEffectManager StatusEffectManager { get; private set; }

    public bool IsGrounded { get; private set; }

    private IObjectExecutable lastExecutable;
    private IPlatform currentPlatform;
    private Vector3 lastPlayformPos;

    private StateMachine<PlayerController> stateMachine;
    private IState<PlayerController>[] states;
    private PlayerState currentState;


    //임시
    public Rigidbody Rigidbody     { get; private set; }
    public float     RotationSpeed => rotationSpeed;

    private void Awake()
    {
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
        var next = states[(int)currentState].CheckTransition(this);
        if (next.HasValue && next.Value != currentState)
        {
            ChangeState(next.Value);
        }
    }

    private void Update()
    {
        TryStateTransition();
        stateMachine.Update();
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, out RaycastHit hit, 0.2f, groundLayer))
        {
            IsGrounded = true;

            if (hit.collider.TryGetComponent<IObjectExecutable>(out var executable))
            {
                if (lastExecutable == executable) return;

                executable.Execute(Rigidbody);
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
            IsGrounded = false;
            lastExecutable = null;
            if (transform.parent != null)
                transform.SetParent(null);
        }
    }

    private void FixedUpdate()
    {
        stateMachine?.FixedUpdate();
        SyncWithPlatform();
    }

    private void SyncWithPlatform()
    {
        if (currentPlatform == null) return;

        Vector3 delta = currentPlatform.PlatformTransform.position - lastPlayformPos;
        Rigidbody.MovePosition(Rigidbody.position + delta);
        lastPlayformPos = currentPlatform.PlatformTransform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), Vector3.down * 0.2f, Color.red);
    }
}