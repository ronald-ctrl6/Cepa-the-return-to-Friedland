using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Score")]
    [SerializeField] private int currentScore;
    [SerializeField] private Text scoreText;

    [Header("Events")]
    public UnityEvent<int> OnScoreChanged;

    public int CurrentScore
    {
        get { return currentScore; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AddPoints(int points)
    {
        currentScore += points;
        UpdateScoreUI();
        OnScoreChanged.Invoke(currentScore);
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
        OnScoreChanged.Invoke(currentScore);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }
}