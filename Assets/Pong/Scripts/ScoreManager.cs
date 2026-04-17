using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI leftScoreText;
    public TextMeshProUGUI rightScoreText;
    public TextMeshProUGUI winText;
    public GameObject winPanel;
    public BallController ball;
    public Transform leftPaddle;
    public Transform rightPaddle;
    public MusicPlayer musicPlayer;
    public GameObject pauseButton;
    public PauseManager pauseManager;

    public int winningScore = 7;

    private int leftScore = 0;
    private int rightScore = 0;
    private bool gameOver = false;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateScoreUI();

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    public void ScoreLeft()
    {
        if (gameOver) return;

        leftScore++;
        UpdateScoreUI();
        CheckWinner();

        if (!gameOver)
        {
            ball.IncreaseSpeed();
            ball.ResetBall();
        }
    }

    public void ScoreRight()
    {
        if (gameOver) return;

        rightScore++;
        UpdateScoreUI();
        CheckWinner();

        if (!gameOver)
        {
            ball.IncreaseSpeed();
            ball.ResetBall();
        }
    }

    void UpdateScoreUI()
    {
        leftScoreText.text = leftScore.ToString();
        rightScoreText.text = rightScore.ToString();
    }

    void CheckWinner()
    {
        if (leftScore >= winningScore)
        {
            gameOver = true;
            ShowWinner("Left Player Wins!");
        }
        else if (rightScore >= winningScore)
        {
            gameOver = true;
            ShowWinner("Right Player Wins!");
        }
    }

    void ShowWinner(string message)
    {
        ball.StopBall();
        ball.HideBall();

        PaddleController leftController = leftPaddle.GetComponent<PaddleController>();
        if (leftController != null)
            leftController.enabled = false;

        AIPaddleController aiController = rightPaddle.GetComponent<AIPaddleController>();
        if (aiController != null)
            aiController.enabled = false;

        leftPaddle.position = new Vector3(leftPaddle.position.x, 0f, leftPaddle.position.z);
        rightPaddle.position = new Vector3(rightPaddle.position.x, 0f, rightPaddle.position.z);

        if (musicPlayer != null)
            musicPlayer.StopMusic();

        if (audioSource != null)
            audioSource.Play();

        if (winPanel != null)
            winPanel.SetActive(true);

        if (winText != null)
            winText.text = message;

        if (pauseButton != null)
            pauseButton.SetActive(false);
        if (pauseManager != null)
            pauseManager.SetPauseEnabled(false);
    }

    public void RestartGame()
    {
        Debug.Log("Restart button clicked");
        leftScore = 0;
        rightScore = 0;
        gameOver = false;

        UpdateScoreUI();

        if (winPanel != null)
            winPanel.SetActive(false);

        if (pauseButton != null)
            pauseButton.SetActive(true);

        ball.gameObject.SetActive(true);
        ball.ResetSpeed();
        ball.ResetBall();

        leftPaddle.position = new Vector3(leftPaddle.position.x, 0f, leftPaddle.position.z);
        rightPaddle.position = new Vector3(rightPaddle.position.x, 0f, rightPaddle.position.z);

        PaddleController leftController = leftPaddle.GetComponent<PaddleController>();
        if (leftController != null)
            leftController.enabled = true;

        AIPaddleController aiController = rightPaddle.GetComponent<AIPaddleController>();
        if (aiController != null)
            aiController.enabled = true;

        if (musicPlayer != null)
            musicPlayer.PlayMusic();
        if (pauseManager != null)
            pauseManager.SetPauseEnabled(true);
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}