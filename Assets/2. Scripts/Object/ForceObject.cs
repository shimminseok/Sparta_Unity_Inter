using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ForceObject : MonoBehaviour, IHUDDisplayable
{
    [SerializeField] private string objectName;
    [SerializeField] private string objectDescription;
    [SerializeField] private float swingSpeed;

    [SerializeField] private bool forwardStart;


    private Quaternion startRotation;
    private Rigidbody rigid;

    public string Name        => objectName;
    public string Description => objectDescription;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }


    private void Start()
    {
        startRotation = transform.localRotation;
    }

    private void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * (45f * (forwardStart ? 1f : -1f));
        transform.localRotation = startRotation * Quaternion.Euler(0f, 0f, angle);
    }

    public void StartPendulum()
    {
        rigid.AddForce(transform.forward * swingSpeed, ForceMode.Impulse);
    }
}