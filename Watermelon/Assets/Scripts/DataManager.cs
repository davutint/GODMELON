using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
	public static DataManager instance;

	[Header("ELEMENTS")]

	private const string ArkaPlanSes = "Sound";
	[SerializeField]private Sprite _openSoundSprite;
	[SerializeField]private Sprite _closeSoundSprite;
	[SerializeField]private Button _soundButton;

	int sound;
	private void Awake()
	{
		instance = this;
	   
	}
   private void Start()
   {
		GetData();
   }
	//burası startta çalışmalı ses manager bunu çekmeli
	public int LoadData()//bunuda oyun basında sesmanagerdan çekip buradaki değerleri ses değeri olarak oyun basında ayarlamalıyız
	{
		int ArkaPlanSesData = PlayerPrefs.GetInt(ArkaPlanSes);
		return ArkaPlanSesData;
	}

	public void Sound()
	{
		AudioManager.instance.ClickSoundCal();
		sound=PlayerPrefs.GetInt("Sound");
		if (sound==1)
		{
			sound=0;
			PlayerPrefs.SetInt("Sound",sound);
			
			AudioManager.instance.AnaSesDurdur();
		}
		else
		{
			sound=1;
			PlayerPrefs.SetInt("Sound",sound);
			
			AudioManager.instance.AnaSesCal();
		}
		GetData();
		Debug.Log(sound);
	}
	
	private void GetData()
	{
		sound=PlayerPrefs.GetInt("Sound");
		if (sound==1)
		{
			_soundButton.image.sprite=_openSoundSprite;
			AudioManager.instance.AnaSesCal();
		}
		else
		{
			_soundButton.image.sprite=_closeSoundSprite;
			AudioManager.instance.AnaSesDurdur();
		}
	}



}
