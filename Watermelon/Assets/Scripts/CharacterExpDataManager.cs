using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterExpDataManager : MonoBehaviour
{
	public LevelConfigSO LevelConfigSO;
	private int _level;
	private int _experience;
	private int _requiredExperience;
	
	[SerializeField]ScoreManager skoreManager;
	[SerializeField]UIManager _uiManager;
	private PlayerData playerData;
	public static CharacterExpDataManager instance;
	private async void Awake()
	{
		instance=this;
		playerData=await GameCloudDatas.LoadData<PlayerData>("playerdata");
		GetLevelDataStart();
		//FirebaseManager.Instance.GetPlayerLevelData(GetLevelDataStart);
		
	}
	

	private void GetLevelDataStart()//burada gereken puan tamamlanınca çağırılacak kod var. OYUNCU BAŞARILI OLURSA ÇALIŞACAK.
	{
		_level=playerData.Level;
		_experience=playerData.XP;
		RequiredExperience(_level);
		Debug.Log("şimdiki level firebaseden çektik  "+_level+" şimdiki exp firebaseden geldi  : "+_experience );
	}
	
	public async void GameOverSetExpLevel(int currentScore)//GAME OVER'DA ÇAĞIR
	{
		_level=playerData.Level;
		
		int newExp=skoreManager.score/20;//skorun 10'a bölünmüş halini karaktere xp olarak veriyoruz
		for (int i = 0; i < LevelConfigSO.MaxLevel; i++)
		{
			if (newExp>=RequiredExperience(_level))
			{
				_level++;
				
			}
			else
				break;
		}
		
		
		
	
		// Oyuncuya exp ver
		playerData.XP+=newExp;
		playerData.Level=_level;
		playerData.Gold+=skoreManager.score/12;
		_uiManager.endGameScoreText.text=skoreManager.score.ToString();
		_uiManager.characterLevelText.text=_level.ToString();
		_uiManager.xpSlider.maxValue=RequiredExperience(_level);//sliderın max value kısmını gereken xp değeri yapıyoruz
		_uiManager.xpSlider.value=newExp; //slider değeri ise şu anki xp değeri oluyor
		_uiManager.SliderBarText.text=newExp+"/"+LevelConfigSO.GetRequiredExp(playerData.Level);//eğer slider düzgün gözükürse anamenü için neededxp kısmınıda çekebiliriz
		_uiManager.GainedExpText.text="Experience Gained: "+newExp.ToString();
		_uiManager.earningGolds.text=(skoreManager.score/12).ToString();
		
		await GameCloudDatas.SaveData(playerData,"playerdata");
	}
	
	public int RequiredExperience(int level)//Level up kısmını bunu kullanarak kontrol et
	{
		_requiredExperience=LevelConfigSO.GetRequiredExp(level);
		Debug.Log("Gereken xp  "+_requiredExperience);
		return _requiredExperience;
		
	}
}