using UnityEngine;

public class HitboxDamage : MonoBehaviour
{
    public string targetTag = "Enemy";
    public SkillManager skillManager;

    private PlayerAttack playerAttack;

    void Start()
    {
        // Asumsi hitbox child dari player
        playerAttack = GetComponentInParent<PlayerAttack>();
        if (playerAttack == null)
            Debug.LogWarning("⚠️ PlayerAttack gak ditemukan di parent Hitbox!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag)) return;

        int finalDamage = (playerAttack != null) ? playerAttack.damage : 20;

        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(finalDamage);

            // Tambah energi kalau mau
            if (skillManager != null)
                skillManager.AddEnergy(10);
        }

        Debug.Log($"{gameObject.name} kena {other.name}, damage: {finalDamage}");
    }
}
