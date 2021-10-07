using System;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehavior : MonoBehaviour, IDamageable
{
    public delegate void ZombieKilled();        // Ini delegate template
    public static event ZombieKilled OnEnemyKilled;

    ZombieMovement agent; // Connect ke kelas movement agar set update posisi
                                   // langsung berhenti ketika trigger death

    [Header("Zombie Stats")]
    [SerializeField] float zombieHealth = 3f;

    [Header("Animation Reference")]
    [SerializeField] Animator anim;

    [Header("Zombie Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip zombieGrowl;

    bool isAlive = true;

    void Start()
    {
        anim.SetBool("IsAlive", isAlive);
        agent = GetComponent<ZombieMovement>();
        StartGrowlingSound();
    }

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

        agent.enemyDeathStop();
        Destroy(gameObject, 2f);
       
    }

    public void StartGrowlingSound()
    {
        audioSource.PlayOneShot(zombieGrowl);
    }
}
