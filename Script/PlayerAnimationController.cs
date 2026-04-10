using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private Vector2 lastInput;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateMovement(Vector2 moveInput)
    {
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);

        if (moveInput != Vector2.zero)
        {
            animator.SetBool("isRuning", true);
            lastInput = moveInput;

            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
        else
        {
            animator.SetBool("isRuning", false);
        }
    }

    public Vector2 GetLastDirection()
    {
        return lastInput;
    }

    public void SetAttack(bool value)
    {
        animator.SetBool("isAttacking", value);
    }
}
