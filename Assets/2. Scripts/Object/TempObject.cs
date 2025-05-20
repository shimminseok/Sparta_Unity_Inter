using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObject : MonoBehaviour, IHUDDisplayable
{
    [SerializeField] private string name;
    [SerializeField] private string description;

    public string Name        => name;
    public string Description => description;
}