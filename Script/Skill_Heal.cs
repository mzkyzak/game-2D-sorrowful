using UnityEngine;

public class Skill_Heal : SkillBase
{
    [Header("Heal Settings")]
    public float healPercent = 0.3f; // 30%
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth == null)
            Debug.LogWarning("⚠️ PlayerHealth gak ditemukan di scene!");
    }

    public override void Activate()
    {
        if (playerHealth == null)
        {
            Debug.LogWarning("⚠️ PlayerHealth masih null, gak bisa heal!");
            return;
        }

        int healAmount = Mathf.RoundToInt(playerHealth.maxHealth * healPercent);
        playerHealth.Heal(healAmount);

        Debug.Log($"🩹 Heal aktif! Nambah {healAmount} HP");
    }
}
