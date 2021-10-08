using UnityEngine;

public class ZombieBehavior : MonoBehaviour, IDamageable
{
    public delegate void ZombieKilled();        // Ini delegate template
    public static event ZombieKilled OnEnemyKilled;
    public static event ZombieKilled OnZombieHpZero;

    [Header("Zombie Stats")]
    [SerializeField] float zombieHealth = 3f;

    [Header("Animation Reference")]
    [SerializeField] Animator anim;

    bool isAlive = true;

    void Start()
    {
        anim.SetBool("IsAlive", isAlive);
    }

    public void TakeDamage(float amount)
    {
        zombieHealth -= amount;

        if (zombieHealth <= 0)
        {
            if (OnZombieHpZero != null)
                OnZombieHpZero();

            TriggerDeath();
        }
    }

    private void TriggerDeath()
    {
        isAlive = false;
        anim.SetBool("IsAlive", isAlive);
        anim.SetTrigger("Dead");
    }

    private void DespawnAfterDeathAnim()
    {

        if(OnEnemyKilled != null)
        {
            OnEnemyKilled();
        }

        Destroy(gameObject, 2f);
    }
}
