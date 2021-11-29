using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [HideInInspector] public static bool isPlayerAlive = true;
    [HideInInspector] public static bool isPlayerWin = false;

    [SerializeField] private int zombieLeftToKill = 30;
    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private GameObject WinCanvas;
    [SerializeField] private TextMeshProUGUI zombieLeftValueText;

    public delegate void PlayerWin();
    public static event PlayerWin OnPlayerWin;
    
    void OnEnable()
    {
        isPlayerAlive = true;
        isPlayerWin = false;

        GameOverCanvas.SetActive(false);

        PlayerHealth.OnPlayerDeath += PlayerDied;
        ZombieBehavior.OnZombieHpZero += ReduceZombieLeftToKill;

        zombieLeftValueText.gameObject.SetActive(true);
        zombieLeftValueText.SetText(zombieLeftToKill.ToString());
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= PlayerDied;
        ZombieBehavior.OnZombieHpZero -= ReduceZombieLeftToKill;
    }

    void ReduceZombieLeftToKill()
    {
        --zombieLeftToKill;
        zombieLeftValueText.SetText(zombieLeftToKill.ToString());

        if (zombieLeftToKill <= 0)
        {
            isPlayerWin = true;
            Cursor.lockState = CursorLockMode.Confined;
            WinCanvas.SetActive(true);

            if (OnPlayerWin != null)
                OnPlayerWin();
        }
    }

    void PlayerDied()
    {
        isPlayerAlive = false;
        Cursor.lockState = CursorLockMode.Confined;
        GameOverCanvas.SetActive(true);
    }

    public void GoToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
