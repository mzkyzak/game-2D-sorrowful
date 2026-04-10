using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float attackCooldown = 1f;
    public float attackActiveTime = 0.4f; // berapa lama hitbox aktif
    public GameObject hitbox; // drag dari Inspector
    private float lastAttackTime;

    private EnemyAnimator enemyAnimator;

    void Start()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        if (hitbox != null)
            hitbox.SetActive(false);
    }

    public void TryAttack(GameObject target, Vector2 lastDir)
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return; // masih cooldown

        lastAttackTime = Time.time;

        enemyAnimator.UpdateDirection(lastDir);
        enemyAnimator.SetAttacking(true);

        // 🔥 Aktifin hitbox selama animasi attack
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        if (hitbox != null)
            hitbox.SetActive(true); // aktifin collider

        yield return new WaitForSeconds(attackActiveTime);

        if (hitbox != null)
            hitbox.SetActive(false); // matiin collider

        // tunggu sampai cooldown kelar baru bisa attack lagi
        yield return new WaitForSeconds(attackCooldown - attackActiveTime);

        enemyAnimator.SetAttacking(false);
    }
}
