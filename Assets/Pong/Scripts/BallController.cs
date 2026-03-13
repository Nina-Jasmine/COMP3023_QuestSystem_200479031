using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 10f;
    public float speedIncrease = 1f;
    public float maxSpeed = 16f;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private int launchDirection = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        LaunchBall();
    }

    void LaunchBall()
    {
        float yDirection = Random.Range(-0.75f, 0.75f);

        Vector2 direction = new Vector2(launchDirection, yDirection).normalized;
        rb.linearVelocity = direction * speed;

        launchDirection *= -1;
    }

    public void ResetBall()
    {
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;
        LaunchBall();
    }

    public void IncreaseSpeed()
    {
        speed = Mathf.Min(speed + speedIncrease, maxSpeed);
    }

    public void BounceFromPaddle(float paddleY, float paddleHeight, bool hitLeftPaddle)
    {
        float offset = transform.position.y - paddleY;
        float normalizedOffset = offset / (paddleHeight / 2f);

        float xDirection = hitLeftPaddle ? 1f : -1f;
        Vector2 newDirection = new Vector2(xDirection, normalizedOffset).normalized;

        rb.linearVelocity = newDirection * speed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}