using System;
using UnityEngine;

public class ZombieBehavior : MonoBehaviour, IDamageable
{
    public delegate void ZombieKilled();
    public static event ZombieKilled OnEnemyKilled;

    [Header("Zombie Stats")]
    [SerializeField] float zombieHealth = 3f;

    [Header("Animation Reference")]
    [SerializeField] Animator anim;

    bool isAlive = true;

    public void TakeDamage(float amount)
    {
        zombieHealth -= amount;

        if (zombieHealth <= 0)
        {
            TriggerDeath();
        }
    }

    private void TriggerDeath()
    {
        anim.SetTrigger("Dead");
    }

    private void DespawnAfterDeathAnim()
    {
        Destroy(gameObject, 2f);

        if(OnEnemyKilled != null)
        {
            OnEnemyKilled();
        }
    }
}
