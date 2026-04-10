using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI Settings")]
    public Image healthBar;

    [Header("References")]
    public GameObject gameOverPanel;   // Panel Game Over
    public GameObject backSound;       // GameObject Backsound

    private PlayerMovement playerMovement;
    private bool isDead = false;

    public AudioClip DamagedSFX;
    public AudioClip DeadSFX;
    private AudioSource audioSource; // Komponen audio source


    void Start()
    {
        currentHealth = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();

        if (healthBar != null) UpdateHealthBar();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); // Pastikan panel game over awalnya mati
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (DamagedSFX != null && audioSource != null)
            audioSource.PlayOneShot(DamagedSFX);

        if (currentHealth < 0) currentHealth = 0;

        if (healthBar != null) UpdateHealthBar();

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthBar();

        Debug.Log($"Player heal {amount}, current health: {currentHealth}");
    }


    void UpdateHealthBar()
    {
        float fillValue = (float)currentHealth / maxHealth;
        healthBar.fillAmount = fillValue;
    }


    void Die()
    {
        isDead = true;

        if (DeadSFX != null && audioSource != null)
            audioSource.PlayOneShot(DeadSFX);

        if (playerMovement != null)
            playerMovement.Die();

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // 🔥 Munculin panel game over
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // 🔇 Matikan backsound
        if (backSound != null)
            backSound.SetActive(false);

        Debug.Log("Player mati!");
    }

    public void Respawn(Vector2 spawnPos)
    {
        transform.position = spawnPos;
        currentHealth = maxHealth;
        isDead = false;

        if (healthBar != null) UpdateHealthBar();

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        if (playerMovement != null)
        {
            playerMovement.isDead = false;
            playerMovement.animator.SetBool("isDead", false);
        }

        // 🔁 Sembunyikan panel dan nyalain lagi backsound
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (backSound != null)
            backSound.SetActive(true);
    }
}
