using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public static MenuManager instance;
	[SerializeField]private GameObject _levelSelection;
	[SerializeField]private GameObject _settingsPanel;
	[SerializeField]private Sprite _openSoundSprite;
	[SerializeField]private Sprite _closeSoundSprite;
	[SerializeField]private Button _soundButton;
	[SerializeField]private TextMeshProUGUI _soundText;
	int sound;
	[SerializeField]private Vector3 targetPos;
	[SerializeField]private Vector3 targetPos2;
	[SerializeField]private float tweenTime;
	[SerializeField]LeanTweenType tweenType;
	[SerializeField]AudioSource menuSound;
	public AudioSource clickSound;
	
	[Header("UPGRADES")]
	[SerializeField]private GameObject BlackHoleUpgradePanel;
	[SerializeField]private GameObject ShipUpgradePanel;
	
	[Header("MAIN MENU DATAS UI")]
	public TextMeshProUGUI playerName;
	public TextMeshProUGUI playerLevel;
	public TextMeshProUGUI sliderText;
	public TextMeshProUGUI goldText;
	public Slider playerExpSliderValue;
	public Slider levelSliderValue; // levele eşit olmalı
	[Header("UPGRADES DATA UI")]
	public TextMeshProUGUI BlackHoleUpgradeGoldText;
	public TextMeshProUGUI BlackHoleSizeText;
	[Space(2)]
	public TextMeshProUGUI ShipUpgradeGoldText;
	public TextMeshProUGUI destroyedPlanetCountText;
	
	
	private void Awake()
	{
		instance=this;
		if (PlayerPrefs.HasKey("Sound"))
		{
			
			sound=PlayerPrefs.GetInt("Sound");
		}
		else
		{
			PlayerPrefs.SetInt("Sound", 1);
		}
	   
	}
	private void Start()
	{
		GetData();
		if (sound==1)
		{
			menuSound.Play();
		}
	}
	public void QuitGame()
	{
		clickSound.Play();
		Application.Quit();
	}
	
	public void LevelSelection()
	{
		_levelSelection.SetActive(true);
		clickSound.Play();
	}
	public void BackToMenu()
	{
		_levelSelection.SetActive(false);
		clickSound.Play();
		
	}
	
	public void OpenSettings()
	{
		
		_settingsPanel.LeanMoveLocal(targetPos,tweenTime).setEase(tweenType);
		clickSound.Play();
	}
	
	public void CloseSettings()
	{
		
		_settingsPanel.LeanMoveLocal(targetPos2,tweenTime).setEase(tweenType);
		clickSound.Play();
	}
	
	
	public void MilkyWay()
	{
		
		SceneManager.LoadScene(1);
	}
	public void Andromeda()
	{
		
		SceneManager.LoadScene(2);
	}
	public void Triangled()
	{clickSound.Play();
		SceneManager.LoadScene(3);
	}
	public void TimerLevel()
	{clickSound.Play();
		SceneManager.LoadScene(4);
	}

	public void Sound()
	{
		clickSound.Play();
		sound=PlayerPrefs.GetInt("Sound");
		if (sound==1)
		{
			sound=0;
			PlayerPrefs.SetInt("Sound",sound);
			_soundText.SetText("Sound Off");
			menuSound.Stop();
		}
		else
		{
			sound=1;
			PlayerPrefs.SetInt("Sound",sound);
			_soundText.SetText("Sound On");
			menuSound.Play();
		}
		GetData();
		Debug.Log(sound);
	}
	public void ToggleBlackHoleUpgradePanel()
	{
		if (BlackHoleUpgradePanel.activeSelf)
		{
			BlackHoleUpgradePanel.SetActive(false);
		}
		else
		{
			BlackHoleUpgradePanel.SetActive(true);
			PlanetUnlocker.instance.CheckPlanets();
		}
	}
	public void ToggleShipUpgradePanel()
	{
		if (ShipUpgradePanel.activeSelf)
		{
			ShipUpgradePanel.SetActive(false);
		}
		else
		{
			ShipUpgradePanel.SetActive(true);
		}
	}

	
	
	private void GetData()
	{
		sound=PlayerPrefs.GetInt("Sound");
		if (sound==1)
		{
			_soundButton.image.sprite=_openSoundSprite;
			_soundText.SetText("Sound On");
		}
		else
		{
			_soundButton.image.sprite=_closeSoundSprite;
			_soundText.SetText("Sound Off");
		}
	}
}
