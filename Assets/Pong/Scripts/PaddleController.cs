using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 12f;
    public float limit = 3.5f;

    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;

    void Update()
    {
        float move = 0f;

        if (Input.GetKey(upKey))
            move = 1f;
        else if (Input.GetKey(downKey))
            move = -1f;

        transform.Translate(Vector2.up * move * speed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, -limit, limit);
        transform.position = pos;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        BallController ball = collision.gameObject.GetComponent<BallController>();

        if (ball != null)
        {
            bool isLeftPaddle = transform.position.x < 0;
            float paddleHeight = transform.localScale.y;

            ball.BounceFromPaddle(transform.position.y, paddleHeight, isLeftPaddle);
        }
    }
}