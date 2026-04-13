using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Pong");
    }
    public void StartSnake()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Snake");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}