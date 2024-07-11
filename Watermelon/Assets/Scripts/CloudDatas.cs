using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloudDatas : MonoBehaviour
{
	public static CloudDatas instance;
	public PlayerData playerData;
	public BlackHoleData blackHoleData;
	public ShipData shipData;
	
	public GameObject[] planetSlots=null;
	
	
	public LevelConfigSO levelConfigSO;
	
	
   private void Awake()
   {
		instance=this;	
		
   }

	
	public async Task LoadPlayerData()
	{
		try
		{
			
			var allDatas = await CloudSaveService.Instance.Data.Player.LoadAllAsync();

			if (allDatas.ContainsKey("blackhole"))//KARADELİK DEGERLERİ
			{
			 	if (allDatas.TryGetValue("blackhole", out var keyName)) 
				{
					blackHoleData= keyName.Value.GetAs<BlackHoleData>();
					//PlanetUnlocker.instance.CheckPlanets();
		
				}
			}
			else//DEFAULT DEGERLER
			{	
				blackHoleData.size=0.1f;
				blackHoleData.radius=0.1f;
				await GameCenterManager.SaveData(blackHoleData,"blackhole");
			}
			if (allDatas.ContainsKey("ship"))//PLAYER DATA KISMI
			{
				if (allDatas.TryGetValue("ship", out var keyName)) 
				{
					shipData= keyName.Value.GetAs<ShipData>();
				}
			}
			else//DEFAULT DEGERLER
			{
				shipData.destroyPlanetCount=2;
				shipData.speed=100;
				await GameCenterManager.SaveData(shipData,key: "ship");
			}

			if (allDatas.ContainsKey("playerdata"))//PLAYER DATA KISMI
			{
				if (allDatas.TryGetValue("playerdata", out var keyName)) 
				{
					playerData= keyName.Value.GetAs<PlayerData>();
				}
			}
			else//DEFAULT DEGERLER
			{
				playerData.XP=0;
				playerData.Level=0;
				playerData.Gold=250;
				await GameCenterManager.SaveData(playerData,"playerdata");
			}

				// Verileri oyunun gerekli yerlerine atayın
			UpdateDataUI();
			UpdatePlanetSlots();
				
		}
		catch (CloudSaveException e)
		{
			Debug.LogError($"Error loading player data: {e}");
		}
			
	}
	
	  public async void BlackHoleSizeUpgrade(int goldPrice)
   {	
		if (playerData.Gold>=goldPrice&&CloudDatas.instance.blackHoleData.size<1)
		{
			playerData.Gold-=goldPrice;
			blackHoleData.size+=0.1f;
			await GameCenterManager.SaveData(blackHoleData,"blackhole");//yapılan upgrade clouda yoolandı
			await GameCenterManager.SaveData(playerData,"playerdata");
			PlanetUnlocker.instance.CheckPlanets();
			UpdateDataUI();
		}

		//altın yetmediğine dair feedback;
   }
		
	  public async void ShipDestroyCountUpgrade(int goldPrice)
	{	
		if (playerData.Gold>=goldPrice&&shipData.destroyPlanetCount<6)
		{
			playerData.Gold-=goldPrice;
			shipData.destroyPlanetCount+=1;
			await GameCenterManager.SaveData(shipData,"ship");//yapılan upgrade clouda yoolandı
			await GameCenterManager.SaveData(playerData,"playerdata");
			UpdateDataUI();
			UpdatePlanetSlots();
		}

		//altın yetmediğine dair feedback;
	}
	  public async void ShipSpeedUpgrade(int goldPrice)
	{	
		if (playerData.Gold>=goldPrice&&shipData.speed>50)
		{
			playerData.Gold-=goldPrice;
			shipData.speed-=10;//full olduğuna dair feedback lazım
			await GameCenterManager.SaveData(shipData,"ship");//yapılan upgrade clouda yoolandı
			await GameCenterManager.SaveData(playerData,"playerdata");
			UpdateDataUI();
		}

		//altın yetmediğine dair feedback;
	}
	
	public void UpdateDataUI()
	{
		MenuManager.instance.goldText.text=playerData.Gold.ToString();
		MenuManager.instance.BlackHoleUpgradeGoldText.text=playerData.Gold.ToString();
		MenuManager.instance.BlackHoleSizeText.text=blackHoleData.size.ToString();
		MenuManager.instance.playerExpSliderValue.value=playerData.XP;
		MenuManager.instance.playerExpSliderValue.maxValue=levelConfigSO.GetRequiredExp(playerData.Level);
		MenuManager.instance.levelSliderValue.value=playerData.Level;
		
		MenuManager.instance.ShipUpgradeGoldText.text=playerData.Gold.ToString();
		MenuManager.instance.destroyedPlanetCountText.text=shipData.destroyPlanetCount.ToString();
		MenuManager.instance.playerLevel.text=playerData.Level.ToString();
		MenuManager.instance.sliderText.text=playerData.XP.ToString()+"/"+levelConfigSO.GetRequiredExp(playerData.Level);
	}
	
	public void UpdatePlanetSlots()
	{
		if (planetSlots!=null)
		{
			for (int i = 0; i < shipData.destroyPlanetCount; i++)
			{
				planetSlots[i].SetActive(true);
			}
		}
		
		
	}
	
	
}

public struct PlayerData
{
	public int XP;
	public int Level;
	public int Gold;
	
	
}

public struct ShipData
{
	public int speed; //slider değeri olacak
	public int destroyPlanetCount;//yok edebileceği gezegen sayısı
}
public struct BlackHoleData
{
	public float size;
	public float radius;
}
