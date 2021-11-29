using UnityEngine;
using UnityEngine.AI;

public class ZombieBehavior : MonoBehaviour, IDamageable
{
    public delegate void ZombieKilled();        // Ini delegate template
    public static event ZombieKilled OnEnemyKilled;
    public static event ZombieKilled OnZombieHpZero;

    ZombieMovement agent; // Connect ke kelas movement agar set update posisi
                                   // langsung berhenti ketika trigger death

    [Header("Zombie Stats")]
    [SerializeField] float zombieHealth = 3f;

    [Header("Animation Reference")]
    [SerializeField] Animator anim;

    [Header("Zombie Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip zombieGrowl;

    [Header("Zombie Body Part Reference")]
    [SerializeField] private Transform headTransform;

    void Start()
    {
        anim.SetBool("IsAlive", true);
        agent = GetComponent<ZombieMovement>();
        
        StartGrowlingSound();
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += StopAgentMovement;
        GameManagerScript.OnPlayerWin += StopAgentMovement;
        GameManagerScript.OnPlayerWin += TriggerDeath;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= StopAgentMovement;
        GameManagerScript.OnPlayerWin -= StopAgentMovement;
        GameManagerScript.OnPlayerWin -= TriggerDeath;
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDeath -= StopAgentMovement;
        GameManagerScript.OnPlayerWin -= StopAgentMovement;
        GameManagerScript.OnPlayerWin -= TriggerDeath;
    }

    void StopAgentMovement()
    {
        agent.enemyDeathStop();
    }

    public void TakeDamage(float amount, Collider colliderHit)
    {
        // Critical instant dead if headshot:
        if (colliderHit.transform.name == headTransform.name)
        {
            zombieHealth = 0;
        }
        else
        {
            zombieHealth -= amount;
        }

        if (zombieHealth <= 0)
        {
            StopAgentMovement();   
            TriggerDeath();
            
            if (OnZombieHpZero != null)
                OnZombieHpZero();
        }
    }

    private void TriggerDeath()
    {
        anim.SetBool("IsAlive", false);
        anim.SetTrigger("Dead");
        gameObject.GetComponent<ZombieMovement>().SetZombieDeath();
    }

    private void DespawnAfterDeathAnim()
    {

        if(OnEnemyKilled != null)
        {
            OnEnemyKilled();
        }

        Destroy(gameObject, 2f);
       
    }

    public void StartGrowlingSound()
    {
        audioSource.PlayOneShot(zombieGrowl);
    }
}
