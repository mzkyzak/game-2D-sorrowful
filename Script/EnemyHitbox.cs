using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int damage = 10;
    public string targetTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null)
                hp.TakeDamage(damage);
        }
    }
}
