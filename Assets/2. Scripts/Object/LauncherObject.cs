using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherObject : MonoBehaviour, IHUDDisplayable, IPlatform
{
    public string    Name              { get; }
    public string    Description       { get; }
    public Transform PlatformTransform { get; }

    public void Execute(GameObject player)
    {
    }

    public void Exit(GameObject player)
    {
    }
}