using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;

    [Header("SFX")]
    public AudioClip shootSFX;
    public AudioClip skillShootSFX;
    private AudioSource audioSource;

    [Header("Hitbox")]
    public GameObject hitboxUp;
    public GameObject hitboxDown;
    public GameObject hitboxLeft;
    public GameObject hitboxRight;

    [Header("Attack Settings")]
    public float attackDuration = 0.4f;
    public float hitboxDelay = 0.07f;
    public float hitboxActiveTime = 0.12f;
    public float attackCooldown = 0.5f;
    public int damage = 20;

    private float attackTimer;
    private bool canAttack = true;

    [HideInInspector] public bool isSkillMode = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        DisableAllHitboxes();
    }

    void Update()
    {
        if (playerMovement.isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                FinishAttack();
            }
        }
    }

    // INPUT SYSTEM (OPTIONAL)
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
            DoAttack();
    }

    // UI BUTTON ANDROID
    public void AttackButton()
    {
        DoAttack();
    }

    private void DoAttack()
    {
        if (!canAttack || playerMovement.isAttacking || playerMovement.isDead)
            return;

        playerMovement.isAttacking = true;
        attackTimer = attackDuration;

        if (isSkillMode)
            animator.SetBool("isSkill", true);
        else
            animator.SetBool("isAttacking", true);

        float dirX = playerMovement.lastMoveX;
        float dirY = playerMovement.lastMoveY;
        if (dirX == 0 && dirY == 0)
            dirY = -1f;

        animator.SetFloat("AttackDirX", dirX);
        animator.SetFloat("AttackDirY", dirY);

        DisableAllHitboxes();
        StartCoroutine(ActivateHitboxWithDelay(dirX, dirY));

        canAttack = false;
    }

    private void FinishAttack()
    {
        playerMovement.isAttacking = false;
        animator.SetBool("isAttacking", false);
        animator.SetBool("isSkill", false);
        DisableAllHitboxes();
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator ActivateHitboxWithDelay(float dirX, float dirY)
    {
        yield return new WaitForSeconds(hitboxDelay);

        GameObject activeHitbox;

        if (Mathf.Abs(dirY) > Mathf.Abs(dirX))
            activeHitbox = dirY > 0 ? hitboxUp : hitboxDown;
        else
            activeHitbox = dirX > 0 ? hitboxRight : hitboxLeft;

        activeHitbox.SetActive(true);

        if (audioSource)
        {
            if (isSkillMode && skillShootSFX != null)
                audioSource.PlayOneShot(skillShootSFX);
            else if (shootSFX != null)
                audioSource.PlayOneShot(shootSFX);
        }

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            activeHitbox.transform.position,
            activeHitbox.transform.localScale,
            0f
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyMove enemy = hit.GetComponent<EnemyMove>();
                if (enemy != null)
                {
                    float knockbackForce = isSkillMode ? 8f : 4f;
                    Vector2 knockDir = (enemy.transform.position - transform.position).normalized;
                    enemy.ApplyKnockback(knockDir, knockbackForce);
                }
            }
        }

        yield return new WaitForSeconds(hitboxActiveTime);
        activeHitbox.SetActive(false);
    }

    private void DisableAllHitboxes()
    {
        hitboxUp.SetActive(false);
        hitboxDown.SetActive(false);
        hitboxLeft.SetActive(false);
        hitboxRight.SetActive(false);
    }
}
