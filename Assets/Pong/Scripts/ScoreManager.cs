using TMPro;
using UnityEngine;

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

    public int winningScore = 5;

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
        // else if (winText != null)
        //     winText.gameObject.SetActive(false);
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
    }
}