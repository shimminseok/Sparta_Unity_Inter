using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Stack<IUIBase> openedUI = new Stack<IUIBase>();

    public void CheckOpenUI()
    {
        if (openedUI.TryPeek(out IUIBase openUI))
        {
            Close();
        }
        else
        {
            Open(openUI);
        }
    }

    public void Open(IUIBase openIui)
    {
        openedUI.Push(openIui);
    }

    public void Close()
    {
        openedUI.Pop();
    }
}