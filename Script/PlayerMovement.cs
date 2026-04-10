using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    [HideInInspector] public Animator animator;

    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isDead = false;

    [HideInInspector] public float lastMoveX = 0f;
    [HideInInspector] public float lastMoveY = -1f; // default ngadep bawah

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isAttacking)
        {
            float currentSpeed = speed * 0.7f;
            rb.velocity = moveInput * currentSpeed;
        }
        else
        {
            rb.velocity = moveInput * speed;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (isDead) return; // ❌ kalau mati, input diabaikan

        moveInput = context.ReadValue<Vector2>();

        if (context.performed)
        {
            animator.SetBool("isRuning", true);

            if (moveInput != Vector2.zero)
            {
                lastMoveX = moveInput.x;
                lastMoveY = moveInput.y;

                animator.SetFloat("LastInputX", moveInput.x);
                animator.SetFloat("LastInputY", moveInput.y);
            }
        }

        if (context.canceled)
        {
            animator.SetBool("isRuning", false);
        }

        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }

    public void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;

        // 🧭 set arah terakhir supaya blend tree "mati" hadap arah yang benar
        animator.SetFloat("LastInputX", lastMoveX);
        animator.SetFloat("LastInputY", lastMoveY);

        animator.SetBool("isRuning", false);
        animator.SetBool("isDead", true);

        Debug.Log("Player mati, animasi arah terakhir disimpan.");
    }
}
