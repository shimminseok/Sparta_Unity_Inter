using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private List<IUIBase> openedUI = new List<IUIBase>();


    public void CheckOpenPopup(IUIBase panel)
    {
        if (openedUI.Contains(panel))
        {
            panel.Close();
        }
        else
        {
            panel.Open();
        }
    }

    public void OpenPanel(IUIBase panel)
    {
        openedUI.Add(panel);
    }

    public void ClosePanel(IUIBase panel)
    {
        openedUI.Remove(panel);
    }

    public void AllClosePanel()
    {
        for (int i = openedUI.Count - 1; i >= 0; i--)
        {
            openedUI[i].Close();
        }
    }
}