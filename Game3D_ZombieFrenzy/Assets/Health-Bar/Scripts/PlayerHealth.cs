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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    //Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        TakeDamage(1);
    //    }
    //}

    public void TakeDamage(float amount, Collider colliderHit)
    {
        if (GameManagerScript.isPlayerAlive)
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
