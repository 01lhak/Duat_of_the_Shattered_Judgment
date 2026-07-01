using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    public float moveSpeed = 5f;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 인트로 중 입력 차단
        if (!FadeInEffect.isInputAllowed)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            anim.SetBool("isMoving", false);
            return;
        }

        // ESC 또는 인벤토리 열려있으면 입력 차단
        if (GameManager.IsInputLocked)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            anim.SetBool("isMoving", false);
            return;
        }

        // A / D 입력
        float moveInput = Input.GetAxisRaw("Horizontal");

        // 이동
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // 애니메이션
        anim.SetBool("isMoving", moveInput != 0);

        // 방향 전환
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}