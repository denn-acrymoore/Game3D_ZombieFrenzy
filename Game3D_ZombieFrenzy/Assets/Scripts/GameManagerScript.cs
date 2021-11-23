using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [HideInInspector] public static bool isPlayerAlive = true;
    [HideInInspector] public static int currScore = 0;

    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private TextMeshProUGUI currentScoreGameOver;
    [SerializeField] private TextMeshProUGUI bestScoreGameOver;
    [SerializeField] private TextMeshProUGUI currScoreHUD;
    
    void OnEnable()
    {
        isPlayerAlive = true;
        currScore = 0;

        GameOverCanvas.SetActive(false);

        PlayerHealth.OnPlayerDeath += PlayerDied;
        ZombieBehavior.OnZombieHpZero += AddScore;

        currScoreHUD.gameObject.SetActive(true);
        currScoreHUD.SetText("Score: " + currScore);
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= PlayerDied;
        ZombieBehavior.OnZombieHpZero -= AddScore;
    }

    void AddScore()
    {
        currScore += 10;
        currScoreHUD.SetText("Score: " + currScore);
    }

    void PlayerDied()
    {
        currScoreHUD.gameObject.SetActive(false);

        isPlayerAlive = false;
        Cursor.lockState = CursorLockMode.Confined;

        GameOverCanvas.SetActive(true);

        int bestScore = PlayerPrefs.GetInt("HighScore", 0);

        if (currScore > bestScore)
        {
            PlayerPrefs.SetInt("HighScore", currScore);
            bestScore = currScore;
        }

        currentScoreGameOver.SetText("Score: " + currScore);
        bestScoreGameOver.SetText("Best Score: " + bestScore);
    }

    public void GoToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToGameSceneAgain()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
