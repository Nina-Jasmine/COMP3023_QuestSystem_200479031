using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public bool isLeftGoal;
    public ScoreManager scoreManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            if (isLeftGoal)
            {
                scoreManager.ScoreRight();
            }
            else
            {
                scoreManager.ScoreLeft();
            }
        }
    }
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     Debug.Log("Something entered goal: " + other.name);

    //     if (other.CompareTag("Ball"))
    //     {
    //         Debug.Log("Ball entered goal!");

    //         if (isLeftGoal)
    //             scoreManager.ScoreRight();
    //         else
    //             scoreManager.ScoreLeft();
    //     }
    // }
}