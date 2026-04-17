using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float delay = 0.8f;

    private void Start()
    {
        Destroy(gameObject, delay);
    }
}