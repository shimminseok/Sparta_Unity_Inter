public interface IState<T>
{
    void OnEnter(T entity);
    void OnUpdate(T entity);
    void OnFixedUpdate(T entity);
    void OnExit(T entity);

    PlayerState? CheckTransition(T owner);
}