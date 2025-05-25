using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int playerScore = 0;
    public TMP_Text scoreText;

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        playerScore += points;
        UpdateScoreUI();
        Debug.Log("Skor: " + playerScore);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + playerScore;
        }
    }

    public void ResetScore()
    {
        playerScore = 0;
        UpdateScoreUI();
        Debug.Log("Skor sýfýrlandý!");
    }
}
