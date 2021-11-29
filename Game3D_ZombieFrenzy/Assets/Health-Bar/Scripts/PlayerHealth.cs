using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    public int maxHealth = 5;
    private int currentHealth;

    public HealthBar healthBar;

    [Header("Player Hurt and Death Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip playerHurtSound;
    [SerializeField] private AudioClip playerDeathSound;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float amount, Collider colliderHit)
    {
        if (GameManagerScript.isPlayerAlive && !GameManagerScript.isPlayerWin)
        {
            currentHealth -= (int) amount;

            healthBar.SetHealth(currentHealth);

            if (currentHealth <= 0)
            {
                audioSource.PlayOneShot(playerDeathSound);

                if (OnPlayerDeath != null)
                    OnPlayerDeath();
            }
            else
            {
                audioSource.PlayOneShot(playerHurtSound);
            }
        }
    }
}
