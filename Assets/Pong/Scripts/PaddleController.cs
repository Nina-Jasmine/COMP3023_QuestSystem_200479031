using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 10f;
    public float limit = 4f;

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
}