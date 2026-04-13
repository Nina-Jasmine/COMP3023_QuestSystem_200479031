using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    private bool canPause = true;

    void Update()
    {
        if (!canPause) return;

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (!canPause) return;

        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        Debug.Log("Pause clicked. isPaused = " + isPaused);
    }

    public void SetPauseEnabled(bool enabled)
    {
        canPause = enabled;

        if (!canPause)
        {
            isPaused = false;
            Time.timeScale = 1f;
        }
    }
}