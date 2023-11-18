using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;
using System;

public class ScoreManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI gameScoreText;
    [SerializeField] private TextMeshProUGUI menuBestScoreText;


    [Header("Settings")]
    [SerializeField] private float scoreMultiplier;
    private int score;
    private int bestScore;

    [Header("Data")]
    private const string bestScoreKey = "bestScoreKey";

    private void Awake()
    {
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
    }

}
