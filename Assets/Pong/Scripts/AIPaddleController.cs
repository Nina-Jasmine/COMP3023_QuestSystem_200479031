using UnityEngine;

public class AIPaddleController : MonoBehaviour
{
    public Transform ball;
    public float speed = 6f;
    public float limit = 3.5f;
    public float deadZone = 0.1f;

    void Update()
    {
        if (ball == null) return;

        float difference = ball.position.y - transform.position.y;

        if (Mathf.Abs(difference) > deadZone)
        {
            Vector3 targetPosition = new Vector3(
                transform.position.x,
                ball.position.y,
                transform.position.z
            );

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );
        }

        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, -limit, limit);
        transform.position = pos;
    }
}