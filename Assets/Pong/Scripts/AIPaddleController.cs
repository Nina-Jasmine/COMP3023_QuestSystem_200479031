using UnityEngine;

public class AIPaddleController : MonoBehaviour
{
    public Transform ball;
    public float speed = 8f;
    public float limit = 3.5f;

    void Update()
    {
        if (ball == null) return;

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

        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, -limit, limit);
        transform.position = pos;
    }
}