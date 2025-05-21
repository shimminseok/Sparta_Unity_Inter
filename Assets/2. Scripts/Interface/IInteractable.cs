public interface IInteractable
{
    public string InteractDecription { get; }

    public void PrintUI();
    public void Execute(PlayerController player);
}