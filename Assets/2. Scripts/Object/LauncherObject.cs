using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherObject : MonoBehaviour, IHUDDisplayable, IPlatform
{
    [SerializeField] private string objectName;
    [SerializeField] private string objectDescription;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float fireForce;
    [SerializeField] private float upForce;


    public string Name        => objectName;
    public string Description => objectDescription;
    private Coroutine fireCoroutine;


    public void OnUpdate()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.World);
    }

    public void Execute(PlayerController player)
    {
        if (fireCoroutine == null)
            fireCoroutine = StartCoroutine(InputFire(player.Rigidbody));
    }

    public void Exit(PlayerController player)
    {
        StopCoroutine(fireCoroutine);
        fireCoroutine = null;
        transform.Rotate(0f, 0f, 0f, Space.World);
    }


    public IEnumerator InputFire(Rigidbody playerRb)
    {
        yield return new WaitUntil(() => PlayerController.Instance.InputHandler.InteractRequested);
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z); // y 초기화
        playerRb.AddForce((transform.forward * fireForce) + (Vector3.up * upForce), ForceMode.Impulse);

        PlayerController.Instance.InputHandler.ResetInteractRequested();
    }
}