using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 3f;
    [SerializeField] private GameObject damagedPrefab;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(damagedPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
