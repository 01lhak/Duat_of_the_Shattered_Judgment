using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5.0f;
    public float minX = -5f;
    public float maxX = 45f;
    public float characterScale = 1f;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        transform.localScale = new Vector3(characterScale, characterScale, 1f);
    }

    void Update()
    {
        // 인트로 중 입력 차단
        if (!FadeInEffect.isInputAllowed)
        {
            anim.SetBool("move", false);
            anim.SetBool("stop", true);
            return;
        }

        // ESC 또는 인벤토리 열려있으면 입력 차단
        if (GameManager.IsInputLocked)
        {
            anim.SetBool("move", false);
            anim.SetBool("stop", true);
            return;
        }

        // 입력값 확인
        float moveInput = 0f;

        if (Keyboard.current.aKey.isPressed)
            moveInput = -1f;

        if (Keyboard.current.dKey.isPressed)
            moveInput = 1f;

        // 위치 이동 및 제한
        Vector3 pos = transform.position;
        pos.x += moveInput * speed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;

        // 애니메이션 및 방향 전환
        if (moveInput != 0)
        {
            anim.SetBool("move", true);
            anim.SetBool("stop", false);

            transform.localScale = new Vector3(
                moveInput > 0 ? characterScale : -characterScale,
                characterScale,
                1f);
        }
        else
        {
            anim.SetBool("move", false);
            anim.SetBool("stop", true);
        }
    }
}