using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectForceHandler : MonoBehaviour
{
    [SerializeField] private float knockbackForce;
    [SerializeField] private float upwardForce;


    private Vector3 movedir;
    private Vector3 prevPos;

    private void Start()
    {
        prevPos = transform.position;
    }

    private void Update()
    {
        movedir = (transform.position - prevPos).normalized;
        prevPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IKnockbackable>(out IKnockbackable knockbackable))
        {
            Vector3 knockbackDir = movedir * knockbackForce + Vector3.up * upwardForce;
            knockbackable.ApplyKnockback(knockbackDir);
        }
    }
}