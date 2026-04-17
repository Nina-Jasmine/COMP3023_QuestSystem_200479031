using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SnakeHead : MonoBehaviour
{
    private Vector2 direction = Vector2.right;
    private Vector2 nextDirection = Vector2.right;

    private AudioSource audioSource;
    private bool isGameOver = false;
    private float moveTimer = 0f;

    public float moveDelay = 0.35f;
    public GameObject startPanel;
    private bool gameStarted = false;
    private static bool skipStartPanel = false;
    public GameObject gameOverPanel;
    public GameObject eatEffectPrefab;
    public GameObject deathEffectPrefab;
    public AudioSource backgroundMusic;
    public AudioSource gameOverMusic;
    public AudioSource gameOverSound;

    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;
    public int foodMinX = -7;
    public int foodMaxX = 7;
    public int foodMinY = -3;
    public int foodMaxY = 3;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI finalScoreText;
    private int bestScore = 0;
    private int score = 0;

    public GameObject bodySegmentPrefab;

    private List<Transform> bodySegments = new List<Transform>();
    private int pendingGrowth = 0;

    void Start()
    {

        audioSource = GetComponent<AudioSource>();
        UpdateScoreUI();

        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateBestScoreUI();

        if (bodySegmentPrefab == null)
        {
            Debug.LogError("Body Segment Prefab is not assigned!");
        }

        if (skipStartPanel)
        {
            gameStarted = true;
            if (startPanel != null)
                startPanel.SetActive(false);
        }
        else
        {
            gameStarted = false;
            if (startPanel != null)
                startPanel.SetActive(true);
        }
    }

    void Update()
    {
        if (!gameStarted)
        {
            if (Input.anyKeyDown)
            {
                gameStarted = true;

                if (startPanel != null)
                    startPanel.SetActive(false);
            }
            return;
        }

        if (isGameOver) return;

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && direction != Vector2.down)
            nextDirection = Vector2.up;

        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && direction != Vector2.up)
            nextDirection = Vector2.down;

        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && direction != Vector2.right)
            nextDirection = Vector2.left;

        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && direction != Vector2.left)
            nextDirection = Vector2.right;
    }

    void FixedUpdate()
    {
        if (!gameStarted) return;
        if (isGameOver) return;

        moveTimer += Time.fixedDeltaTime;

        if (moveTimer >= moveDelay)
        {
            moveTimer = 0f;

            direction = nextDirection;

            Vector3 oldHeadPosition = transform.position;
            Vector3 newHeadPosition = oldHeadPosition + (Vector3)direction;

            // Check wall collision before moving
            if (newHeadPosition.x < minX || newHeadPosition.x > maxX || newHeadPosition.y < minY || newHeadPosition.y > maxY)
            {
                Debug.Log("Game Over - hit wall");
                GameOver();
                return;
            }

            // Check body collision before moving
            int bodyCheckCount = bodySegments.Count;

            // If not growing this turn, ignore the tail segment
            // because it will move away during this step
            if (pendingGrowth == 0 && bodySegments.Count > 0)
            {
                bodyCheckCount = bodySegments.Count - 1;
            }

            for (int i = 0; i < bodyCheckCount; i++)
            {
                if (bodySegments[i] != null && bodySegments[i].position == newHeadPosition)
                {
                    Debug.Log("Game Over - hit body");
                    GameOver();
                    return;
                }
            }

            // Move head
            transform.position = newHeadPosition;
            // Check food by position (grid-based, reliable)
            GameObject food = GameObject.FindGameObjectWithTag("Food");

            if (food != null && food.transform.position == transform.position)
            {
                Debug.Log("Food eaten!");

                if (audioSource != null)
                    audioSource.Play();
                if (eatEffectPrefab != null)
                {
                    Instantiate(eatEffectPrefab, food.transform.position + Vector3.up * 0.1f, Quaternion.identity);
                }

                pendingGrowth++;
                score += 10;

                if (score > bestScore)
                {
                    bestScore = score;
                    PlayerPrefs.SetInt("BestScore", bestScore);
                    UpdateBestScoreUI();
                }

                if (score % 30 == 0)
                {
                    moveDelay = Mathf.Max(0.08f, moveDelay - 0.02f);
                }

                UpdateScoreUI();
                RepositionFood(food.transform);
            }

            // Move body
            Vector3 previousPosition = oldHeadPosition;

            for (int i = 0; i < bodySegments.Count; i++)
            {
                Vector3 temp = bodySegments[i].position;
                bodySegments[i].position = previousPosition;
                previousPosition = temp;
            }

            // Add new body segment if needed
            if (pendingGrowth > 0 && bodySegmentPrefab != null)
            {
                GameObject newSegment = Instantiate(bodySegmentPrefab, previousPosition, Quaternion.identity);
                bodySegments.Add(newSegment.transform);
                pendingGrowth--;
            }
        }
    }


    void RepositionFood(Transform food)
    {
        Vector3 newPosition;
        bool validPosition;

        int gridMinX = Mathf.RoundToInt(minX);
        int gridMaxX = Mathf.RoundToInt(maxX);
        int gridMinY = Mathf.RoundToInt(minY);
        int gridMaxY = Mathf.RoundToInt(maxY);

        do
        {
            validPosition = true;

            int x = Random.Range(foodMinX, foodMaxX + 1);
            int y = Random.Range(foodMinY, foodMaxY + 1);

            newPosition = new Vector3(x, y, 0f);

            if (transform.position == newPosition)
                validPosition = false;

            for (int i = 0; i < bodySegments.Count; i++)
            {
                if (bodySegments[i] != null && bodySegments[i].position == newPosition)
                {
                    validPosition = false;
                    break;
                }
            }

        } while (!validPosition);

        food.position = newPosition;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
    void UpdateBestScoreUI()
    {
        if (bestScoreText != null)
            bestScoreText.text = "Best: " + bestScore;
    }

    void UpdateFinalScoreUI()
    {
        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + score;
    }

    void GameOver()
    {
        isGameOver = true;
        pendingGrowth = 0;


        if (backgroundMusic != null)
            backgroundMusic.Stop();

        if (gameOverSound != null)
            gameOverSound.Play();

        if (gameOverMusic != null)
            gameOverMusic.Play();

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity);
        }

        SpriteRenderer headRenderer = GetComponent<SpriteRenderer>();
        if (headRenderer != null)
            headRenderer.enabled = false;

        for (int i = 0; i < bodySegments.Count; i++)
        {
            if (bodySegments[i] != null)
            {
                SpriteRenderer sr = bodySegments[i].GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.enabled = false;
            }
        }

        UpdateFinalScoreUI();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        skipStartPanel = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        skipStartPanel = false;
        SceneManager.LoadScene("MainMenu");
    }
}