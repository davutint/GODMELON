using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;


public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	[Header("Setting")]
	private GameState gameState;

	[Header("Actions")]
	public static Action<GameState> onGameStateChanged;

	[Header("Proton Beam")]
	public ParticleSystem protonBeamPrefab; // Proton ışını efektinin prefab'ı
	public Transform spaceshipTransform; // Uzay gemisinin Transform referansı
	
	private List<ISelectable> selectedPlanets = new List<ISelectable>();
	private List<Sprite> selectedPlanetsSprite = new List<Sprite>();
	[Header("PORTAL")]
	public GameObject portal;
	public GameObject spaceShip;
	public GameObject effectObj;
	[SerializeField]GameObject targetGameobj;
	public GameObject planetSelectionObj;
	public GameObject protonPanel;
	[SerializeField]GameObject secondTarget;
	[SerializeField]LeanTweenType tweenType;
	[SerializeField]Image[] selectedPlanetsImages;
	
	bool manage=false;
	public bool blackHoleActive;
	public GameState startingState;
	public int gettingDestroyedPlanetCount;
	private ShipData shipData;
	public GameState selectingGameState;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else Destroy(gameObject);
		

	}
	private async void Start()
	{
		shipData=await GameCloudDatas.LoadData<ShipData>("ship");
		gettingDestroyedPlanetCount=shipData.destroyPlanetCount;
		for (int i = 0; i < gettingDestroyedPlanetCount; i++)
		{
			selectedPlanetsImages[i].gameObject.SetActive(true);
		}
		DOTween.SetTweensCapacity(1250, 50);
		SetGameState(startingState);
		
	}

	public void SetProtonBeamState()
	{
		SetGameState(GameState.ProtonBeam);
		
	}

	private void Update()
	{
		if (gameState == GameState.ProtonBeam)
		{
			Portal();
		}
		if (gameState==GameState.BlackHole)
		{
			ManageBlackHole();
		}
	}
	
	public void ManageBlackHole()
	{
		if(blackHoleActive==false)
		{
			//Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			BlackHole.instance.BlackHoleProcessCallback();
			blackHoleActive=true;
		}
	}

	public void Portal()
	{
		if (!manage&portal!=null)
		{
			effectObj.SetActive(true);
			portal.SetActive(true);
			
		}
		
		StartCoroutine(bekle());
		
	}
	IEnumerator bekle()
	{
		//AudioManager.instance.PortalSound();
		yield return new WaitForSeconds(2f);
		
		if (gameState == GameState.ProtonBeam)
		{
			
			spaceShip.transform.DOMove(targetGameobj.transform.position,.5f).OnComplete(()=>
			{
				spaceShip.transform.SetParent(null);
				//protonPanel.SetActive(true);
				
				ManageProtonBeamSelection();
			});
		}
		
	}
	private void ManageProtonBeamSelection()
	{	
		portal.transform.DOScale(Vector3.zero,2f).OnComplete(()=>
		{
			DOTween.Kill(portal);
			portal.SetActive(false);
			
			
		});
		
		manage=true;
		if (Input.GetMouseButtonDown(0))
		{
			
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);

			if (hitCollider != null && hitCollider.GetComponent<ISelectable>() != null)
			{
				ISelectable selectable = hitCollider.GetComponent<ISelectable>();
				if (!selectedPlanets.Contains(selectable))
				{
					
					selectedPlanets.Add(selectable);
					selectedPlanetsSprite.Add(selectable.GetSprite());
					
					for (int i = 0; i < selectedPlanets.Count && i < gettingDestroyedPlanetCount; i++)
					{
						selectedPlanetsImages[i].sprite = selectedPlanetsSprite[i]; // bir UI ekranında seçilen gezegen spritelarını görüyoruz.
						if (selectedPlanets[i]!=null)
						{
							
							selectedPlanets[i].SelectedImageOpen(); // seçilen gezegenleri UI ekranında gösteriyoruz
						}
						
					}
					if (selectedPlanets.Count == gettingDestroyedPlanetCount)
					{
						//protonPanel.SetActive(false);
						FireProtonBeams();
					}
				}
			}
		}
	}

	private void FireProtonBeams()
	{
		if (spaceshipTransform == null)
		{
			Debug.LogError("Spaceship Transform is not set.");
			return;
		}
		StartCoroutine(lazerWaitCoroutine());
	   
	
	}

	IEnumerator lazerWaitCoroutine()
	{
		Vector3 spaceshipPosition = spaceshipTransform.position;
   		Debug.Log("Firing proton beams from position: " + spaceshipPosition);
		 List<ISelectable> planetsToProcess = new List<ISelectable>(selectedPlanets);

		foreach (ISelectable planet in planetsToProcess)
		{
			Vector3 planet2DPosition = planet.GetPosition();
			
		// 	Proton ışını efektini uzay gemisinde spawn et
			try
			{
			// Işını uzay gemisinde oluştur ve gezegene doğru yönlendir
				ParticleSystem protonBeam = Instantiate(protonBeamPrefab, spaceshipPosition, Quaternion.identity);
				protonBeam.transform.LookAt(planet2DPosition);
				AudioManager.instance.LazerSound();
				protonBeam.Play();

			// Gezegenleri yok et
				planet.DestroyObj();
	   	 	}
			catch (Exception e)
			{
				Debug.LogError("Error while instantiating proton beam or destroying planet: " + e.Message);
			}

		// 0.3 saniye bekle
			yield return new WaitForSeconds(0.3f);
		}

		selectedPlanets.Clear();
		
		effectObj.SetActive(false);
		SetGameState(selectingGameState); // Tekrar oyun state'ine dön
		
		StartCoroutine(spaceShipMove());
		//spaceShip.LeanMove(secondTarget.transform.position,.4f).setEase(tweenType);
	}
	
	IEnumerator spaceShipMove()
	{
		yield return new WaitForSeconds(2.3f);
		//spaceShip.transform.DOMove(secondTarget.transform.position,.5f);
		spaceShip.LeanMoveLocal(secondTarget.transform.position,.5f).setEase(tweenType);
	}
	
	private void SetMeteorStatepriv()
	{
		SetGameState(GameState.MeteorState);
	}
	private void SetMenu()
	{
		SetGameState(GameState.Menu);
	}

	private void SetGame()
	{
		SetGameState(GameState.Game);
		Debug.Log(gameState);

	}
	private void SetSettings()
	{
		SetGameState(GameState.Settings);

	}
	private void SetBlackHole()
	{
		SetGameState(GameState.BlackHole);
	}

	
	private void SetGameOverMenu()
	{
		//blackhole
		//BlackHoleProcessCallback(BlackHoleobj.transform.position);
		SetGameState(GameState.GameoverMenu);

	}

	public void SetGameState(GameState gameState)
	{
		this.gameState = gameState;
		onGameStateChanged?.Invoke(obj: gameState);
	}

	public GameState GetGameState()
	{
		return gameState;
	}

	public void SetGameState()
	{
		SetGame();
	}

	public bool IsGameState()
	{
		return gameState == GameState.Game;
	}
	public bool IsTimerGameState()
	{
		return gameState == GameState.TimeRace;
	}

	public void SetStartingState()
	{
		SetGameState(startingState);
	}
	public void SetGameoverMenuState()
	{
		SetGameOverMenu();
	}
	public void SetGameoverState()
	{
		SetGameOverMenu();
		
	}

	public void SetSettingsState()
	{
		SetSettings();
	}
	public void SetBlackHoleState()
	{
		SetBlackHole();
	}

	public void SetMeteorState()
	{
		SetMeteorStatepriv();


	}
	
}
