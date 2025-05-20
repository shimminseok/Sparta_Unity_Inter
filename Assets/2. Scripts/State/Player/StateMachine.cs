using UnityEngine;

public class StateMachine<T> where T : class
{
    private T ownerEntity;
    private IState<T> currentState;

    public void Setup(T owner, IState<T> entryState)
    {
        ownerEntity = owner;
        ChangeState(entryState);
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate(ownerEntity);
        }
    }

    public void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnFixedUpdate(ownerEntity);
        }
    }

    public void ChangeState(IState<T> newState)
    {
        if (newState == null)
            return;


        if (currentState != null)
        {
            currentState.OnExit(ownerEntity);
        }

        currentState = newState;
        currentState.OnEnter(ownerEntity);
    }
}