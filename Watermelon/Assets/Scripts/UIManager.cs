using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class UIManager : MonoBehaviour
{
	private const string ArkaPlanData = "ArkaPlanData";
	[Header("Elements")]
	public Slider xpSlider;
	
	public TextMeshProUGUI SliderBarText;
	
	public TextMeshProUGUI GainedExpText;
	public TextMeshProUGUI characterLevelText;
	public TextMeshProUGUI earningGolds;
	public TextMeshProUGUI lastHighScoreText;
	public TextMeshProUGUI endGameScoreText;
	
	[SerializeField] private GameObject gamePanel;
	[SerializeField] private GameObject settingsPanel;
	[SerializeField] private GameObject gameOverPanel;
	[SerializeField] private GameObject gameEndPanel;
	
	[SerializeField]private RectTransform destroyerEventButtonObj;
	public RectTransform destroyerSliderObj;
	public Slider destroyerSlider;
	public GameObject stars;
	[Header("Zamana Karşı Yarış")]
	public float RoundTime = 60f;
	[SerializeField]private GameObject timerContainer=null;
	public TextMeshProUGUI timerText;
	public GameObject BlackHoleButtonObj;
	//public Transform BestScoreObj, bestScoreTextObj, RestartButonobj, GameoverTextobj, SupportDeveloperObj, LiderTablosuobj;
	//[SerializeField] private GameObject MaviArkaPlan; //, SiyahArkaPlan, PembeArkaPlan;//burada sadece bir tanesi aktif olacak ve bu sahneye göre olacak. Her sahnede farklı atama yapacağız
	//public Authentication authentication;
	
	private int planetDestroyedCount;
	public static Action KaraDelikEvent;

	public static UIManager instance;

	public GameObject KırmızıOlcakPanel;
	
	private ShipData shipData;
	public void BlackHoleButton()
	{
		AdManager.instance.OdulluGoster();
		//GameManager.instance.SetBlackHoleState();
		
	}
	public void OnProtonBeamButtonClicked()
	{
		
		List<Planet> planets=new List<Planet>();
		planets.AddRange(FindObjectsOfType<Planet>());
		
		if (planets.Count<planetDestroyedCount+1)
		{
			return;
		}
		//reklam aç
		GameManager.instance.SetProtonBeamState();
		destroyerUIbuttonsMove();

		
		//butona basınca butonu uçuruyoruz
		

	}
	public void destroyerUIbuttonsMove()//timer sahnesinde destroyersliderobj bizim skillContainerımız
	{
		//destroyerEventButtonObj.DOAnchorPosX(1600,.5f).SetEase(Ease.InOutBack);
		destroyerEventButtonObj.gameObject.SetActive(false);
		destroyerSliderObj.DOAnchorPosX(-1900,.5f).SetEase(Ease.InOutBack);
	}
	public void SkillContainerBack()
	{
		
		destroyerSliderObj.DOAnchorPosX(0,.5f).SetEase(Ease.InOutBack);
	}

	private async void Awake()
	{
		instance = this;
		GameManager.onGameStateChanged += GameStateChangedCallback;
		
		
		shipData=await GameCloudDatas.LoadData<ShipData>("ship");//yokedilebilecek gezegen sayısını çektik
		planetDestroyedCount=shipData.destroyPlanetCount;
		//geminin hızını, yani dolması gereken slider değerini çektik.
	}


	private void Start()
	{
		destroyerSlider.value=0;
		
	}
	private void OnDestroy()
	{
		GameManager.onGameStateChanged -= GameStateChangedCallback;

	}
	


	
	public void ActivateDestroyerButton()
	{
		destroyerEventButtonObj.DOAnchorPosX(-150,.5f).SetEase(Ease.InOutBack).OnComplete(()=>
		{
			ScoreManager.instance.isDestroyerUsable=false;
		});
	}
	

	public void DenemeMeteor()
	{
		GameManager.instance.SetMeteorState();
	}

	public void KaraDelikButonuAktifEt()
	{
		//KaraDelikButonobj.SetActive(true);
		KaraDelikButonuAnim(0f);

	}

	public void MainMenu()
	{
		SceneManager.LoadScene(0);
	}
	public void KaraDelikButonuAnim(float position)
	{
		//Butonobj.transform.DOLocalMoveX(position, .55f).SetEase(Ease.InOutBack);
		//TextHolderobj.transform.DOLocalMoveX(position, .55f).SetEase(Ease.InOutBack);
	}

	private void GameStateChangedCallback(GameState gameState)
	{
		switch (gameState)
		{
			case GameState.Game:
				SetGame();
				break;
			case GameState.ProtonBeam:
			ProtonBeamState();
				break;
			case GameState.Gameover:
				GameOverState();
				break;
			case GameState.GameoverMenu:
				SetGameOverMenu();
				CharacterExpDataManager.instance.GameOverSetExpLevel(ScoreManager.instance.score);
				break;
			case GameState.Settings:
				SetSettings();
				break;
			case GameState.TimeRace:
				TimeRacePanel();
				SetGame();
				break;
			

		}
	}

	private void TimeRacePanel()
	{
		if (timerContainer!=null)
		{
			timerContainer.SetActive(true);
			SkillContainerBack();
			
		}
		GameManager.instance.planetSelectionObj.transform.DOLocalMoveX(1600,.5f).OnComplete(()=>
		{
			GameManager.instance.protonPanel.SetActive(false);
		});// eski yerine götür;
	   
	}

	private void SetGame()
	{
		gamePanel.SetActive(true);
		
		gameOverPanel.SetActive(false);
		settingsPanel.SetActive(false);
		GameManager.instance.planetSelectionObj.transform.DOLocalMoveX(1600,.5f).OnComplete(()=>
		{
			GameManager.instance.protonPanel.SetActive(false);
		});// eski yerine götür;
	}
	
	private void ProtonBeamState()
	{
		GameManager.instance.protonPanel.SetActive(true);
		GameManager.instance.planetSelectionObj.transform.DOLocalMoveX(0,.5f);// ekranın ortasına getir;
	}
	private void SetSettings()
	{
		settingsPanel.SetActive(true);
		gamePanel.SetActive(false);
		
		gameOverPanel.SetActive(false);
		
	}

	private void GameOverState()
	{
		GameManager.instance.SetGameoverState();
	}
	private void SetGameOverMenu()//buraya bir dotween kullanarak animasyon oluştur
	{
		//bir başka paneli kırmızıya döndür ve kırmızıya döndürdükten sonra aşağıdakiler çalışsın.
		//karadelik eventi burada olmalı artık
		
		KırmızıOlcakPanel.GetComponent<Image>().DOFade(.8f, 1f).OnComplete(() =>
		{
			gameOverPanel.SetActive(true);
			
			gamePanel.SetActive(false);
			settingsPanel.SetActive(false);
			int score=ScoreManager.instance.score;
			gameEndPanel.SetActive(true);
			if (score>ScoreManager.instance.lastHighScore)//en son yüksek skoredan fazla yapmışsak paneli ona uygun olanı açıyoruz
			{
				//starları burada aktif et
				stars.SetActive(true);
				lastHighScoreText.text="Last High Score: "+ScoreManager.instance.lastHighScore.ToString();
			}
			else
			{
				//burada deactive et
				stars.SetActive(false);
				lastHighScoreText.text="Last High Score: "+ScoreManager.instance.lastHighScore.ToString();
			}

			//animasyonlar

			//MenuAnimOynat();

		});


	}


	public void LiderTablosuAc()
	{
		//authentication.OnShowLeaderboard();
		//yorum satırını kaldır yukardakinin
	}

	/*public void MenuAnimOynat()
	{

		GameoverTextobj.DOLocalMoveY(550f, 1f).SetEase(Ease.InOutBack);
		RestartButonobj.DOLocalMoveX(0, 1f).SetEase(Ease.InOutBack);
		LiderTablosuobj.DOLocalMoveX(0, 1f).SetEase(Ease.InOutBack);
		bestScoreTextObj.DOLocalMoveX(0, 1f).SetEase(Ease.InOutBack);
		BestScoreObj.DOLocalMoveX(0, 1f).SetEase(Ease.InOutBack);
		SupportDeveloperObj.DOLocalMoveX(230f, 1f).SetEase(Ease.InOutBack);
		SupportDeveloperObj.DOScale(new Vector2(.87f, 1.23f), .55f).SetLoops(-1, LoopType.Yoyo);



	}*/



	public void SupportDeveloper()
	{
		AudioManager.instance.ClickSoundCal();
		//AdManager.instance.OdulluGoster();
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void PlayButtonCallback()
	{
		AudioManager.instance.ClickSoundCal();
		GameManager.instance.SetStartingState();
		SetGame();
	}

	public void RestartGame()
	{
		AudioManager.instance.ClickSoundCal();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
