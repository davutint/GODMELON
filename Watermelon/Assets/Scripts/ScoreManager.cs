using UnityEngine;
using TMPro;
using DG.Tweening;
using Apple.GameKit.Leaderboards;
using Apple.GameKit;
using System.Linq;
using UnityEngine.SceneManagement;


public class ScoreManager : MonoBehaviour
{
	[Header("Elements")]
	[SerializeField] private TextMeshProUGUI gameScoreText;
	
	[Header("Settings")]
	[SerializeField] private float scoreMultiplier;
	public int score;
	public int lastHighScore;
	private int bestScore;

	[Header("Data")]
	private const string bestScoreKey = "bestScoreKey";

	public static ScoreManager instance;
	public bool isDestroyerUsable=true;
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
		lastHighScore=bestScore;
		
	}

	private void GameStateChangedCallback(GameState gameState)
	{
		switch (gameState)
		{

			case GameState.Gameover:
				CalculateBestScore();
				break;
			case GameState.GameoverMenu:
				CalculateBestScore();
				CheckGameOverBadOrGood();
				break;


		}
	}

	private void WatchAdToDoubleScore()//bunu reklam izlediğinde alabilecek
	{
		score=score*2;
		CalculateBestScore();
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
	public void MeteorEventGetPoint(PlanetType planetType)
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
		int scoreToAdd=(int)planetType;
		score += (int)(scoreToAdd * scoreMultiplier);
		UIManager.instance.RoundTime+=(float)planetType/9;//merge olunca round time artıyor.
		if (GameManager.instance.IsGameState())
		{
			IncreaseSliderValueSmoothly(scoreToAdd);
		}
		
		
		UpdateScoreText();
		Debug.Log(planetType+" "+(float)planetType/9+" kadar zamana ekledi");
	}
	public void IncreaseSliderValueSmoothly(int increment)
	{
		
		float currentValue = UIManager.instance.destroyerSlider.value;
		float targetValue = currentValue + increment;
		
		UIManager.instance.destroyerSlider.DOValue(targetValue, 0.5f) // 0.5 saniye içinde smooth geçiş
					  .SetEase(Ease.InOutQuad); // Ease türünü isteğinize göre ayarlayabilirsiniz
					  
		if (UIManager.instance.destroyerSlider.value>=UIManager.instance.destroyerSlider.maxValue)
		{
			//destroyer butonu ekrana gelicek
			if (isDestroyerUsable)
			{
				UIManager.instance.ActivateDestroyerButton();
			}
			else
			{
				return;
			}
			
		}
	}

	public void UpdateScoreText()
	{
		gameScoreText.text = score.ToString();

	}


	private void CheckGameOverBadOrGood()//APPLE LİDERTABLOSUNA BURADA YOLLUYORUZ
	{
		
		//apple lidertablosunu burada güncelle
		
		int sceneindex=SceneManager.GetActiveScene().buildIndex;
		if (sceneindex==4)
		{
			OnReportTimerLeaderboardScore();
		}
		else
		{
			OnReportLeaderboardScore();
		}
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
	public async void OnReportTimerLeaderboardScore()
	{
		var leaderboards = await GKLeaderboard.LoadLeaderboards();
		var leaderboard = leaderboards.First(l => l.BaseLeaderboardId == "TimerGodmelon");

		await leaderboard.SubmitScore(bestScore, 0, GKLocalPlayer.Local);



		var scores = await leaderboard.LoadEntries(GKLeaderboard.PlayerScope.Global, GKLeaderboard.TimeScope.AllTime, 0, 100);

		Debug.LogError($"my score: {scores.LocalPlayerEntry.Score}");

		foreach (var score in scores.Entries)
		{
			Debug.LogError($"score: {score.Score} by {score.Player.DisplayName}");
		}
	}


}
