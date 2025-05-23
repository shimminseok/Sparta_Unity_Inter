using UnityEngine;

public interface IPlatform
{
    void OnUpdate();
    void Execute(PlayerController player);
    void Exit(PlayerController player);
}