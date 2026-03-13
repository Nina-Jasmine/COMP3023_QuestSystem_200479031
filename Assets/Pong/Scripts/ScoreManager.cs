using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI leftScoreText;
    public TextMeshProUGUI rightScoreText;
    public BallController ball;

    private int leftScore = 0;
    private int rightScore = 0;

    void Start()
    {
        UpdateScoreUI();
    }

    public void ScoreLeft()
    {
        leftScore++;
        UpdateScoreUI();
        ball.IncreaseSpeed();
        ball.ResetBall();
    }

    public void ScoreRight()
    {
        rightScore++;
        UpdateScoreUI();
        ball.IncreaseSpeed();
        ball.ResetBall();
    }

    void UpdateScoreUI()
    {
        leftScoreText.text = leftScore.ToString();
        rightScoreText.text = rightScore.ToString();
    }
}