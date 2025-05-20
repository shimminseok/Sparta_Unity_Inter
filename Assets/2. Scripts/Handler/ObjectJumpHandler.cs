using UnityEngine;

public class ObjectJumpHandler : MonoBehaviour
{
    [SerializeField] private float jumpForce;


    public void Jump(Rigidbody rigid)
    {
        rigid.velocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z); // y 초기화
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}