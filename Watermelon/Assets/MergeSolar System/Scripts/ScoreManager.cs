using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Apple.GameKit.Leaderboards;
using Apple.GameKit;
using Apple.GameKit.Multiplayer;
using System.Linq;
using System.Threading.Tasks;

public class ScoreManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI gameScoreText;
    [SerializeField] private TextMeshProUGUI menuBestScoreText;
    [SerializeField] private TextMeshProUGUI GameOverBestScoreText;


    [Header("Settings")]
    [SerializeField] private float scoreMultiplier;
    private int score;
    private int bestScore;

    [Header("Data")]
    private const string bestScoreKey = "bestScoreKey";

    public static ScoreManager instance;
    private void Awake()
    {
        instance = this;
        LoadData();
        MergeManager.onMergeProcessed += MergeProcessedCallback;
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;
        GameManager.onGameStateChanged -= GameStateChangedCallback;

    }
    void Start()
    {
        UpdateScoreText();
        UpdateBestScoreText();
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {

            case GameState.Gameover:
                CalculateBestScore();
                break;
            case GameState.GameoverMenu:
                UpdateBestScoreText();
                break;


        }
    }

    private void CalculateBestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            SaveData();
        }
    }

    public void KaraDelikGezegenleriYuttuPuanı(PlanetType planetType)
    {
        int scoreToAdd = (int)planetType;
        score += (int)(scoreToAdd * scoreMultiplier);
        CalculateBestScore();
        UpdateScoreText();
    }

    private void LoadData()
    {
        bestScore = PlayerPrefs.GetInt(bestScoreKey);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(bestScoreKey, bestScore);
    }


    private void MergeProcessedCallback(PlanetType planetType, Vector2 unused)
    {
        int scoreToAdd = (int)planetType;
        score += (int)(scoreToAdd * scoreMultiplier);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        gameScoreText.text = score.ToString();

    }

    private void UpdateBestScoreText()
    {
        menuBestScoreText.text = bestScore.ToString();
        GameOverBestScoreText.text = bestScore.ToString();
        OnReportLeaderboardScore();
        //üsttekinin yorum satırını oyunu yayınlarken kaldır
    }

    public async void OnReportLeaderboardScore()
    {
        var leaderboards = await GKLeaderboard.LoadLeaderboards();
        var leaderboard = leaderboards.First(l => l.BaseLeaderboardId == "Godmelon");

        await leaderboard.SubmitScore(bestScore, 0, GKLocalPlayer.Local);



        var scores = await leaderboard.LoadEntries(GKLeaderboard.PlayerScope.Global, GKLeaderboard.TimeScope.AllTime, 0, 100);

        Debug.LogError($"my score: {scores.LocalPlayerEntry.Score}");

        foreach (var score in scores.Entries)
        {
            Debug.LogError($"score: {score.Score} by {score.Player.DisplayName}");
        }
    }

}
