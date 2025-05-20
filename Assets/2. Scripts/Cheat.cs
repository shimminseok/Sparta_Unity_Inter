using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            // PlayerController.Instance.StatusEffectManager.ApplyEffect(BuffFactory.CreateBuff());
        }
    }
}