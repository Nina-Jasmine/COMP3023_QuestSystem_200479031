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
    public float moveDelay = 0.2f;
    public GameObject gameOverPanel;
    public AudioSource backgroundMusic;
    public AudioSource gameOverSound;

    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;
    public TextMeshProUGUI scoreText;
    private int score = 0;

    public GameObject bodySegmentPrefab;

    private List<Transform> bodySegments = new List<Transform>();
    private Vector3 lastPosition;
    private int pendingGrowth = 0;

    void Start()
    {
        lastPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
        UpdateScoreUI();
    }

    void Update()
    {
        if (isGameOver) return;

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && direction != Vector2.down)
            nextDirection = Vector2.up;

        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && direction != Vector2.up)
            nextDirection = Vector2.down;

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && direction != Vector2.right)
            nextDirection = Vector2.left;

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && direction != Vector2.left)
            nextDirection = Vector2.right;
    }

    void FixedUpdate()
    {
        if (isGameOver) return;

        moveTimer += Time.fixedDeltaTime;

        if (moveTimer >= moveDelay)
        {
            moveTimer = 0f;

            direction = nextDirection;

            Vector3 previousPosition = transform.position;
            Vector3 nextPosition = transform.position + (Vector3)direction;

            // Wall hit = game over
            if (nextPosition.x < minX || nextPosition.x > maxX || nextPosition.y < minY || nextPosition.y > maxY)
            {
                Debug.Log("Game Over - hit wall at next position: " + nextPosition +
                          " | limits: " + minX + "," + maxX + "," + minY + "," + maxY);
                isGameOver = true;
                pendingGrowth = 0;
                HideBody();
                if (backgroundMusic != null)
                    backgroundMusic.Stop();

                if (gameOverSound != null)
                    gameOverSound.Play();

                if (gameOverPanel != null)
                    gameOverPanel.SetActive(true);
                return;
            }

            transform.position = nextPosition;

            for (int i = 0; i < bodySegments.Count; i++)
            {
                Vector3 temp = bodySegments[i].position;
                bodySegments[i].position = previousPosition;
                previousPosition = temp;
            }

            lastPosition = previousPosition;

            if (pendingGrowth > 0)
            {
                GameObject newSegment = Instantiate(bodySegmentPrefab, lastPosition, Quaternion.identity);
                bodySegments.Add(newSegment.transform);
                pendingGrowth--;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOver) return;

        if (other.CompareTag("Food"))
        {
            Debug.Log("Food eaten!");

            if (audioSource != null)
                audioSource.Play();

            pendingGrowth++;
            score += 10;
            UpdateScoreUI();
            RepositionFood(other.transform);
        }
    }

    void RepositionFood(Transform food)
    {
        float foodMinX = -8f;
        float foodMaxX = 8f;
        float foodMinY = -3f;
        float foodMaxY = 4f;

        float x = Mathf.Round(Random.Range(foodMinX, foodMaxX));
        float y = Mathf.Round(Random.Range(foodMinY, foodMaxY));

        food.position = new Vector3(x, y, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGameOver) return;

        if (collision.gameObject.CompareTag("Body"))
        {
            Debug.Log("Game Over - hit body");
            isGameOver = true;
            pendingGrowth = 0;
            HideBody();
            if (backgroundMusic != null)
                backgroundMusic.Stop();

            if (gameOverSound != null)
                gameOverSound.Play();

            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);


        }
    }
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
    void Grow()
    {
        Vector3 spawnPosition = new Vector3(
            Mathf.Round(lastPosition.x),
            Mathf.Round(lastPosition.y),
            0f
        );

        GameObject newSegment = Instantiate(bodySegmentPrefab, spawnPosition, Quaternion.identity);
        bodySegments.Add(newSegment.transform);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    void HideBody()
    {
        for (int i = 0; i < bodySegments.Count; i++)
        {
            if (bodySegments[i] != null)
                bodySegments[i].gameObject.SetActive(false);
        }
    }
}