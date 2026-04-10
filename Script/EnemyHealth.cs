using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    [Header("soound")]
    public AudioClip DamagedSFX;
    public AudioClip DieSFX;
    private AudioSource audioSource;

    [HideInInspector]
    public ZombieSpawner spawner;

    [Header("Health Bar")]
    public GameObject healthBarPrefab;
    public float healthBarYOffset = 2f;
    private GameObject healthBarInstance;
    public Slider healthBarSlider;

    void Start()
    {
{
    currentHealth = maxHealth;
    audioSource = GetComponent<AudioSource>();

    // Coba ambil slider dari child kalau belum di-assign lewat inspector
    if (healthBarSlider == null)
    {
        healthBarSlider = GetComponentInChildren<Slider>();
    }

    if (healthBarSlider != null)
    {
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;
    }
}
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (DamagedSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(DamagedSFX);
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBarSlider != null)
            healthBarSlider.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("EnemyHitbox")) // pastiin hitbox lu dikasih tag ini
                child.gameObject.SetActive(false);
        }

        if (spawner != null)
            spawner.ZombieDead();

        if (DieSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(DieSFX);
        }

        if (healthBarInstance != null)
            Destroy(healthBarInstance);

        Destroy(gameObject, 1f);
    }
}
