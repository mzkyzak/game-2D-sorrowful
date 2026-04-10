using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    public GameObject hitbox;


    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void ActivateHitbox() => hitbox.SetActive(true);
    public void DeactivateHitbox() => hitbox.SetActive(false);

    public void UpdateDirection(Vector2 dir)
    {
        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);
    }

    public void SetMoving(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);
    }

    public void SetAttacking(bool isAttacking)
    {
        animator.SetBool("isAttacking", isAttacking);
    }

    // Dipanggil dari event animasi di akhir attack
    public void EndAttack()
    {
        SetAttacking(false);
    }
}
