using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    public int maxHealth = 5;
    private int currentHealth;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            if (OnPlayerDeath != null)
                OnPlayerDeath();
        }
    }
}
