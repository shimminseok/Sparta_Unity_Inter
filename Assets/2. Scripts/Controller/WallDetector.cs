using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    public bool IsTouchingWall { get; private set; }


    private void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("Wall"))
        // {
        // }
    }

    private void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag("Wall"))
        // {
        // }
    }
}