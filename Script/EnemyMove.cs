using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(EnemyAnimator), typeof(EnemyAttack))]
public class EnemyMove : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 2f;
    public float chaseRange = 5f;
    public float attackRange = 1.2f;

    private Rigidbody2D rb;
    private EnemyAnimator enemyAnimator;
    private EnemyAttack enemyAttack;
    private EnemyHealth enemyHealth;
    private Animator animator;
    private Vector2 lastDir;

    private bool isStunned = false; // 🔹 untuk knockback stun

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (isStunned) return; // skip movement saat knockback

        if (enemyAnimator != null && animator.GetBool("isAttacking"))
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (enemyHealth != null && GetCurrentHealth() <= 0)
        {
            rb.velocity = Vector2.zero;
            enemyAnimator.SetMoving(false);
            return;
        }

        GameObject target = FindClosestPlayer();
        if (target == null)
        {
            enemyAnimator.SetMoving(false);
            return;
        }

        float dist = Vector2.Distance(target.transform.position, transform.position);

        if (dist > chaseRange)
        {
            enemyAnimator.SetMoving(false);
            return;
        }

        if (dist <= attackRange)
        {
            enemyAnimator.SetMoving(false);
            enemyAttack.TryAttack(target, lastDir);
            return;
        }

        Vector2 dir = (target.transform.position - transform.position).normalized;

        if (dir != Vector2.zero)
        {
            lastDir = dir;
            enemyAnimator.UpdateDirection(dir);
        }

        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
        enemyAnimator.SetMoving(true);
    }

    private int GetCurrentHealth()
    {
        var field = typeof(EnemyHealth).GetField(
            "currentHealth",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );

        return (int)field.GetValue(enemyHealth);
    }

    private GameObject FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
            return null;

        GameObject closest = players[0];
        float minDist = Vector2.Distance(transform.position, closest.transform.position);

        foreach (GameObject p in players)
        {
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < minDist)
            {
                closest = p;
                minDist = dist;
            }
        }

        return closest;
    }

    // ================= Knockback =================
    public void ApplyKnockback(Vector2 direction, float force, float duration = 0.2f)
    {
        StartCoroutine(KnockbackCoroutine(direction, force, duration));
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction, float force, float duration)
    {
        float timer = 0f;
        rb.velocity = Vector2.zero;
        isStunned = true;

        // 🔹 Trigger animasi knockback kalo ada
        if (animator != null)
            animator.SetTrigger("Knockback");

        while (timer < duration)
        {
            rb.MovePosition(rb.position + direction.normalized * force * Time.fixedDeltaTime);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        isStunned = false;
    }
}
