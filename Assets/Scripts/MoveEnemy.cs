using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    public float moveSpeed = 300f; // 이동 속도(초당)

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // 오른쪽 -> 왼쪽 방향: Vector2.left = (-1, 0)
        rb.linearVelocity = Vector2.left * moveSpeed;
    }
}
