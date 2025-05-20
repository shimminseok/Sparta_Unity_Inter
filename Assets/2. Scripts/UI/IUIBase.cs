using UnityEngine;

public interface IUIBase
{
    public void Open();
    public void Close();
}

public class UIBase<T> : SceneOnlySingleton<T> where T : UIBase<T>, IUIBase
{
    [Header("UIBase")]
    [SerializeField] private GameObject content;

    public virtual void Open()
    {
        UIManager.Instance.CheckOpenUI();
        content.SetActive(true);
    }

    public virtual void Close()
    {
        UIManager.Instance.Close();
        content.SetActive(false);
    }
}